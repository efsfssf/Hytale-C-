using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction
{
	// Token: 0x02000B13 RID: 2835
	internal abstract class SimpleInstantInteraction : SimpleInteraction
	{
		// Token: 0x060058B2 RID: 22706 RVA: 0x001B1DC4 File Offset: 0x001AFFC4
		public SimpleInstantInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x060058B3 RID: 22707 RVA: 0x001B1DD0 File Offset: 0x001AFFD0
		protected override void Tick0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context)
		{
			bool flag = !firstRun;
			if (!flag)
			{
				this.FirstRun(gameInstance, clickType, hasAnyButtonClick, type, context);
				base.Tick0(gameInstance, clickType, hasAnyButtonClick, firstRun, time, type, context);
			}
		}

		// Token: 0x060058B4 RID: 22708
		protected abstract void FirstRun(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context);
	}
}
