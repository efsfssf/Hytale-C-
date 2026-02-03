using System;
using HytaleClient.Data.Map;
using HytaleClient.Graphics.Particles;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.InGame.Modules.Map;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Client
{
	// Token: 0x02000B39 RID: 2873
	internal class BreakBlockInteraction : SimpleBlockInteraction
	{
		// Token: 0x06005931 RID: 22833 RVA: 0x001B5685 File Offset: 0x001B3885
		public BreakBlockInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x06005932 RID: 22834 RVA: 0x001B5694 File Offset: 0x001B3894
		protected override void InteractWithBlock(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context, BlockPosition targetBlockHit, int blockId)
		{
			ClientBlockType clientBlockType = gameInstance.MapModule.ClientBlockTypes[blockId];
			BlockType.BlockGathering gathering = clientBlockType.Gathering;
			bool flag = ((gathering != null) ? gathering.Soft : null) != null;
			BlockType.BlockGathering gathering2 = clientBlockType.Gathering;
			bool flag2 = ((gathering2 != null) ? gathering2.Harvest : null) != null;
			int x = targetBlockHit.X;
			int y = targetBlockHit.Y;
			int z = targetBlockHit.Z;
			bool flag3 = gameInstance.GameMode == 1;
			if (flag3)
			{
				bool harvest = this.Interaction.Harvest;
				if (harvest)
				{
					bool flag4 = flag2;
					if (flag4)
					{
						BreakBlockInteraction.SimulateBreakBlock(context, gameInstance, x, y, z, blockId, clientBlockType);
					}
				}
				else
				{
					BreakBlockInteraction.SimulateBreakBlock(context, gameInstance, x, y, z, blockId, clientBlockType);
				}
			}
			else
			{
				bool flag5 = !this.Interaction.Harvest && !flag;
				if (flag5)
				{
					bool flag6 = clientBlockType.DrawType > 0;
					if (flag6)
					{
					}
					bool flag7 = clientBlockType.BlockParticleSetId != null;
					if (flag7)
					{
						Vector3 blockPosition = gameInstance.InteractionModule.TargetBlockHit.BlockPosition;
						ParticleSystemProxy particleSystemProxy;
						bool flag8 = gameInstance.ParticleSystemStoreModule.TrySpawnBlockSystem(blockPosition, clientBlockType, ClientBlockParticleEvent.Hit, out particleSystemProxy, true, false);
						if (flag8)
						{
							particleSystemProxy.Position = blockPosition + new Vector3(0.5f) + particleSystemProxy.Position;
						}
					}
				}
				bool harvest2 = this.Interaction.Harvest;
				if (harvest2)
				{
					bool flag9 = flag2;
					if (flag9)
					{
						BreakBlockInteraction.SimulateBreakBlock(context, gameInstance, x, y, z, blockId, clientBlockType);
					}
				}
				else
				{
					bool flag10 = flag;
					if (flag10)
					{
						BreakBlockInteraction.SimulateBreakBlock(context, gameInstance, x, y, z, blockId, clientBlockType);
					}
				}
			}
		}

		// Token: 0x06005933 RID: 22835 RVA: 0x001B582C File Offset: 0x001B3A2C
		private static void SimulateBreakBlock(InteractionContext context, GameInstance gameInstance, int targetBlockX, int targetBlockY, int targetBlockZ, int blockId, ClientBlockType blockType)
		{
			context.InstanceStore.OldBlockId = blockId;
			bool flag = blockType.VariantOriginalId != 0 && blockType.FluidBlockId != 0 && blockType.FluidBlockId != blockId;
			if (flag)
			{
				context.InstanceStore.ExpectedBlockId = blockType.FluidBlockId;
				gameInstance.InjectPacket(new ServerSetBlock(targetBlockX, targetBlockY, targetBlockZ, blockType.FluidBlockId, false));
			}
			else
			{
				context.InstanceStore.ExpectedBlockId = 0;
				gameInstance.InjectPacket(new ServerSetBlock(targetBlockX, targetBlockY, targetBlockZ, 0, false));
			}
		}

		// Token: 0x06005934 RID: 22836 RVA: 0x001B58BC File Offset: 0x001B3ABC
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
