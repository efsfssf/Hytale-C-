using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HytaleClient.Data.Map;
using HytaleClient.Math;
using NLog;

namespace HytaleClient.Data.BlockyModels
{
	// Token: 0x02000B68 RID: 2920
	internal class BlockyModel
	{
		// Token: 0x17001372 RID: 4978
		// (get) Token: 0x060059F0 RID: 23024 RVA: 0x001BE369 File Offset: 0x001BC569
		// (set) Token: 0x060059F1 RID: 23025 RVA: 0x001BE371 File Offset: 0x001BC571
		public int NodeCount { get; private set; }

		// Token: 0x060059F2 RID: 23026 RVA: 0x001BE37C File Offset: 0x001BC57C
		public BlockyModel(int preAllocatedNodeCount)
		{
			this.AllNodes = new BlockyModelNode[preAllocatedNodeCount];
			this.ParentNodes = new int[preAllocatedNodeCount];
		}

		// Token: 0x060059F3 RID: 23027 RVA: 0x001BE3E0 File Offset: 0x001BC5E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddNode(ref BlockyModelNode node, int parentNodeIndex = -1)
		{
			bool flag = this.NodeCount >= BlockyModel.MaxNodeCount;
			if (flag)
			{
				BlockyModel.Logger.Warn("Trying to setup a model with more than {0} nodes", BlockyModel.MaxNodeCount);
			}
			else
			{
				this.EnsureNodeCountAllocated(1, 5);
				this.ParentNodes[this.NodeCount] = parentNodeIndex;
				bool flag2 = parentNodeIndex != -1;
				if (flag2)
				{
					this.AllNodes[parentNodeIndex].Children.Add(this.NodeCount);
				}
				else
				{
					this.RootNodes.Add(this.NodeCount);
				}
				this.AllNodes[this.NodeCount] = node;
				bool flag3 = !this.NodeIndicesByNameId.ContainsKey(node.NameId);
				if (flag3)
				{
					this.NodeIndicesByNameId[node.NameId] = this.NodeCount;
				}
				int nodeCount = this.NodeCount;
				this.NodeCount = nodeCount + 1;
			}
		}

		// Token: 0x060059F4 RID: 23028 RVA: 0x001BE4CC File Offset: 0x001BC6CC
		public void AddMapBlockNode(ClientBlockType clientBlockType, int blockNameId, int sideMaskNameId, int mapAtlasWidth)
		{
			this.EnsureNodeCountAllocated(2, 0);
			float num = (float)clientBlockType.VerticalFill / (float)((clientBlockType.MaxFillLevel == 0) ? 8 : clientBlockType.MaxFillLevel);
			BlockyModelNode blockyModelNode = BlockyModelNode.CreateMapBlockNode(blockNameId, 16f * num, num);
			blockyModelNode.ShadingMode = clientBlockType.CubeShadingMode;
			for (int i = 0; i < 6; i++)
			{
				int num2;
				switch (i)
				{
				case 0:
					num2 = 5;
					break;
				case 1:
					num2 = 3;
					break;
				case 2:
					num2 = 4;
					break;
				case 3:
					num2 = 2;
					break;
				case 4:
					num2 = 0;
					break;
				case 5:
					num2 = 1;
					break;
				default:
					throw new Exception("Can't be reached");
				}
				int num3 = clientBlockType.CubeTextures[i].TileLinearPositionsInAtlas[0] * 32;
				blockyModelNode.TextureLayout[num2].Offset.X = num3 % mapAtlasWidth;
				blockyModelNode.TextureLayout[num2].Offset.Y = num3 / mapAtlasWidth * 32;
				bool flag = i >= 2;
				if (flag)
				{
					BlockyModelFaceTextureLayout[] textureLayout = blockyModelNode.TextureLayout;
					int num4 = num2;
					textureLayout[num4].Offset.Y = textureLayout[num4].Offset.Y + (int)((1f - num) * 32f);
				}
			}
			this.AddNode(ref blockyModelNode, -1);
			int num5 = clientBlockType.CubeSideMaskTextureAtlasIndex;
			bool flag2 = num5 == -1;
			if (!flag2)
			{
				BlockyModelNode blockyModelNode2 = BlockyModelNode.CreateMapBlockNode(sideMaskNameId, 0f, num);
				num5 *= 32;
				for (int j = 0; j < 6; j++)
				{
					int num6;
					switch (j)
					{
					case 0:
						num6 = 5;
						break;
					case 1:
						num6 = 3;
						break;
					case 2:
						num6 = 4;
						break;
					case 3:
						num6 = 2;
						break;
					case 4:
						num6 = 0;
						break;
					case 5:
						num6 = 1;
						break;
					default:
						throw new Exception("Can't be reached");
					}
					bool flag3 = j == 0 || j == 1;
					if (flag3)
					{
						blockyModelNode2.TextureLayout[num6].Hidden = true;
					}
					else
					{
						blockyModelNode2.TextureLayout[num6].Offset.X = num5 % mapAtlasWidth;
						blockyModelNode2.TextureLayout[num6].Offset.Y = (num5 / mapAtlasWidth + (int)(1f - num)) * 32;
					}
				}
				this.AddNode(ref blockyModelNode2, this.NodeCount - 1);
			}
		}

