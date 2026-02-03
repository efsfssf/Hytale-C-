using System;
using HytaleClient.Data.ClientInteraction.Client;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.None
{
	// Token: 0x02000B29 RID: 2857
	internal class ClearChainVariableInteraction : SimpleInstantInteraction
	{
		// Token: 0x060058F9 RID: 22777 RVA: 0x001B33E4 File Offset: 0x001B15E4
		public ClearChainVariableInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x060058FA RID: 22778 RVA: 0x001B33F0 File Offset: 0x001B15F0
		protected override void FirstRun(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			ChainingInteraction.ChainData chainData;
			bool flag = !ChainingInteraction.NamedSequenceData.TryGetValue(this.Interaction.ChainId, out chainData);
			if (flag)
			{
				ChainingInteraction.NamedSequenceData.Add(this.Interaction.ChainId, chainData = new ChainingInteraction.ChainData());
			}
			chainData.Variable = string.Empty;
		}
	}
}
