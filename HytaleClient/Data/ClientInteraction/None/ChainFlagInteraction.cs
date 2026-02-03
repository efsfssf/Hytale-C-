using System;
using HytaleClient.Data.ClientInteraction.Client;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.None
{
	// Token: 0x02000B27 RID: 2855
	internal class ChainFlagInteraction : SimpleInstantInteraction
	{
		// Token: 0x060058F3 RID: 22771 RVA: 0x001B313C File Offset: 0x001B133C
		public ChainFlagInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x060058F4 RID: 22772 RVA: 0x001B3148 File Offset: 0x001B1348
		protected override void FirstRun(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			ChainingInteraction.ChainData chainData;
			bool flag = !ChainingInteraction.NamedSequenceData.TryGetValue(this.Interaction.ChainId, out chainData);
			if (flag)
			{
				ChainingInteraction.NamedSequenceData.Add(this.Interaction.ChainId, chainData = new ChainingInteraction.ChainData());
			}
			chainData.CurrentFlag = this.Interaction.Flag;
		}
	}
}
