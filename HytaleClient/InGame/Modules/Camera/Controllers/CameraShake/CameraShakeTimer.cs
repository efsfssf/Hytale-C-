using System;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Camera.Controllers.CameraShake
{
	// Token: 0x02000979 RID: 2425
	public class CameraShakeTimer
	{
		// Token: 0x17001264 RID: 4708
		// (get) Token: 0x06004CC0 RID: 19648 RVA: 0x001473BB File Offset: 0x001455BB
		// (set) Token: 0x06004CC1 RID: 19649 RVA: 0x001473C3 File Offset: 0x001455C3
		public float Time { get; private set; }

		// Token: 0x17001265 RID: 4709
		// (get) Token: 0x06004CC2 RID: 19650 RVA: 0x001473CC File Offset: 0x001455CC
		// (set) Token: 0x06004CC3 RID: 19651 RVA: 0x001473D4 File Offset: 0x001455D4
		public float Easing { get; private set; }

		// Token: 0x06004CC4 RID: 19652 RVA: 0x001473E0 File Offset: 0x001455E0
		public bool HasStarted()
		{
			return this.Time > 0f;
		}

		// Token: 0x06004CC5 RID: 19653 RVA: 0x00147400 File Offset: 0x00145600
		public bool IsComplete()
		{
			return this.Time > this._completionTime;
		}

		// Token: 0x06004CC6 RID: 19654 RVA: 0x00147420 File Offset: 0x00145620
		public void Restart()
		{
			this.Time = 0f;
			this.Easing = 0f;
			this._holdTime = this._easeInDuration + this._duration;
			this._completionTime = this._easeInDuration + this._duration + this._easeOutDuration;
		}

		// Token: 0x06004CC7 RID: 19655 RVA: 0x00147473 File Offset: 0x00145673
		public void Stop()
		{
			this.Time = this._completionTime;
			this.Easing = 0f;
		}

		// Token: 0x06004CC8 RID: 19656 RVA: 0x0014748F File Offset: 0x0014568F
		public void Extend()
		{
			this._holdTime += this._duration;
			this._completionTime = this._holdTime + this._easeOutDuration;
		}

		// Token: 0x06004CC9 RID: 19657 RVA: 0x001474B8 File Offset: 0x001456B8
		public void Set(float duration, float easeIn, float easeOut = 0f, Easing.EasingType easeInType = HytaleClient.Math.Easing.EasingType.Linear, Easing.EasingType easeOutType = HytaleClient.Math.Easing.EasingType.Linear)
		{
			this._duration = duration;
			this._easeInDuration = easeIn;
			this._easeOutDuration = easeOut;
			this._easeInType = easeInType;
			this._easeOutType = easeOutType;
			this._holdTime = easeIn + duration;
			this._completionTime = easeIn + duration + easeOut;
		}

		// Token: 0x06004CCA RID: 19658 RVA: 0x001474F4 File Offset: 0x001456F4
		public void Tick(float deltaTime)
		{
			bool flag = this.Time > this._completionTime;
			if (!flag)
			{
				this.Time += deltaTime;
				bool flag2 = this._easeInDuration > 0f && this.Time < this._easeInDuration;
				if (flag2)
				{
					this.Easing = HytaleClient.Math.Easing.Ease(this._easeInType, this.Time / this._easeInDuration, 0f, 1f, 1f);
				}
				else
				{
					bool flag3 = this.Time <= this._holdTime;
					if (flag3)
					{
						this.Easing = 1f;
					}
					else
					{
						bool flag4 = this._easeOutDuration > 0f;
						if (flag4)
						{
							this.Easing = HytaleClient.Math.Easing.Ease(this._easeOutType, (this._completionTime - this.Time) / this._easeOutDuration, 0f, 1f, 1f);
						}
					}
				}
			}
		}

		// Token: 0x04002837 RID: 10295
		private float _duration;

		// Token: 0x04002838 RID: 10296
		private float _easeInDuration;

		// Token: 0x04002839 RID: 10297
		private float _easeOutDuration;

		// Token: 0x0400283A RID: 10298
		private float _holdTime;

		// Token: 0x0400283B RID: 10299
		private float _completionTime;

		// Token: 0x0400283C RID: 10300
		private Easing.EasingType _easeInType;

		// Token: 0x0400283D RID: 10301
		private Easing.EasingType _easeOutType;
	}
}
