using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.None
{
	// Token: 0x02000B32 RID: 2866
	internal class RunRootInteraction : SimpleInstantInteraction
	{
		// Token: 0x06005917 RID: 22807 RVA: 0x001B4669 File Offset: 0x001B2869
		public RunRootInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x06005918 RID: 22808 RVA: 0x001B4675 File Offset: 0x001B2875
		protected override void FirstRun(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			context.Execute(gameInstance.InteractionModule.RootInteractions[this.Interaction.RootInteraction]);
			context.State.State = 0;
		}

		// Token: 0x06005919 RID: 22809 RVA: 0x001B46A4 File Offset: 0x001B28A4
		protected override void MatchServer0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			context.State.State = context.ServerData.State;
			bool flag = context.State.State == 0;
			if (flag)
			{
				context.Execute(gameInstance.InteractionModule.RootInteractions[context.ServerData.EnteredRootInteraction]);
			}
			base.MatchServer0(gameInstance, clickType, hasAnyButtonClick, type, context);
		}
	}
}
