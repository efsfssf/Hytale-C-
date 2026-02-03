using System;
using System.Collections;
using System.Collections.Generic;

namespace HytaleClient.Data.Map.Chunk
{
	// Token: 0x02000AE8 RID: 2792
	public class HalfBytePaletteChunkData : AbstractBytePaletteChunkData
	{
		// Token: 0x06005800 RID: 22528 RVA: 0x001AB9DA File Offset: 0x001A9BDA
		public HalfBytePaletteChunkData() : base(new byte[16384])
		{
		}

		// Token: 0x06005801 RID: 22529 RVA: 0x001AB9EE File Offset: 0x001A9BEE
		public HalfBytePaletteChunkData(Dictionary<int, byte> externalToInternal, int[] internalToExternal, BitArray internalIdSet, Dictionary<byte, ushort> internalIdCount, byte[] blocks) : base(externalToInternal, internalToExternal, internalIdSet, internalIdCount, blocks)
		{
		}

		// Token: 0x06005802 RID: 22530 RVA: 0x001ABA00 File Offset: 0x001A9C00
		internal override void Set0(int idx, byte b)
		{
			int num = idx >> 1;
			byte b2 = this.blockData[num];
			b &= 15;
			int num2 = idx & 1;
			b = (byte)(b << ((num2 ^ 1) << 2));
			b2 = (byte)((int)b2 & 15 << (num2 << 2));
			b2 |= b;
			this.blockData[num] = b2;
		}

		// Token: 0x06005803 RID: 22531 RVA: 0x001ABA50 File Offset: 0x001A9C50
		internal override byte Get0(int idx)
		{
			int num = idx >> 1;
			byte b = this.blockData[num];
			int num2 = idx & 1;
			b = (byte)(b >> ((num2 ^ 1) << 2));
			return b & 15;
		}

		// Token: 0x06005804 RID: 22532 RVA: 0x001ABA88 File Offset: 0x001A9C88
		public override bool ShouldDemote()
		{
			return this.IsSolidAir();
		}

		// Token: 0x06005805 RID: 22533 RVA: 0x001ABAA0 File Offset: 0x001A9CA0
		public override IChunkData Demote()
		{
			return EmptyPaletteChunkData.Instance;
		}

		// Token: 0x06005806 RID: 22534 RVA: 0x001ABAB8 File Offset: 0x001A9CB8
		public override IChunkData Promote()
		{
			return BytePaletteChunkData.fromHalfBytePalette(this);
		}

		// Token: 0x06005807 RID: 22535 RVA: 0x001ABAD0 File Offset: 0x001A9CD0
		protected override bool IsValidInternalId(int internalId)
		{
			return (internalId & 15) == internalId;
		}

		// Token: 0x06005808 RID: 22536 RVA: 0x001ABAEC File Offset: 0x001A9CEC
		protected override int UnsignedInternalId(byte internalId)
		{
			return (int)(internalId & 15);
		}

		// Token: 0x06005809 RID: 22537 RVA: 0x001ABB04 File Offset: 0x001A9D04
		private static int sUnsignedInternalId(byte internalId)
		{
			return (int)(internalId & 15);
		}

		// Token: 0x0600580A RID: 22538 RVA: 0x001ABB1C File Offset: 0x001A9D1C
		public static HalfBytePaletteChunkData fromBytePalette(BytePaletteChunkData section)
		{
			bool flag = section.BlockCount() > 16;
			if (flag)
			{
				throw new InvalidOperationException("Cannot demote byte palette to half byte palette. Too many blocks! Count: " + section.BlockCount().ToString());
			}
			HalfBytePaletteChunkData halfBytePaletteChunkData = new HalfBytePaletteChunkData();
			Dictionary<byte, byte> dictionary = new Dictionary<byte, byte>();
			halfBytePaletteChunkData.internalToExternal = new int[16];
			halfBytePaletteChunkData.internalIdSet.SetAll(false);
			foreach (KeyValuePair<int, byte> keyValuePair in section.externalToInternal)
			{
				int key = keyValuePair.Key;
				byte value = keyValuePair.Value;
				byte b = 0;
				while ((int)b < halfBytePaletteChunkData.internalIdSet.Count && halfBytePaletteChunkData.internalIdSet[(int)b])
				{
					b += 1;
				}
				halfBytePaletteChunkData.internalIdSet[HalfBytePaletteChunkData.sUnsignedInternalId(b)] = true;
				dictionary[value] = b;
				halfBytePaletteChunkData.internalToExternal[(int)b] = key;
				halfBytePaletteChunkData.externalToInternal[key] = b;
			}
			halfBytePaletteChunkData.internalIdCount.Clear();
			foreach (KeyValuePair<byte, ushort> keyValuePair2 in section.internalIdCount)
			{
				byte key2 = keyValuePair2.Key;
				ushort value2 = keyValuePair2.Value;
				byte key3 = dictionary[key2];
				halfBytePaletteChunkData.internalIdCount[key3] = value2;
			}
			for (int i = 0; i < section.blockData.Length; i++)
			{
				byte key4 = section.blockData[i];
				byte b2 = dictionary[key4];
				halfBytePaletteChunkData.Set0(i, b2);
			}
			return halfBytePaletteChunkData;
		}

		// Token: 0x04003668 RID: 13928
		private const int KEY_MASK = 15;

		// Token: 0x04003669 RID: 13929
		public const int MAX_SIZE = 16;
	}
}