		// Token: 0x060059F5 RID: 23029 RVA: 0x001BE730 File Offset: 0x001BC930
		public BlockyModel Clone()
		{
			BlockyModel blockyModel = new BlockyModel(this.NodeCount);
			for (int i = 0; i < this.NodeCount; i++)
			{
				BlockyModelNode blockyModelNode = this.AllNodes[i].Clone();
				blockyModel.AddNode(ref blockyModelNode, this.ParentNodes[i]);
			}
			return blockyModel;
		}

		// Token: 0x060059F6 RID: 23030 RVA: 0x001BE78C File Offset: 0x001BC98C
		public BlockyModel CloneArmsAndLegs(int rightArmNameId, int rightForeamrNameId, int leftArmNameId, int leftForearmNameId, int rightThighNameId, int leftThighNameId)
		{
			BlockyModel blockyModel = new BlockyModel(BlockyModel.MaxNodeCount);
			int originalNodeIndex;
			bool flag = this.NodeIndicesByNameId.TryGetValue(rightArmNameId, out originalNodeIndex);
			if (flag)
			{
				BlockyModel.RecurseCloneNode(this, blockyModel, originalNodeIndex, -1);
				int num = blockyModel.NodeIndicesByNameId[rightArmNameId];
				ref BlockyModelNode ptr = ref blockyModel.AllNodes[num];
				ptr.Position.X = 0f;
				ptr.Position.Y = 0f;
				ptr.Position.Z = -32f;
				ptr.Orientation = Quaternion.Identity;
				int num2;
				bool flag2 = blockyModel.NodeIndicesByNameId.TryGetValue(rightForeamrNameId, out num2);
				if (flag2)
				{
					blockyModel.AllNodes[num2].Orientation = Quaternion.Identity;
				}
			}
			int originalNodeIndex2;
			bool flag3 = this.NodeIndicesByNameId.TryGetValue(leftArmNameId, out originalNodeIndex2);
			if (flag3)
			{
				BlockyModel.RecurseCloneNode(this, blockyModel, originalNodeIndex2, -1);
				int num3 = blockyModel.NodeIndicesByNameId[leftArmNameId];
				ref BlockyModelNode ptr2 = ref blockyModel.AllNodes[num3];
				ptr2.Position.X = 0f;
				ptr2.Position.Y = 0f;
				ptr2.Position.Z = -32f;
				ptr2.Orientation = Quaternion.Identity;
				int num4;
				bool flag4 = blockyModel.NodeIndicesByNameId.TryGetValue(leftForearmNameId, out num4);
				if (flag4)
				{
					blockyModel.AllNodes[num4].Orientation = Quaternion.Identity;
				}
			}
			int originalNodeIndex3;
			bool flag5 = this.NodeIndicesByNameId.TryGetValue(rightThighNameId, out originalNodeIndex3);
			if (flag5)
			{
				BlockyModel.RecurseCloneNode(this, blockyModel, originalNodeIndex3, -1);
				int num5 = blockyModel.NodeIndicesByNameId[rightThighNameId];
				ref BlockyModelNode ptr3 = ref blockyModel.AllNodes[num5];
				ptr3.Position.X = 0f;
				ptr3.Position.Y = 0f;
				ptr3.Position.Z = -32f;
				ptr3.Orientation = Quaternion.Identity;
			}
			int originalNodeIndex4;
			bool flag6 = this.NodeIndicesByNameId.TryGetValue(leftThighNameId, out originalNodeIndex4);
			if (flag6)
			{
				BlockyModel.RecurseCloneNode(this, blockyModel, originalNodeIndex4, -1);
				int num6 = blockyModel.NodeIndicesByNameId[leftThighNameId];
				ref BlockyModelNode ptr4 = ref blockyModel.AllNodes[num6];
				ptr4.Position.X = 0f;
				ptr4.Position.Y = 0f;
				ptr4.Position.Z = -32f;
				ptr4.Orientation = Quaternion.Identity;
			}
			Array.Resize<BlockyModelNode>(ref blockyModel.AllNodes, blockyModel.NodeCount);
			Array.Resize<int>(ref blockyModel.ParentNodes, blockyModel.NodeCount);
			return blockyModel;
		}

