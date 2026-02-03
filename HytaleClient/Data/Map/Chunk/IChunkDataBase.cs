using System;
using System.Collections.Generic;
using HytaleClient.Utils;

namespace HytaleClient.Data.Map.Chunk
{
	// Token: 0x02000AEA RID: 2794
	public abstract class IChunkDataBase
	{
		// Token: 0x06005813 RID: 22547 RVA: 0x001ABD58 File Offset: 0x001A9F58
		public virtual int Get(int x, int y, int z)
		{
			return this.Get(ChunkHelper.IndexOfBlockInChunk(x, y, z));
		}

		// Token: 0x06005814 RID: 22548
		public abstract int Get(int blockIdx);

		// Token: 0x06005815 RID: 22549
		public abstract bool Contains(int blockId);

		// Token: 0x06005816 RID: 22550
		public abstract int BlockCount();

		// Token: 0x06005817 RID: 22551
		public abstract int Count(int blockId);

		// Token: 0x06005818 RID: 22552
		public abstract HashSet<int> Blocks();

		// Token: 0x06005819 RID: 22553
		public abstract Dictionary<int, ushort> BlockCounts();

		// Token: 0x0600581A RID: 22554
		public abstract bool IsSolidAir();
	}
}
