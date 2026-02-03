using System;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Camera.Controllers.CameraShake
{
	// Token: 0x0200097B RID: 2427
	internal class TimedCameraShake : CameraShake
	{
		// Token: 0x06004CD0 RID: 19664 RVA: 0x00147749 File Offset: 0x00145949
		public TimedCameraShake(CameraShakeType firstPerson, CameraShakeType thirdPerson, GameInstance gameInstance)
		{
			this._firstPerson = firstPerson;
			this._thirdPerson = thirdPerson;
			this._gameInstance = gameInstance;
		}

		// Token: 0x06004CD1 RID: 19665 RVA: 0x00147768 File Offset: 0x00145968
		protected override bool Update()
		{
			bool flag = !this._gameInstance.App.Settings.CameraShakeEffect;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool isFirstPerson = this._gameInstance.CameraModule.Controller.IsFirstPerson;
				if (isFirstPerson)
				{
					this._intensityMultiplier = this._gameInstance.App.Settings.FirstPersonCameraShakeIntensity;
					this.UpdateActiveShakeType(this._firstPerson);
				}
				else
				{
					this._intensityMultiplier = this._gameInstance.App.Settings.ThirdPersonCameraShakeIntensity;
					this.UpdateActiveShakeType(this._thirdPerson);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06004CD2 RID: 19666 RVA: 0x0014780C File Offset: 0x00145A0C
		protected override Vector3 ComputeOffset(float time, float intensity, Quaternion angle)
		{
			Vector3 result = this._activeShakeType.Offset.Eval(this._seed, time) * intensity * this._timer.Easing;
			Vector3.Transform(ref result, ref angle, out result);
			return result;
		}

		// Token: 0x06004CD3 RID: 19667 RVA: 0x0014785C File Offset: 0x00145A5C
		protected override Vector3 ComputeRotation(float time, float intensity, Quaternion angle)
		{
			return this._activeShakeType.Rotation.Eval(this._seed, time) * intensity * this._timer.Easing;
		}

		// Token: 0x06004CD4 RID: 19668 RVA: 0x0014789C File Offset: 0x00145A9C
		private void UpdateActiveShakeType(CameraShakeType type)
		{
			bool flag = type == this._activeShakeType;
			if (!flag)
			{
				this._activeShakeType = type;
				this._timer.Set(type.Duration, type.EaseIn, type.EaseOut, type.EaseInType, type.EaseOutType);
			}
		}

		// Token: 0x0400284A RID: 10314
		private readonly CameraShakeType _firstPerson;

		// Token: 0x0400284B RID: 10315
		private readonly CameraShakeType _thirdPerson;

		// Token: 0x0400284C RID: 10316
		private readonly GameInstance _gameInstance;
	}
}
