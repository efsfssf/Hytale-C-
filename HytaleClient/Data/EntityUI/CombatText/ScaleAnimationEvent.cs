using System;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Protocol;

namespace HytaleClient.Data.EntityUI.CombatText
{
	// Token: 0x02000B07 RID: 2823
	public class ScaleAnimationEvent : AnimationEvent
	{
		// Token: 0x0600587F RID: 22655 RVA: 0x001B0029 File Offset: 0x001AE229
		public ScaleAnimationEvent(CombatTextEntityUIComponentAnimationEvent animationEvent) : base(animationEvent)
		{
			this.StartScale = animationEvent.StartScale;
			this.EndScale = animationEvent.EndScale;
		}

		// Token: 0x06005880 RID: 22656 RVA: 0x001B004C File Offset: 0x001AE24C
		public override void ApplyAnimationState(ref EntityUIDrawTask task, float progress)
		{
			bool flag = progress < this.StartAt;
			if (flag)
			{
				task.Scale = new float?(this.StartScale);
			}
			else
			{
				bool flag2 = progress > this.EndAt;
				if (flag2)
				{
					task.Scale = new float?(this.EndScale);
				}
				else
				{
					float num = Math.Abs(this.StartScale - this.EndScale) * (progress - this.StartAt) / (this.EndAt - this.StartAt);
					task.Scale = new float?((this.StartScale < this.EndScale) ? (this.EndScale + num) : (this.StartScale - num));
				}
			}
		}

		// Token: 0x040036F3 RID: 14067
		public float StartScale;

		// Token: 0x040036F4 RID: 14068
		public float EndScale;
	}
}
