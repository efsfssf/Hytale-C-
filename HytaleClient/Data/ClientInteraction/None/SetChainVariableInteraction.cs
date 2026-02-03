using System;
using HytaleClient.Data.ClientInteraction.Client;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.None
{
	// Token: 0x02000B35 RID: 2869
	internal class SetChainVariableInteraction : SimpleInstantInteraction
	{
		// Token: 0x06005924 RID: 22820 RVA: 0x001B4CEF File Offset: 0x001B2EEF
		public SetChainVariableInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x06005925 RID: 22821 RVA: 0x001B4CFC File Offset: 0x001B2EFC
		protected override void FirstRun(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			ChainingInteraction.ChainData chainData;
			bool flag = !ChainingInteraction.NamedSequenceData.TryGetValue(this.Interaction.ChainId, out chainData);
			if (flag)
			{
				ChainingInteraction.NamedSequenceData.Add(this.Interaction.ChainId, chainData = new ChainingInteraction.ChainData());
			}
			chainData.Variable = this.Interaction.Variable;
		}
	}
}
