using System;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Protocol;

namespace HytaleClient.Data.EntityUI.CombatText
{
	// Token: 0x02000B04 RID: 2820
	public abstract class AnimationEvent
	{
		// Token: 0x06005879 RID: 22649 RVA: 0x001AFE5D File Offset: 0x001AE05D
		public AnimationEvent(CombatTextEntityUIComponentAnimationEvent animationEvent)
		{
			this.StartAt = animationEvent.StartAt;
			this.EndAt = animationEvent.EndAt;
		}

		// Token: 0x0600587A RID: 22650
		public abstract void ApplyAnimationState(ref EntityUIDrawTask task, float progress);

		// Token: 0x040036EE RID: 14062
		public float StartAt;

		// Token: 0x040036EF RID: 14063
		public float EndAt;
	}
}
