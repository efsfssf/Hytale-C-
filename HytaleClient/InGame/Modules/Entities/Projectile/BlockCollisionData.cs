using System;
using HytaleClient.Data.Map;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x0200094B RID: 2379
	internal class BlockCollisionData : BoxCollisionData
	{
		// Token: 0x06004A67 RID: 19047 RVA: 0x0012F154 File Offset: 0x0012D354
		public void SetBlockData(CollisionConfig collisionConfig)
		{
			this.X = collisionConfig.BlockX;
			this.Y = collisionConfig.BlockY;
			this.Z = collisionConfig.BlockZ;
			this.BlockId = collisionConfig.BlockId;
			this.BlockType = collisionConfig.BlockType;
			this.BlockMaterial = new BlockType.Material?(collisionConfig.BlockMaterial);
		}

		// Token: 0x06004A68 RID: 19048 RVA: 0x0012F1AF File Offset: 0x0012D3AF
		public void SetDetailBoxIndex(int detailBoxIndex)
		{
			this.DetailBoxIndex = detailBoxIndex;
		}

		// Token: 0x06004A69 RID: 19049 RVA: 0x0012F1B9 File Offset: 0x0012D3B9
		public void SetTouchingOverlapping(bool touching, bool overlapping)
		{
			this.Touching = touching;
			this.Overlapping = overlapping;
		}

		// Token: 0x06004A6A RID: 19050 RVA: 0x0012F1CA File Offset: 0x0012D3CA
		public void clear()
		{
			this.BlockType = null;
			this.BlockMaterial = null;
		}

		// Token: 0x04002622 RID: 9762
		public int X;

		// Token: 0x04002623 RID: 9763
		public int Y;

		// Token: 0x04002624 RID: 9764
		public int Z;

		// Token: 0x04002625 RID: 9765
		public int BlockId;

		// Token: 0x04002626 RID: 9766
		public ClientBlockType BlockType;

		// Token: 0x04002627 RID: 9767
		public BlockType.Material? BlockMaterial;

		// Token: 0x04002628 RID: 9768
		public int DetailBoxIndex;

		// Token: 0x04002629 RID: 9769
		public bool Touching;

		// Token: 0x0400262A RID: 9770
		public bool Overlapping;
	}
}
