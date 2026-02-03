using System;
using HytaleClient.Data.Map;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x0200094E RID: 2382
	internal class BlockData
	{
		// Token: 0x170011F6 RID: 4598
		// (get) Token: 0x06004A83 RID: 19075 RVA: 0x00130195 File Offset: 0x0012E395
		public bool IsTrigger
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170011F7 RID: 4599
		// (get) Token: 0x06004A84 RID: 19076 RVA: 0x00130198 File Offset: 0x0012E398
		public int BlockDamage
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06004A85 RID: 19077 RVA: 0x0013019C File Offset: 0x0012E39C
		public void Assign(BlockData other)
		{
			this._blockId = other._blockId;
			this.BlockType = other.BlockType;
			this._originalBlockTypeId = other._originalBlockTypeId;
			this.OriginalBlockType = other.OriginalBlockType;
			this._submergeFluid = other._submergeFluid;
			this._submergeFluidId = other._submergeFluidId;
			this.FillHeight = other.FillHeight;
			this.CollisionMaterials = other.CollisionMaterials;
			this.IsFiller = other.IsFiller;
			this._blockBoundingBoxes = other._blockBoundingBoxes;
		}

		// Token: 0x06004A86 RID: 19078 RVA: 0x00130222 File Offset: 0x0012E422
		public void Clear()
		{
			this._blockId = int.MaxValue;
			this.BlockType = null;
			this._originalBlockTypeId = int.MaxValue;
			this.OriginalBlockType = null;
			this._submergeFluid = null;
			this._submergeFluidId = int.MaxValue;
			this._blockBoundingBoxes = null;
		}

		// Token: 0x06004A87 RID: 19079 RVA: 0x00130264 File Offset: 0x0012E464
		public ClientBlockType GetSubmergeFluid(GameInstance gameInstance)
		{
			bool flag = this.FillHeight > 0f && this._submergeFluid == null && this._submergeFluidId == int.MaxValue;
			if (flag)
			{
				this._submergeFluidId = this.BlockType.FluidBlockId;
				bool flag2 = this._submergeFluidId == 0;
				if (flag2)
				{
					throw new Exception("Have fluid key but fill level is 0");
				}
				this._submergeFluid = gameInstance.MapModule.ClientBlockTypes[this._submergeFluidId];
			}
			return this._submergeFluid;
		}

		// Token: 0x06004A88 RID: 19080 RVA: 0x001302E8 File Offset: 0x0012E4E8
		public BlockHitbox GetBlockBoundingBoxes(GameInstance gameInstance)
		{
			bool flag = this._blockBoundingBoxes == null;
			if (flag)
			{
				this._blockBoundingBoxes = gameInstance.ServerSettings.BlockHitboxes[this.OriginalBlockType.HitboxType];
			}
			return this._blockBoundingBoxes;
		}

		// Token: 0x06004A89 RID: 19081 RVA: 0x0013032C File Offset: 0x0012E52C
		public int OriginX(int x)
		{
			return x - this.BlockType.FillerX;
		}

		// Token: 0x06004A8A RID: 19082 RVA: 0x0013034C File Offset: 0x0012E54C
		public int OriginY(int y)
		{
			return y - this.BlockType.FillerY;
		}

		// Token: 0x06004A8B RID: 19083 RVA: 0x0013036C File Offset: 0x0012E56C
		public int OriginZ(int z)
		{
			return z - this.BlockType.FillerZ;
		}

		// Token: 0x04002642 RID: 9794
		protected int _blockId = int.MaxValue;

		// Token: 0x04002643 RID: 9795
		public ClientBlockType BlockType;

		// Token: 0x04002644 RID: 9796
		public ClientBlockType OriginalBlockType;

		// Token: 0x04002645 RID: 9797
		protected int _originalBlockTypeId = int.MaxValue;

		// Token: 0x04002646 RID: 9798
		protected ClientBlockType _submergeFluid;

		// Token: 0x04002647 RID: 9799
		protected int _submergeFluidId = int.MaxValue;

		// Token: 0x04002648 RID: 9800
		public float FillHeight;

		// Token: 0x04002649 RID: 9801
		public int CollisionMaterials;

		// Token: 0x0400264A RID: 9802
		public bool IsFiller;

		// Token: 0x0400264B RID: 9803
		protected BlockHitbox _blockBoundingBoxes;
	}
}