		// Token: 0x060059F7 RID: 23031 RVA: 0x001BEA24 File Offset: 0x001BCC24
		private static void RecurseCloneNode(BlockyModel original, BlockyModel clone, int originalNodeIndex, int parentNodeIndex)
		{
			int nodeCount = clone.NodeCount;
			BlockyModelNode blockyModelNode = original.AllNodes[originalNodeIndex].Clone();
			clone.AddNode(ref blockyModelNode, parentNodeIndex);
			List<int> children = original.AllNodes[originalNodeIndex].Children;
			clone.EnsureNodeCountAllocated(children.Count, 0);
			foreach (int originalNodeIndex2 in children)
			{
				BlockyModel.RecurseCloneNode(original, clone, originalNodeIndex2, nodeCount);
			}
		}

		// Token: 0x060059F8 RID: 23032 RVA: 0x001BEAC0 File Offset: 0x001BCCC0
		public void Attach(BlockyModel attachment, NodeNameManager nodeNameManager, byte? atlasIndex = null, Point? uvOffset = null, int forcedTargetNodeNameId = -1)
		{
			this.EnsureNodeCountAllocated(attachment.NodeCount, 0);
			int num;
			bool flag = !this.NodeIndicesByNameId.TryGetValue(forcedTargetNodeNameId, out num);
			if (flag)
			{
				num = -1;
			}
			for (int i = 0; i < attachment.RootNodes.Count; i++)
			{
				int num2 = attachment.RootNodes[i];
				this.RecurseAttach(attachment, ref attachment.AllNodes[num2], num, nodeNameManager, atlasIndex, uvOffset, num != -1);
			}
		}

		// Token: 0x060059F9 RID: 23033 RVA: 0x001BEB40 File Offset: 0x001BCD40
		private void RecurseAttach(BlockyModel attachment, ref BlockyModelNode attachmentNode, int parentNodeIndex, NodeNameManager nodeNameManager, byte? atlasIndex, Point? uvOffset, bool forcedAttachment)
		{
			bool flag = !forcedAttachment && attachmentNode.IsPiece;
			if (flag)
			{
				int parentNodeIndex2;
				bool flag2 = this.NodeIndicesByNameId.TryGetValue(attachmentNode.NameId, out parentNodeIndex2);
				if (flag2)
				{
					for (int i = 0; i < attachmentNode.Children.Count; i++)
					{
						int num = attachmentNode.Children[i];
						this.RecurseAttach(attachment, ref attachment.AllNodes[num], parentNodeIndex2, nodeNameManager, atlasIndex, uvOffset, forcedAttachment);
					}
					return;
				}
				string argument;
				bool flag3 = nodeNameManager.TryGetNameFromId(attachmentNode.NameId, out argument);
				if (!flag3)
				{
					throw new Exception("Node name not found in manager");
				}
				BlockyModel.Logger.Warn("Couldn't find attachment target: {0}", argument);
			}
			int nodeCount = this.NodeCount;
			BlockyModelNode blockyModelNode = attachmentNode.Clone();
			bool flag4 = atlasIndex != null;
			if (flag4)
			{
				blockyModelNode.AtlasIndex = atlasIndex.Value;
			}
			blockyModelNode.GradientId = attachment.GradientId;
			bool flag5 = uvOffset != null && blockyModelNode.TextureLayout != null;
			if (flag5)
			{
				for (int j = 0; j < blockyModelNode.TextureLayout.Length; j++)
				{
					BlockyModelFaceTextureLayout[] textureLayout = blockyModelNode.TextureLayout;
					int num2 = j;
					textureLayout[num2].Offset.X = textureLayout[num2].Offset.X + uvOffset.Value.X;
					BlockyModelFaceTextureLayout[] textureLayout2 = blockyModelNode.TextureLayout;
					int num3 = j;
					textureLayout2[num3].Offset.Y = textureLayout2[num3].Offset.Y + uvOffset.Value.Y;
				}
			}
			this.AddNode(ref blockyModelNode, parentNodeIndex);
			for (int k = 0; k < attachmentNode.Children.Count; k++)
			{
				int num4 = attachmentNode.Children[k];
				this.RecurseAttach(attachment, ref attachment.AllNodes[num4], nodeCount, nodeNameManager, atlasIndex, uvOffset, forcedAttachment);
			}
		}

