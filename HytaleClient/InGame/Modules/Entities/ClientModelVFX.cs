using System;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Entities
{
	// Token: 0x02000944 RID: 2372
	internal class ClientModelVFX
	{
		// Token: 0x06004905 RID: 18693 RVA: 0x0011BD4A File Offset: 0x00119F4A
		public void UpdateCustomAnimation(float customRatio)
		{
			this.AnimationProgress = customRatio;
		}

		// Token: 0x06004906 RID: 18694 RVA: 0x0011BD54 File Offset: 0x00119F54
		public void UpdateAnimation(float frametime)
		{
			bool triggerAnimation = this.TriggerAnimation;
			if (triggerAnimation)
			{
				this._modelVFXAnimationStartTime = frametime;
				this.TriggerAnimation = false;
			}
			float num = 0f;
			float num2 = this.AnimationRange.Y - this.AnimationRange.X;
			switch (this.LoopOption)
			{
			case ClientModelVFX.LoopOptions.PlayOnce:
			{
				float num3 = frametime - this._modelVFXAnimationStartTime;
				bool flag = num3 < this.AnimationDuration;
				if (flag)
				{
					num = num3 * num2 / this.AnimationDuration + this.AnimationRange.X;
				}
				break;
			}
			case ClientModelVFX.LoopOptions.Loop:
			{
				float num3 = (frametime - this._modelVFXAnimationStartTime) % this.AnimationDuration;
				num = num3 * num2 / this.AnimationDuration + this.AnimationRange.X;
				break;
			}
			case ClientModelVFX.LoopOptions.LoopMirror:
			{
				float num3 = (frametime - this._modelVFXAnimationStartTime) % (this.AnimationDuration * 2f);
				bool flag2 = num3 < this.AnimationDuration;
				if (flag2)
				{
					num = num3 * num2 / this.AnimationDuration + this.AnimationRange.X;
				}
				else
				{
					num = (num3 - this.AnimationDuration) * num2 / this.AnimationDuration + this.AnimationRange.X;
					num = this.AnimationRange.Y - num + this.AnimationRange.X;
				}
				break;
			}
			}
			bool flag3 = num != 0f;
			if (flag3)
			{
				this.AnimationProgress = Easing.Ease(this.CurveType, num, 0f, 1f, 1f);
			}
		}

		// Token: 0x04002509 RID: 9481
		public Vector3 HighlightColor;

		// Token: 0x0400250A RID: 9482
		public float HighlightThickness = 1f;

		// Token: 0x0400250B RID: 9483
		public Vector2 NoiseScale;

		// Token: 0x0400250C RID: 9484
		public float AnimationProgress;

		// Token: 0x0400250D RID: 9485
		public int PackedModelVFXParams;

		// Token: 0x0400250E RID: 9486
		public Vector2 NoiseScrollSpeed;

		// Token: 0x0400250F RID: 9487
		public Vector4 PostColor;

		// Token: 0x04002510 RID: 9488
		public int IdInTBO;

		// Token: 0x04002511 RID: 9489
		public string Id;

		// Token: 0x04002512 RID: 9490
		public bool TriggerAnimation;

		// Token: 0x04002513 RID: 9491
		private float _modelVFXAnimationStartTime;

		// Token: 0x04002514 RID: 9492
		public float AnimationDuration;

		// Token: 0x04002515 RID: 9493
		public Vector2 AnimationRange;

		// Token: 0x04002516 RID: 9494
		public ClientModelVFX.LoopOptions LoopOption;

		// Token: 0x04002517 RID: 9495
		public Easing.EasingType CurveType;

		// Token: 0x02000E2B RID: 3627
		public enum EffectDirections
		{
			// Token: 0x04004556 RID: 17750
			None,
			// Token: 0x04004557 RID: 17751
			BottomUp,
			// Token: 0x04004558 RID: 17752
			TopDown,
			// Token: 0x04004559 RID: 17753
			ToCenter,
			// Token: 0x0400455A RID: 17754
			FromCenter
		}

		// Token: 0x02000E2C RID: 3628
		public enum SwitchTo
		{
			// Token: 0x0400455C RID: 17756
			Disappear,
			// Token: 0x0400455D RID: 17757
			PostColor,
			// Token: 0x0400455E RID: 17758
			Distortion
		}

		// Token: 0x02000E2D RID: 3629
		public enum LoopOptions
		{
			// Token: 0x04004560 RID: 17760
			PlayOnce,
			// Token: 0x04004561 RID: 17761
			Loop,
			// Token: 0x04004562 RID: 17762
			LoopMirror
		}
	}
}
