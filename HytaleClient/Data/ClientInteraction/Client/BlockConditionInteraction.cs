using System;
using System.Collections.Generic;
using HytaleClient.Data.Items;
using HytaleClient.Data.Map;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;

namespace HytaleClient.Data.ClientInteraction.Client
{
	// Token: 0x02000B38 RID: 2872
	internal class BlockConditionInteraction : SimpleBlockInteraction
	{
		// Token: 0x0600592F RID: 22831 RVA: 0x001B546E File Offset: 0x001B366E
		public BlockConditionInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x06005930 RID: 22832 RVA: 0x001B547C File Offset: 0x001B367C
		protected override void InteractWithBlock(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context, BlockPosition targetBlockHit, int blockId)
		{
			Vector3 normal = gameInstance.InteractionModule.TargetBlockHit.Normal;
			BlockFace blockFace = RotationHelper.FromNormal(normal);
			ClientBlockType clientBlockType = gameInstance.MapModule.ClientBlockTypes[blockId];
			ClientItemBase item = gameInstance.ItemLibraryModule.GetItem(clientBlockType.Item);
			bool flag = item == null;
			if (flag)
			{
				context.State.State = 3;
			}
			else
			{
				bool flag2 = false;
				context.State.BlockFace_ = blockFace;
				BlockMatcher[] matchers = this.Interaction.Matchers;
				int i = 0;
				while (i < matchers.Length)
				{
					BlockMatcher blockMatcher = matchers[i];
					bool flag3 = blockMatcher.Face > 0;
					if (!flag3)
					{
						goto IL_D3;
					}
					BlockFace blockFace2 = blockMatcher.Face;
					bool flag4 = !blockMatcher.StaticFace;
					if (flag4)
					{
						blockFace2 = RotationHelper.RotateBlockFace(blockFace2, clientBlockType);
					}
					bool flag5 = blockFace2 != blockFace;
					if (!flag5)
					{
						goto IL_D3;
					}
					IL_1C3:
					i++;
					continue;
					IL_D3:
					bool flag6 = blockMatcher.Block != null;
					if (flag6)
					{
						bool flag7 = blockMatcher.Block.Id != null && blockMatcher.Block.Id != item.Id;
						if (flag7)
						{
							goto IL_1C3;
						}
						bool flag8 = blockMatcher.Block.State != null;
						if (flag8)
						{
							string text = null;
							Dictionary<int, string> statesReverse = clientBlockType.StatesReverse;
							if (statesReverse != null)
							{
								statesReverse.TryGetValue(blockId, out text);
							}
							text = (text ?? "default");
							bool flag9 = blockMatcher.Block.State != text;
							if (flag9)
							{
								goto IL_1C3;
							}
						}
						bool flag10 = blockMatcher.Block.TagIndex != int.MinValue;
						if (flag10)
						{
							bool flag11 = clientBlockType.TagIndexes == null || !clientBlockType.TagIndexes.ContainsKey(blockMatcher.Block.TagIndex);
							if (flag11)
							{
								goto IL_1C3;
							}
						}
					}
					flag2 = true;
					break;
				}
				bool flag12 = flag2;
				if (flag12)
				{
					context.State.State = 0;
				}
				else
				{
					context.State.State = 3;
				}
			}
		}
	}
}
