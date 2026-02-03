using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.None
{
	// Token: 0x02000B34 RID: 2868
	internal class SerialInteraction : ClientInteraction
	{
		// Token: 0x0600591F RID: 22815 RVA: 0x001B4C9A File Offset: 0x001B2E9A
		public SerialInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x06005920 RID: 22816 RVA: 0x001B4CA6 File Offset: 0x001B2EA6
		protected override void Tick0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context)
		{
		}

		// Token: 0x06005921 RID: 22817 RVA: 0x001B4CA9 File Offset: 0x001B2EA9
		protected override void Revert0(GameInstance gameInstance, InteractionType type, InteractionContext context)
		{
		}

		// Token: 0x06005922 RID: 22818 RVA: 0x001B4CAC File Offset: 0x001B2EAC
		protected override void MatchServer0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
		}

		// Token: 0x06005923 RID: 22819 RVA: 0x001B4CB0 File Offset: 0x001B2EB0
		public override void Compile(InteractionModule module, ClientRootInteraction.OperationsBuilder builder)
		{
			foreach (int num in this.Interaction.SerialInteractions)
			{
				module.Interactions[num].Compile(module, builder);
			}
		}
	}
}
