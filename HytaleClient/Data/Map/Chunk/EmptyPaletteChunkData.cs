using System;
using System.Collections.Generic;
using System.IO;
using HytaleClient.Protocol;

namespace HytaleClient.Data.Map.Chunk
{
	// Token: 0x02000AE7 RID: 2791
	public class EmptyPaletteChunkData : IChunkData
	{
		// Token: 0x060057F0 RID: 22512 RVA: 0x001AB87C File Offset: 0x001A9A7C
		private EmptyPaletteChunkData()
		{
		}

		// Token: 0x060057F1 RID: 22513 RVA: 0x001AB888 File Offset: 0x001A9A88
		public override BlockSetResult Set(int x, int y, int z, int blockId)
		{
			return (blockId == 0) ? BlockSetResult.BLOCK_UNCHANGED : BlockSetResult.REQUIRES_PROMOTE;
		}

		// Token: 0x060057F2 RID: 22514 RVA: 0x001AB8A4 File Offset: 0x001A9AA4
		public override BlockSetResult Set(int blockIdx, int blockId)
		{
			return (blockId == 0) ? BlockSetResult.BLOCK_UNCHANGED : BlockSetResult.REQUIRES_PROMOTE;
		}

		// Token: 0x060057F3 RID: 22515 RVA: 0x001AB8C0 File Offset: 0x001A9AC0
		public override int Get(int x, int y, int z)
		{
			return 0;
		}

		// Token: 0x060057F4 RID: 22516 RVA: 0x001AB8D4 File Offset: 0x001A9AD4
		public override int Get(int blockIdx)
		{
			return 0;
		}

		// Token: 0x060057F5 RID: 22517 RVA: 0x001AB8E8 File Offset: 0x001A9AE8
		public override bool ShouldDemote()
		{
			return false;
		}

		// Token: 0x060057F6 RID: 22518 RVA: 0x001AB8FB File Offset: 0x001A9AFB
		public override IChunkData Demote()
		{
			throw new InvalidOperationException("Cannot demote empty chunk section!");
		}

		// Token: 0x060057F7 RID: 22519 RVA: 0x001AB908 File Offset: 0x001A9B08
		public override IChunkData Promote()
		{
			return new HalfBytePaletteChunkData();
		}

		// Token: 0x060057F8 RID: 22520 RVA: 0x001AB920 File Offset: 0x001A9B20
		public override bool Contains(int blockId)
		{
			return blockId == 0;
		}

		// Token: 0x060057F9 RID: 22521 RVA: 0x001AB938 File Offset: 0x001A9B38
		public override int BlockCount()
		{
			return 1;
		}

		// Token: 0x060057FA RID: 22522 RVA: 0x001AB94C File Offset: 0x001A9B4C
		public override int Count(int blockId)
		{
			return (blockId == 0) ? 32768 : 0;
		}

		// Token: 0x060057FB RID: 22523 RVA: 0x001AB96C File Offset: 0x001A9B6C
		public override HashSet<int> Blocks()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(0);
			return hashSet;
		}

		// Token: 0x060057FC RID: 22524 RVA: 0x001AB990 File Offset: 0x001A9B90
		public override Dictionary<int, ushort> BlockCounts()
		{
			Dictionary<int, ushort> dictionary = new Dictionary<int, ushort>();
			dictionary[0] = 32768;
			return dictionary;
		}

		// Token: 0x060057FD RID: 22525 RVA: 0x001AB9B8 File Offset: 0x001A9BB8
		public override bool IsSolidAir()
		{
			return true;
		}

		// Token: 0x060057FE RID: 22526 RVA: 0x001AB9CB File Offset: 0x001A9BCB
		public override void Deserialize(BinaryReader reader, int maxValidBlockTypeId, PaletteType paletteType)
		{
		}

		// Token: 0x04003667 RID: 13927
		public static readonly EmptyPaletteChunkData Instance = new EmptyPaletteChunkData();
	}
}
