using System;
using System.Collections;
using System.Collections.Generic;

namespace HytaleClient.Data.Map.Chunk
{
	// Token: 0x02000AE6 RID: 2790
	public class BytePaletteChunkData : AbstractBytePaletteChunkData
	{
		// Token: 0x060057E4 RID: 22500 RVA: 0x001AB441 File Offset: 0x001A9641
		public BytePaletteChunkData() : base(new byte[32768])
		{
		}

		// Token: 0x060057E5 RID: 22501 RVA: 0x001AB455 File Offset: 0x001A9655
		public BytePaletteChunkData(Dictionary<int, byte> externalToInternal, int[] internalToExternal, BitArray internalIdSet, Dictionary<byte, ushort> internalIdCount, byte[] blocks) : base(externalToInternal, internalToExternal, internalIdSet, internalIdCount, blocks)
		{
		}

		// Token: 0x060057E6 RID: 22502 RVA: 0x001AB468 File Offset: 0x001A9668
		internal override byte Get0(int idx)
		{
			return this.blockData[idx];
		}

		// Token: 0x060057E7 RID: 22503 RVA: 0x001AB482 File Offset: 0x001A9682
		internal override void Set0(int idx, byte b)
		{
			this.blockData[idx] = b;
		}

		// Token: 0x060057E8 RID: 22504 RVA: 0x001AB490 File Offset: 0x001A9690
		public override bool ShouldDemote()
		{
			return this.BlockCount() <= 14;
		}

		// Token: 0x060057E9 RID: 22505 RVA: 0x001AB4B0 File Offset: 0x001A96B0
		public override IChunkData Demote()
		{
			return HalfBytePaletteChunkData.fromBytePalette(this);
		}

		// Token: 0x060057EA RID: 22506 RVA: 0x001AB4C8 File Offset: 0x001A96C8
		public override IChunkData Promote()
		{
			return ShortPaletteChunkData.fromBytePalette(this);
		}

		// Token: 0x060057EB RID: 22507 RVA: 0x001AB4E0 File Offset: 0x001A96E0
		protected override bool IsValidInternalId(int internalId)
		{
			return (internalId & 255) == internalId;
		}

		// Token: 0x060057EC RID: 22508 RVA: 0x001AB4FC File Offset: 0x001A96FC
		protected override int UnsignedInternalId(byte internalId)
		{
			return (int)(internalId & byte.MaxValue);
		}

		// Token: 0x060057ED RID: 22509 RVA: 0x001AB518 File Offset: 0x001A9718
		private static int sUnsignedInternalId(byte internalId)
		{
			return (int)(internalId & byte.MaxValue);
		}

		// Token: 0x060057EE RID: 22510 RVA: 0x001AB534 File Offset: 0x001A9734
		public static BytePaletteChunkData fromHalfBytePalette(HalfBytePaletteChunkData section)
		{
			BytePaletteChunkData bytePaletteChunkData = new BytePaletteChunkData();
			bytePaletteChunkData.externalToInternal.Clear();
			bytePaletteChunkData.internalToExternal = new int[section.internalToExternal.Length * 2];
			foreach (KeyValuePair<int, byte> keyValuePair in section.externalToInternal)
			{
				int key = keyValuePair.Key;
				byte value = keyValuePair.Value;
				bytePaletteChunkData.externalToInternal[key] = value;
				bytePaletteChunkData.internalToExternal[(int)value] = key;
			}
			bytePaletteChunkData.internalIdSet.SetAll(false);
			bytePaletteChunkData.internalIdSet.Or(section.internalIdSet);
			bytePaletteChunkData.internalIdCount.Clear();
			foreach (KeyValuePair<byte, ushort> keyValuePair2 in section.internalIdCount)
			{
				bytePaletteChunkData.internalIdCount[keyValuePair2.Key] = keyValuePair2.Value;
			}
			for (int i = 0; i < bytePaletteChunkData.blockData.Length; i++)
			{
				bytePaletteChunkData.blockData[i] = section.Get0(i);
			}
			return bytePaletteChunkData;
		}

		// Token: 0x060057EF RID: 22511 RVA: 0x001AB694 File Offset: 0x001A9894
		public static BytePaletteChunkData fromShortPalette(ShortPaletteChunkData section)
		{
			bool flag = section.BlockCount() > 256;
			if (flag)
			{
				throw new InvalidOperationException("Cannot demote short palette to byte palette. Too many blocks! Count: " + section.BlockCount().ToString());
			}
			BytePaletteChunkData bytePaletteChunkData = new BytePaletteChunkData();
			Dictionary<short, byte> dictionary = new Dictionary<short, byte>();
			bytePaletteChunkData.internalToExternal = new int[256];
			bytePaletteChunkData.internalIdSet.SetAll(false);
			foreach (KeyValuePair<int, short> keyValuePair in section.externalToInternal)
			{
				int key = keyValuePair.Key;
				short value = keyValuePair.Value;
				byte b = 0;
				while ((int)b < bytePaletteChunkData.internalIdSet.Count && bytePaletteChunkData.internalIdSet[(int)b])
				{
					b += 1;
				}
				bytePaletteChunkData.internalIdSet[(int)b] = true;
				dictionary[value] = b;
				bytePaletteChunkData.internalToExternal[(int)b] = key;
				bytePaletteChunkData.externalToInternal[key] = b;
			}
			bytePaletteChunkData.internalIdCount.Clear();
			foreach (KeyValuePair<short, ushort> keyValuePair2 in section.internalIdCount)
			{
				short key2 = keyValuePair2.Key;
				ushort value2 = keyValuePair2.Value;
				byte key3 = dictionary[key2];
				bytePaletteChunkData.internalIdCount[key3] = value2;
			}
			for (int i = 0; i < 32768; i++)
			{
				short key4 = section.blockData[i];
				byte b2 = dictionary[key4];
				bytePaletteChunkData.blockData[i] = b2;
			}
			return bytePaletteChunkData;
		}

		// Token: 0x04003664 RID: 13924
		private const int KEY_MASK = 255;

		// Token: 0x04003665 RID: 13925
		public const int MAX_SIZE = 256;

		// Token: 0x04003666 RID: 13926
		public const int DEMOTE_SIZE = 14;
	}
}
