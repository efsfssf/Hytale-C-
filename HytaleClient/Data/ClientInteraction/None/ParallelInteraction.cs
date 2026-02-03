using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.None
{
	// Token: 0x02000B2E RID: 2862
	internal class ParallelInteraction : ClientInteraction
	{
		// Token: 0x06005908 RID: 22792 RVA: 0x001B4231 File Offset: 0x001B2431
		public ParallelInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x06005909 RID: 22793 RVA: 0x001B4240 File Offset: 0x001B2440
		protected override void Tick0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context)
		{
			context.Execute(gameInstance.InteractionModule.RootInteractions[this.Interaction.ChainingNext[0]]);
			for (int i = 1; i < this.Interaction.ChainingNext.Length; i++)
			{
				int rootInteractionId = this.Interaction.ChainingNext[i];
				context.InstanceStore.ForkedChain = context.Fork(context.Duplicate(), rootInteractionId);
			}
			context.State.State = 0;
		}

		// Token: 0x0600590A RID: 22794 RVA: 0x001B42C3 File Offset: 0x001B24C3
		protected override void Revert0(GameInstance gameInstance, InteractionType type, InteractionContext context)
		{
		}

		// Token: 0x0600590B RID: 22795 RVA: 0x001B42C6 File Offset: 0x001B24C6
		protected override void MatchServer0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			this.Tick0(gameInstance, InteractionModule.ClickType.Single, false, true, 0f, type, context);
		}
	}
}
