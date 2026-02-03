using System;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Protocol;

namespace HytaleClient.Data.EntityUI.CombatText
{
	// Token: 0x02000B05 RID: 2821
	public class OpacityAnimationEvent : AnimationEvent
	{
		// Token: 0x0600587B RID: 22651 RVA: 0x001AFE7F File Offset: 0x001AE07F
		public OpacityAnimationEvent(CombatTextEntityUIComponentAnimationEvent animationEvent) : base(animationEvent)
		{
			this.StartOpacity = animationEvent.StartOpacity;
			this.EndOpacity = animationEvent.EndOpacity;
		}

		// Token: 0x0600587C RID: 22652 RVA: 0x001AFEA4 File Offset: 0x001AE0A4
		public override void ApplyAnimationState(ref EntityUIDrawTask task, float progress)
		{
			bool flag = progress < this.StartAt;
			if (flag)
			{
				task.Opacity = new byte?((byte)(255f * this.StartOpacity));
			}
			else
			{
				bool flag2 = progress > this.EndAt;
				if (flag2)
				{
					task.Opacity = new byte?((byte)(255f * this.EndOpacity));
				}
				else
				{
					float num = Math.Abs(this.StartOpacity - this.EndOpacity) * (progress - this.StartAt) / (this.EndAt - this.StartAt);
					task.Opacity = new byte?((byte)(255f * ((this.StartOpacity < this.EndOpacity) ? (this.StartOpacity + num) : (this.EndOpacity - num))));
				}
			}
		}

		// Token: 0x040036F0 RID: 14064
		public float StartOpacity;

		// Token: 0x040036F1 RID: 14065
		public float EndOpacity;
	}
}
