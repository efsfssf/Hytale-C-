using System;
using System.IO;
using HytaleClient.Protocol;
using HytaleClient.Utils;

namespace HytaleClient.Data.Map.Chunk
{
	// Token: 0x02000AE9 RID: 2793
	public abstract class IChunkData : IChunkDataBase
	{
		// Token: 0x0600580B RID: 22539 RVA: 0x001ABD04 File Offset: 0x001A9F04
		public virtual BlockSetResult Set(int x, int y, int z, int blockId)
		{
			return this.Set(ChunkHelper.IndexOfBlockInChunk(x, y, z), blockId);
		}

		// Token: 0x0600580C RID: 22540
		public abstract BlockSetResult Set(int blockIdx, int blockId);

		// Token: 0x0600580D RID: 22541 RVA: 0x001ABD28 File Offset: 0x001A9F28
		public override bool IsSolidAir()
		{
			return this.BlockCount() == 1 && this.Contains(0);
		}

		// Token: 0x0600580E RID: 22542
		public abstract bool ShouldDemote();

		// Token: 0x0600580F RID: 22543
		public abstract IChunkData Demote();

		// Token: 0x06005810 RID: 22544
		public abstract IChunkData Promote();

		// Token: 0x06005811 RID: 22545
		public abstract void Deserialize(BinaryReader reader, int maxValidBlockTypeId, PaletteType paletteType);
	}
}
