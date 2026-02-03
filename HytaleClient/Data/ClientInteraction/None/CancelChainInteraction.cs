using System;
using HytaleClient.Data.ClientInteraction.Client;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.None
{
	// Token: 0x02000B26 RID: 2854
	internal class CancelChainInteraction : SimpleInstantInteraction
	{
		// Token: 0x060058F1 RID: 22769 RVA: 0x001B3117 File Offset: 0x001B1317
		public CancelChainInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x060058F2 RID: 22770 RVA: 0x001B3123 File Offset: 0x001B1323
		protected override void FirstRun(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			ChainingInteraction.NamedSequenceData.Remove(this.Interaction.ChainId);
		}
	}
}
