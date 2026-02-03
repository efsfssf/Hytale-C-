using System;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Protocol;

namespace HytaleClient.Data.EntityUI.CombatText
{
	// Token: 0x02000B06 RID: 2822
	public class PositionAnimationEvent : AnimationEvent
	{
		// Token: 0x0600587D RID: 22653 RVA: 0x001AFF65 File Offset: 0x001AE165
		public PositionAnimationEvent(CombatTextEntityUIComponentAnimationEvent animationEvent) : base(animationEvent)
		{
			this.PositionOffset = animationEvent.PositionOffset;
		}

		// Token: 0x0600587E RID: 22654 RVA: 0x001AFF7C File Offset: 0x001AE17C
		public override void ApplyAnimationState(ref EntityUIDrawTask task, float progress)
		{
			bool flag = progress < this.StartAt || progress > this.EndAt;
			if (!flag)
			{
				float num = (progress - this.StartAt) / (this.EndAt - this.StartAt);
				bool flag2 = this.PositionOffset.X != 0f;
				if (flag2)
				{
					task.TransformationMatrix.M41 = task.TransformationMatrix.M41 + num * this.PositionOffset.X;
				}
				bool flag3 = this.PositionOffset.Y != 0f;
				if (flag3)
				{
					task.TransformationMatrix.M42 = task.TransformationMatrix.M42 + num * this.PositionOffset.Y;
				}
			}
		}

		// Token: 0x040036F2 RID: 14066
		public Vector2f PositionOffset;
	}
}
