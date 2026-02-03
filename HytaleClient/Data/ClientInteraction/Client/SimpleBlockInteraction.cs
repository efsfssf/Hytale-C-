using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Client
{
	// Token: 0x02000B49 RID: 2889
	internal class SimpleBlockInteraction : SimpleInstantInteraction
	{
		// Token: 0x06005988 RID: 22920 RVA: 0x001BA4A5 File Offset: 0x001B86A5
		public SimpleBlockInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x06005989 RID: 22921 RVA: 0x001BA4B4 File Offset: 0x001B86B4
		protected override void FirstRun(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			bool flag = context.MetaStore.TargetBlockRaw == null;
			if (flag)
			{
				context.State.State = 3;
			}
			else
			{
				BlockPosition targetBlock = context.MetaStore.TargetBlock;
				context.State.BlockPosition_ = context.MetaStore.TargetBlockRaw;
				int block = gameInstance.MapModule.GetBlock(targetBlock.X, targetBlock.Y, targetBlock.Z, 1);
				bool flag2 = block == 1 || block == 0;
				if (flag2)
				{
					context.State.State = 3;
				}
				else
				{
					this.InteractWithBlock(gameInstance, clickType, hasAnyButtonClick, type, context, context.State.BlockPosition_, block);
				}
			}
		}

		// Token: 0x0600598A RID: 22922 RVA: 0x001BA563 File Offset: 0x001B8763
		protected virtual void InteractWithBlock(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context, BlockPosition targetBlockHit, int blockId)
		{
		}
	}
}
