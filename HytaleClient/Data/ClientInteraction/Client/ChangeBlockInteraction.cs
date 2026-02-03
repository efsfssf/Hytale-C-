using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Client
{
	// Token: 0x02000B3B RID: 2875
	internal class ChangeBlockInteraction : SimpleBlockInteraction
	{
		// Token: 0x0600593B RID: 22843 RVA: 0x001B5E90 File Offset: 0x001B4090
		public ChangeBlockInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x0600593C RID: 22844 RVA: 0x001B5E9C File Offset: 0x001B409C
		protected override void InteractWithBlock(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context, BlockPosition targetBlockHit, int blockId)
		{
			int num;
			bool flag = this.Interaction.BlockChanges.TryGetValue(blockId, out num);
			if (flag)
			{
				context.InstanceStore.OldBlockId = blockId;
				context.InstanceStore.ExpectedBlockId = num;
				gameInstance.MapModule.SetClientBlock(targetBlockHit.X, targetBlockHit.Y, targetBlockHit.Z, num);
			}
			else
			{
				context.State.State = 3;
			}
		}

		// Token: 0x0600593D RID: 22845 RVA: 0x001B5F10 File Offset: 0x001B4110
		protected override void Revert0(GameInstance gameInstance, InteractionType type, InteractionContext context)
		{
			base.Revert0(gameInstance, type, context);
			InteractionSyncData state = context.State;
			int oldBlockId = context.InstanceStore.OldBlockId;
			int expectedBlockId = context.InstanceStore.ExpectedBlockId;
			bool flag = state.BlockPosition_ == null || oldBlockId == int.MaxValue;
			if (!flag)
			{
				bool flag2 = gameInstance.MapModule.GetBlock(state.BlockPosition_.X, state.BlockPosition_.Y, state.BlockPosition_.Z, int.MaxValue) != expectedBlockId;
				if (!flag2)
				{
					gameInstance.MapModule.SetClientBlock(state.BlockPosition_.X, state.BlockPosition_.Y, state.BlockPosition_.Z, oldBlockId);
				}
			}
		}
	}
}
