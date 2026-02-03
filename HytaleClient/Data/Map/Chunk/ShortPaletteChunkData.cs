using System;
using System.Collections;
using System.Collections.Generic;

namespace HytaleClient.Data.Map.Chunk
{
	// Token: 0x02000AEC RID: 2796
	public class ShortPaletteChunkData : AbstractShortPaletteChunkData
	{
		// Token: 0x06005828 RID: 22568 RVA: 0x001ABF39 File Offset: 0x001AA139
		public ShortPaletteChunkData() : base(new short[32768])
		{
		}

		// Token: 0x06005829 RID: 22569 RVA: 0x001ABF4D File Offset: 0x001AA14D
		public ShortPaletteChunkData(Dictionary<int, short> externalToInternal, int[] internalToExternal, BitArray internalIdSet, Dictionary<short, ushort> internalIdCount, short[] blocks) : base(externalToInternal, internalToExternal, internalIdSet, internalIdCount, blocks)
		{
		}

		// Token: 0x0600582A RID: 22570 RVA: 0x001ABF60 File Offset: 0x001AA160
		internal override short Get0(int idx)
		{
			return this.blockData[idx];
		}

		// Token: 0x0600582B RID: 22571 RVA: 0x001ABF7A File Offset: 0x001AA17A
		internal override void Set0(int idx, short s)
		{
			this.blockData[idx] = s;
		}

		// Token: 0x0600582C RID: 22572 RVA: 0x001ABF88 File Offset: 0x001AA188
		public override bool ShouldDemote()
		{
			return this.BlockCount() <= 254;
		}

		// Token: 0x0600582D RID: 22573 RVA: 0x001ABFAC File Offset: 0x001AA1AC
		public override IChunkData Demote()
		{
			return BytePaletteChunkData.fromShortPalette(this);
		}

		// Token: 0x0600582E RID: 22574 RVA: 0x001ABFC4 File Offset: 0x001AA1C4
		public override IChunkData Promote()
		{
			throw new InvalidOperationException("Short palette cannot be promoted.");
		}

		// Token: 0x0600582F RID: 22575 RVA: 0x001ABFD4 File Offset: 0x001AA1D4
		protected override bool IsValidInternalId(int internalId)
		{
			return (internalId & 65535) == internalId;
		}

		// Token: 0x06005830 RID: 22576 RVA: 0x001ABFF0 File Offset: 0x001AA1F0
		protected override int UnsignedInternalId(short internalId)
		{
			return (int)internalId & 65535;
		}

		// Token: 0x06005831 RID: 22577 RVA: 0x001AC00C File Offset: 0x001AA20C
		public static ShortPaletteChunkData fromBytePalette(BytePaletteChunkData section)
		{
			Dictionary<int, short> dictionary = new Dictionary<int, short>();
			int[] array = new int[section.internalToExternal.Length * 2];
			foreach (KeyValuePair<int, byte> keyValuePair in section.externalToInternal)
			{
				int key = keyValuePair.Key;
				byte value = keyValuePair.Value;
				dictionary[key] = (short)value;
				array[(int)value] = key;
			}
			BitArray bitArray = new BitArray(32768);
			for (int i = 0; i < 256; i++)
			{
				bitArray[i] = section.internalIdSet[i];
			}
			Dictionary<short, ushort> dictionary2 = new Dictionary<short, ushort>();
			foreach (KeyValuePair<byte, ushort> keyValuePair2 in section.internalIdCount)
			{
				byte key2 = keyValuePair2.Key;
				ushort value2 = keyValuePair2.Value;
				dictionary2[(short)key2] = value2;
			}
			short[] array2 = new short[32768];
			for (int j = 0; j < 32768; j++)
			{
				array2[j] = (short)section.blockData[j];
			}
			return new ShortPaletteChunkData(dictionary, array, bitArray, dictionary2, array2);
		}

		// Token: 0x0400366B RID: 13931
		private const int KEY_MASK = 65535;

		// Token: 0x0400366C RID: 13932
		private const int MAX_SIZE = 65536;

		// Token: 0x0400366D RID: 13933
		private const int DEMOTE_SIZE = 254;
	}
}
