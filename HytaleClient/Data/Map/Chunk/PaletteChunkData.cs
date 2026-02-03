using System;
using System.Collections.Generic;

namespace HytaleClient.Data.Map.Chunk
{
	// Token: 0x02000AEB RID: 2795
	public class PaletteChunkData : IChunkDataBase
	{
		// Token: 0x0600581C RID: 22556 RVA: 0x001ABD81 File Offset: 0x001A9F81
		public PaletteChunkData()
		{
			this.chunkSection = EmptyPaletteChunkData.Instance;
		}

		// Token: 0x0600581D RID: 22557 RVA: 0x001ABD96 File Offset: 0x001A9F96
		public PaletteChunkData(IChunkData chunkSection)
		{
			this.chunkSection = chunkSection;
		}

		// Token: 0x0600581E RID: 22558 RVA: 0x001ABDA8 File Offset: 0x001A9FA8
		public IChunkData GetChunkSection()
		{
			return this.chunkSection;
		}

		// Token: 0x0600581F RID: 22559 RVA: 0x001ABDC0 File Offset: 0x001A9FC0
		public void SetChunkSection(IChunkData chunkSection)
		{
			this.chunkSection = chunkSection;
		}

		// Token: 0x06005820 RID: 22560 RVA: 0x001ABDCC File Offset: 0x001A9FCC
		public override int Get(int blockIdx)
		{
			return this.chunkSection.Get(blockIdx);
		}

		// Token: 0x06005821 RID: 22561 RVA: 0x001ABDEC File Offset: 0x001A9FEC
		public void Set(int blockIdx, int blockId)
		{
			BlockSetResult blockSetResult = this.chunkSection.Set(blockIdx, blockId);
			bool flag = blockSetResult == BlockSetResult.REQUIRES_PROMOTE;
			if (flag)
			{
				this.chunkSection = this.chunkSection.Promote();
				BlockSetResult blockSetResult2 = this.chunkSection.Set(blockIdx, blockId);
				bool flag2 = blockSetResult2 > BlockSetResult.BLOCK_ADDED_OR_REMOVED;
				if (flag2)
				{
					throw new InvalidOperationException("Promoted chunk section failed to correctly add the new block!");
				}
			}
			else
			{
				bool flag3 = blockSetResult == BlockSetResult.BLOCK_ADDED_OR_REMOVED;
				if (flag3)
				{
				}
				bool flag4 = this.chunkSection.ShouldDemote();
				if (flag4)
				{
					this.chunkSection = this.chunkSection.Demote();
				}
			}
		}

		// Token: 0x06005822 RID: 22562 RVA: 0x001ABE7C File Offset: 0x001AA07C
		public override bool Contains(int blockId)
		{
			return this.chunkSection.Contains(blockId);
		}

		// Token: 0x06005823 RID: 22563 RVA: 0x001ABE9C File Offset: 0x001AA09C
		public override int BlockCount()
		{
			return this.chunkSection.BlockCount();
		}

		// Token: 0x06005824 RID: 22564 RVA: 0x001ABEBC File Offset: 0x001AA0BC
		public override int Count(int blockId)
		{
			return this.chunkSection.Count(blockId);
		}

		// Token: 0x06005825 RID: 22565 RVA: 0x001ABEDC File Offset: 0x001AA0DC
		public override HashSet<int> Blocks()
		{
			return this.chunkSection.Blocks();
		}

		// Token: 0x06005826 RID: 22566 RVA: 0x001ABEFC File Offset: 0x001AA0FC
		public override Dictionary<int, ushort> BlockCounts()
		{
			return this.chunkSection.BlockCounts();
		}

		// Token: 0x06005827 RID: 22567 RVA: 0x001ABF1C File Offset: 0x001AA11C
		public override bool IsSolidAir()
		{
			return this.chunkSection.IsSolidAir();
		}

		// Token: 0x0400366A RID: 13930
		private IChunkData chunkSection;
	}
}