		// Token: 0x060059FA RID: 23034 RVA: 0x001BED24 File Offset: 0x001BCF24
		public void SetAtlasIndex(byte atlasIndex)
		{
			for (int i = 0; i < this.NodeCount; i++)
			{
				this.AllNodes[i].AtlasIndex = atlasIndex;
			}
		}

		// Token: 0x060059FB RID: 23035 RVA: 0x001BED5C File Offset: 0x001BCF5C
		public void SetGradientId(byte gradientId)
		{
			for (int i = 0; i < this.NodeCount; i++)
			{
				ref BlockyModelNode ptr = ref this.AllNodes[i];
				bool flag = ptr.TextureLayout != null;
				if (flag)
				{
					ptr.GradientId = gradientId;
				}
			}
		}

		// Token: 0x060059FC RID: 23036 RVA: 0x001BEDA4 File Offset: 0x001BCFA4
		public void OffsetUVs(Point offset)
		{
			for (int i = 0; i < this.NodeCount; i++)
			{
				ref BlockyModelNode ptr = ref this.AllNodes[i];
				bool flag = ptr.TextureLayout != null;
				if (flag)
				{
					for (int j = 0; j < ptr.TextureLayout.Length; j++)
					{
						BlockyModelFaceTextureLayout[] textureLayout = ptr.TextureLayout;
						int num = j;
						textureLayout[num].Offset.X = textureLayout[num].Offset.X + offset.X;
						BlockyModelFaceTextureLayout[] textureLayout2 = ptr.TextureLayout;
						int num2 = j;
						textureLayout2[num2].Offset.Y = textureLayout2[num2].Offset.Y + offset.Y;
					}
				}
			}
		}

		// Token: 0x060059FD RID: 23037 RVA: 0x001BEE48 File Offset: 0x001BD048
		private void EnsureNodeCountAllocated(int required, int growth = 0)
		{
			bool flag = this.AllNodes.Length < this.NodeCount + required;
			if (flag)
			{
				Array.Resize<BlockyModelNode>(ref this.AllNodes, this.NodeCount + required + growth);
				Array.Resize<int>(ref this.ParentNodes, this.NodeCount + required + growth);
			}
		}

		// Token: 0x04003809 RID: 14345
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400380A RID: 14346
		public const int EmptyNodeNameId = -1;

		// Token: 0x0400380B RID: 14347
		public const int NodeGrowthAmount = 5;

		// Token: 0x0400380C RID: 14348
		public static int MaxNodeCount = 256;

		// Token: 0x0400380D RID: 14349
		public readonly List<int> RootNodes = new List<int>();

		// Token: 0x0400380F RID: 14351
		public BlockyModelNode[] AllNodes = new BlockyModelNode[0];

		// Token: 0x04003810 RID: 14352
		public readonly Dictionary<int, int> NodeIndicesByNameId = new Dictionary<int, int>();

		// Token: 0x04003811 RID: 14353
		public int[] ParentNodes = new int[0];

		// Token: 0x04003812 RID: 14354
		public LodMode Lod = LodMode.Auto;

		// Token: 0x04003813 RID: 14355
		public byte GradientId;
	}
}
