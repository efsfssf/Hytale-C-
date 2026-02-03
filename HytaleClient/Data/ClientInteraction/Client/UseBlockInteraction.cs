using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Client
{
	// Token: 0x02000B4B RID: 2891
	internal class UseBlockInteraction : SimpleBlockInteraction
	{
		// Token: 0x0600598E RID: 22926 RVA: 0x001BA751 File Offset: 0x001B8951
		public UseBlockInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x0600598F RID: 22927 RVA: 0x001BA760 File Offset: 0x001B8960
		protected override void InteractWithBlock(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context, BlockPosition targetBlockHit, int blockId)
		{
			int num;
			bool flag = gameInstance.MapModule.ClientBlockTypes[blockId].Interactions.TryGetValue(type, out num);
			if (flag)
			{
				context.State.State = 0;
				context.Execute(gameInstance.InteractionModule.RootInteractions[num]);
			}
			else
			{
				context.State.State = 3;
			}
		}

		// Token: 0x06005990 RID: 22928 RVA: 0x001BA7C4 File Offset: 0x001B89C4
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
