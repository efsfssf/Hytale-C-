using System;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules.Camera.Controllers.CameraShake
{
	// Token: 0x02000977 RID: 2423
	public abstract class CameraShake
	{
		// Token: 0x06004CA5 RID: 19621 RVA: 0x00146D8C File Offset: 0x00144F8C
		public CameraShake()
		{
			this._seed = Environment.TickCount;
		}

		// Token: 0x06004CA6 RID: 19622 RVA: 0x00146E04 File Offset: 0x00145004
		public void Update(float time, float deltaTime, Quaternion angle)
		{
			bool flag = !this.Update();
			if (!flag)
			{
				this.UpdateIntensity();
				this._timer.Tick(deltaTime);
				float time2 = this.GetTimeStep(time) * this._frequency;
				float intensity = this._intensity * this._intensityMultiplier;
				this._offset = this.ComputeOffset(time2, intensity, angle);
				this._rotation = this.ComputeRotation(time2, intensity, angle);
			}
		}

		// Token: 0x06004CA7 RID: 19623 RVA: 0x00146E70 File Offset: 0x00145070
		public void AddShake(ref Vector3 offset, ref Vector3 rotation)
		{
			offset += this._offset;
			rotation += this._rotation;
		}

		// Token: 0x06004CA8 RID: 19624 RVA: 0x00146EA1 File Offset: 0x001450A1
		public void SetIntensity(float intensity)
		{
			this._intensity = intensity;
			this._targetIntensity = intensity;
		}

		// Token: 0x06004CA9 RID: 19625 RVA: 0x00146EB4 File Offset: 0x001450B4
		public void SetIntensityTarget(float targetIntensity, AccumulationMode accumulationMode)
		{
			switch (accumulationMode)
			{
			case 0:
				this._targetIntensity = targetIntensity;
				break;
			case 1:
				this._targetIntensity += targetIntensity;
				break;
			case 2:
				this._targetIntensity = (this._targetIntensity + targetIntensity) * 0.5f;
				break;
			default:
				throw new ArgumentOutOfRangeException("accumulationMode", accumulationMode, null);
			}
		}

		// Token: 0x06004CAA RID: 19626 RVA: 0x00146F1C File Offset: 0x0014511C
		public float GetTimeStep(float time)
		{
			return this._activeShakeType.Continuous ? time : (this._activeShakeType.StartTime + this._timer.Time);
		}

		// Token: 0x06004CAB RID: 19627 RVA: 0x00146F58 File Offset: 0x00145158
		public virtual bool IsActive()
		{
			return this._timer.HasStarted();
		}

		// Token: 0x06004CAC RID: 19628 RVA: 0x00146F78 File Offset: 0x00145178
		public virtual bool IsComplete()
		{
			return this._timer.IsComplete();
		}

		// Token: 0x06004CAD RID: 19629 RVA: 0x00146F95 File Offset: 0x00145195
		public virtual void Stop()
		{
			this.Reset();
			this._timer.Stop();
		}

		// Token: 0x06004CAE RID: 19630 RVA: 0x00146FAB File Offset: 0x001451AB
		public virtual void ExtendDuration()
		{
			this._timer.Extend();
		}

		// Token: 0x06004CAF RID: 19631 RVA: 0x00146FBC File Offset: 0x001451BC
		public virtual void Reset()
		{
			this._timer.Restart();
			this._frequency = 1f;
			this._intensity = 1f;
			this._targetIntensity = 1f;
			this._intensityMultiplier = 1f;
			this._offset = Vector3.Zero;
			this._rotation = Vector3.Zero;
			this._activeShakeType = CameraShakeType.None;
		}

		// Token: 0x06004CB0 RID: 19632
		protected abstract bool Update();

		// Token: 0x06004CB1 RID: 19633
		protected abstract Vector3 ComputeOffset(float time, float intensity, Quaternion angle);

		// Token: 0x06004CB2 RID: 19634
		protected abstract Vector3 ComputeRotation(float time, float intensity, Quaternion angle);

		// Token: 0x06004CB3 RID: 19635 RVA: 0x00147024 File Offset: 0x00145224
		private void UpdateIntensity()
		{
			bool flag = !MathHelper.WithinEpsilon(this._intensity, this._targetIntensity);
			if (flag)
			{
				this._intensity = MathHelper.Lerp(this._intensity, this._targetIntensity, 0.05f);
			}
		}

		// Token: 0x04002825 RID: 10277
		private const float IntensityLerpRate = 0.05f;

		// Token: 0x04002826 RID: 10278
		protected Vector3 _offset = Vector3.Zero;

		// Token: 0x04002827 RID: 10279
		protected Vector3 _rotation = Vector3.Zero;

		// Token: 0x04002828 RID: 10280
		protected float _frequency = 1f;

		// Token: 0x04002829 RID: 10281
		protected float _intensity = 1f;

		// Token: 0x0400282A RID: 10282
		protected float _targetIntensity = 1f;

		// Token: 0x0400282B RID: 10283
		protected float _intensityMultiplier = 1f;

		// Token: 0x0400282C RID: 10284
		protected CameraShakeType _activeShakeType = CameraShakeType.None;

		// Token: 0x0400282D RID: 10285
		protected readonly int _seed;

		// Token: 0x0400282E RID: 10286
		protected readonly CameraShakeTimer _timer = new CameraShakeTimer();
	}
}
