using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.None
{
	// Token: 0x02000B2F RID: 2863
	internal class RemoveEntityInteraction : SimpleInstantInteraction
	{
		// Token: 0x0600590C RID: 22796 RVA: 0x001B42DD File Offset: 0x001B24DD
		public RemoveEntityInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x0600590D RID: 22797 RVA: 0x001B42EC File Offset: 0x001B24EC
		protected override void FirstRun(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			Entity entity = ClientInteraction.GetEntity(gameInstance, context, this.Interaction.EntityTarget);
			bool flag = entity == null;
			if (!flag)
			{
				entity.Removed = true;
			}
		}

		// Token: 0x0600590E RID: 22798 RVA: 0x001B4320 File Offset: 0x001B2520
		protected override void Revert0(GameInstance gameInstance, InteractionType type, InteractionContext context)
		{
			Entity entity = ClientInteraction.GetEntity(gameInstance, context, this.Interaction.EntityTarget);
			bool flag = entity == null;
			if (!flag)
			{
				entity.Removed = false;
			}
		}
	}
}
