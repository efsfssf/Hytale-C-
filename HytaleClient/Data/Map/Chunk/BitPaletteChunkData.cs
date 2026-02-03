using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using HytaleClient.Protocol;

namespace HytaleClient.Data.Map.Chunk
{
	// Token: 0x02000AE4 RID: 2788
	public class BitPaletteChunkData : IChunkData
	{
		// Token: 0x060057CA RID: 22474 RVA: 0x001AA958 File Offset: 0x001A8B58
		public BitPaletteChunkData(PaletteType paletteType)
		{
			Debug.Assert(paletteType > 0, "Attempted to instantiate an empty BitPalette. Use EmptyPaletteChunkData.Instance instead");
			this._promotionIndex = BitPaletteChunkData.PaletteTypeToPromotionIndex(paletteType);
			this.Resize((int)BitPaletteChunkData.promotionTable[paletteType], false);
			this._externalToInternal[0] = 0;
			this._internalToExternal[0] = 0;
			this._typeCounts[0] = 32768;
		}

		// Token: 0x060057CB RID: 22475 RVA: 0x001AA9DA File Offset: 0x001A8BDA
		public BitPaletteChunkData(BinaryReader reader, int maxValidBlockTypeId, PaletteType paletteType)
		{
			this.Deserialize(reader, maxValidBlockTypeId, paletteType);
		}

		// Token: 0x060057CC RID: 22476 RVA: 0x001AAA08 File Offset: 0x001A8C08
		private static int PaletteTypeToPromotionIndex(PaletteType type)
		{
			int result;
			switch (type)
			{
			case 1:
				result = 1;
				break;
			case 2:
				result = 5;
				break;
			case 3:
				result = 13;
				break;
			default:
				result = 1;
				break;
			}
			return result;
		}

		// Token: 0x060057CD RID: 22477 RVA: 0x001AAA44 File Offset: 0x001A8C44
		private void Resize(int bitsPerBlock, bool retainData = true)
		{
			Debug.Assert(bitsPerBlock > 0 && bitsPerBlock <= 16);
			int num = 1 << bitsPerBlock;
			this._bitMask = (uint)(num - 1);
			uint[] array = new uint[32768 * bitsPerBlock / 32];
			int[] array2 = new int[num];
			if (retainData)
			{
				bool flag = array2.Length > this._internalToExternal.Length;
				if (flag)
				{
					Buffer.BlockCopy(this._internalToExternal, 0, array2, 0, this._internalToExternal.Length * 4);
					for (int i = 0; i < 32768; i++)
					{
						int internalId = (int)BitPaletteChunkData.GetInternalId(i, this._bitsPerBlock, this._data);
						BitPaletteChunkData.SetInternalId(i, internalId, bitsPerBlock, array);
					}
				}
				else
				{
					Dictionary<int, short> dictionary = new Dictionary<int, short>(16);
					Dictionary<short, ushort> dictionary2 = new Dictionary<short, ushort>(16);
					Dictionary<short, short> dictionary3 = new Dictionary<short, short>();
					short num2 = 0;
					foreach (KeyValuePair<int, short> keyValuePair in this._externalToInternal)
					{
						int key = keyValuePair.Key;
						short value = keyValuePair.Value;
						dictionary[key] = num2;
						dictionary2[num2] = this._typeCounts[value];
						array2[(int)num2] = this._internalToExternal[(int)value];
						dictionary3[value] = num2;
						num2 += 1;
					}
					this._externalToInternal = dictionary;
					this._typeCounts = dictionary2;
					for (int j = 0; j < 32768; j++)
					{
						num2 = BitPaletteChunkData.GetInternalId(j, this._bitsPerBlock, this._data);
						BitPaletteChunkData.SetInternalId(j, (int)dictionary3[num2], bitsPerBlock, array);
					}
				}
			}
			else
			{
				this._externalToInternal.Clear();
				this._typeCounts.Clear();
			}
			this._data = array;
			this._internalToExternal = array2;
			int num3 = this._promotionIndex - 1;
			bool flag2 = num3 <= 0;
			if (flag2)
			{
				this._demoteSize = 0;
			}
			else
			{
				this._demoteSize = (1 << (int)BitPaletteChunkData.promotionTable[num3]) - 2;
			}
			this._bitsPerBlock = bitsPerBlock;
		}

		// Token: 0x060057CE RID: 22478 RVA: 0x001AAC80 File Offset: 0x001A8E80
		private static void CopyData(uint[] src, int srcBits, uint[] dest, int destBits)
		{
		}

		// Token: 0x060057CF RID: 22479 RVA: 0x001AAC84 File Offset: 0x001A8E84
		public override int Get(int blockIdx)
		{
			int internalId = (int)this.GetInternalId(blockIdx);
			return this._internalToExternal[internalId];
		}

		// Token: 0x060057D0 RID: 22480 RVA: 0x001AACA8 File Offset: 0x001A8EA8
		private short GetInternalId(int blockIdx)
		{
			int num = blockIdx * this._bitsPerBlock;
			int num2 = num / 32;
			uint num3 = this._data[num2];
			int num4 = num % 32;
			int num5 = (num + this._bitsPerBlock - 1) / 32;
			bool flag = num2 == num5;
			short result;
			if (flag)
			{
				result = (short)(num3 >> num4 & this._bitMask);
			}
			else
			{
				int num6 = 32 - num4;
				result = (short)((num3 >> num4 | this._data[num5] << num6) & this._bitMask);
			}
			return result;
		}

		// Token: 0x060057D1 RID: 22481 RVA: 0x001AAD2C File Offset: 0x001A8F2C
		private static short GetInternalId(int blockIdx, int bitsPerBlock, uint[] src)
		{
			uint num = (1U << bitsPerBlock) - 1U;
			int num2 = blockIdx * bitsPerBlock;
			int num3 = num2 / 32;
			uint num4 = src[num3];
			int num5 = num2 % 32;
			int num6 = (num2 + bitsPerBlock - 1) / 32;
			bool flag = num3 == num6;
			short result;
			if (flag)
			{
				result = (short)(num4 >> num5 & num);
			}
			else
			{
				int num7 = 32 - num5;
				result = (short)((num4 >> num5 | src[num6] << num7) & num);
			}
			return result;
		}

		// Token: 0x060057D2 RID: 22482 RVA: 0x001AAD9C File Offset: 0x001A8F9C
		private void SetInternalId(int blockIdx, int blockId)
		{
			int num = blockIdx * this._bitsPerBlock;
			int num2 = num / 32;
			uint num3 = this._data[num2];
			int num4 = num % 32;
			uint num5 = this._bitMask << num4;
			num3 &= ~num5;
			uint num6 = (uint)((uint)blockId << num4);
			num3 |= num6;
			this._data[num2] = num3;
			int num7 = (num + this._bitsPerBlock - 1) / 32;
			bool flag = num2 != num7;
			if (flag)
			{
				int num8 = 32 - num4;
				int num9 = this._bitsPerBlock - num8;
				this._data[num7] = (this._data[num7] >> num9 << num9 | (uint)(blockId >> num8));
			}
		}

		// Token: 0x060057D3 RID: 22483 RVA: 0x001AAE48 File Offset: 0x001A9048
		private static void SetInternalId(int blockIdx, int blockId, int bitsPerBlock, uint[] dst)
		{
			uint num = (1U << bitsPerBlock) - 1U;
			int num2 = blockIdx * bitsPerBlock;
			int num3 = num2 / 32;
			uint num4 = dst[num3];
			int num5 = num2 % 32;
			uint num6 = num << num5;
			num4 &= ~num6;
			uint num7 = (uint)((uint)blockId << num5);
			num4 |= num7;
			dst[num3] = num4;
			int num8 = (num2 + bitsPerBlock - 1) / 32;
			bool flag = num3 != num8;
			if (flag)
			{
				int num9 = 32 - num5;
				int num10 = bitsPerBlock - num9;
				dst[num8] = (dst[num8] >> num10 << num10 | (uint)(blockId >> num9));
			}
		}

		// Token: 0x060057D4 RID: 22484 RVA: 0x001AAED8 File Offset: 0x001A90D8
		public override BlockSetResult Set(int blockIdx, int blockId)
		{
			short internalId = this.GetInternalId(blockIdx);
			short num;
			bool flag = this._externalToInternal.TryGetValue(blockId, out num);
			BlockSetResult result;
			if (flag)
			{
				bool flag2 = num != internalId;
				if (flag2)
				{
					bool flag3 = this.DecrementBlockCount(internalId);
					this.IncrementBlockCount(num);
					this.SetInternalId(blockIdx, (int)num);
					bool flag4 = flag3;
					if (flag4)
					{
						result = BlockSetResult.BLOCK_ADDED_OR_REMOVED;
					}
					else
					{
						result = BlockSetResult.BLOCK_CHANGED;
					}
				}
				else
				{
					result = BlockSetResult.BLOCK_UNCHANGED;
				}
			}
			else
			{
				int num2 = this.NextInternalId(internalId);
				bool flag5 = !this.IsValidInternalId(num2);
				if (flag5)
				{
					result = BlockSetResult.REQUIRES_PROMOTE;
				}
				else
				{
					this.DecrementBlockCount(internalId);
					short num3 = (short)num2;
					this.CreateBlockId(num3, blockId);
					this.SetInternalId(blockIdx, (int)num3);
					result = BlockSetResult.BLOCK_ADDED_OR_REMOVED;
				}
			}
			return result;
		}

		// Token: 0x060057D5 RID: 22485 RVA: 0x001AAF84 File Offset: 0x001A9184
		public override bool Contains(int blockId)
		{
			return this._externalToInternal.ContainsKey(blockId);
		}

		// Token: 0x060057D6 RID: 22486 RVA: 0x001AAFA4 File Offset: 0x001A91A4
		public override int BlockCount()
		{
			return this._typeCounts.Count;
		}

		// Token: 0x060057D7 RID: 22487 RVA: 0x001AAFC4 File Offset: 0x001A91C4
		public override int Count(int blockId)
		{
			short key;
			bool flag = this._externalToInternal.TryGetValue(blockId, out key);
			int result;
			if (flag)
			{
				result = (int)this._typeCounts[key];
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x060057D8 RID: 22488 RVA: 0x001AAFFC File Offset: 0x001A91FC
		public override HashSet<int> Blocks()
		{
			return new HashSet<int>(this._externalToInternal.Keys);
		}

		// Token: 0x060057D9 RID: 22489 RVA: 0x001AB020 File Offset: 0x001A9220
		public override Dictionary<int, ushort> BlockCounts()
		{
			Dictionary<int, ushort> dictionary = new Dictionary<int, ushort>();
			foreach (KeyValuePair<short, ushort> keyValuePair in this._typeCounts)
			{
				short key = keyValuePair.Key;
				ushort value = keyValuePair.Value;
				int key2 = this._internalToExternal[(int)key];
				dictionary[key2] = value;
			}
			return dictionary;
		}

		// Token: 0x060057DA RID: 22490 RVA: 0x001AB0A4 File Offset: 0x001A92A4
		public override bool ShouldDemote()
		{
			bool flag = this._demoteSize == 0;
			bool result;
			if (flag)
			{
				bool flag2 = !this._externalToInternal.ContainsKey(0);
				result = (!flag2 && this.BlockCount() == 1);
			}
			else
			{
				result = (this.BlockCount() <= this._demoteSize);
			}
			return result;
		}

		// Token: 0x060057DB RID: 22491 RVA: 0x001AB0F8 File Offset: 0x001A92F8
		public override IChunkData Demote()
		{
			Debug.Assert(this._promotionIndex - 1 >= 0, "Unable to demote: Invalid palette size.");
			this._promotionIndex--;
			bool flag = this._promotionIndex == 0;
			IChunkData result;
			if (flag)
			{
				result = EmptyPaletteChunkData.Instance;
			}
			else
			{
				this.Resize((int)BitPaletteChunkData.promotionTable[this._promotionIndex], true);
				result = this;
			}
			return result;
		}

		// Token: 0x060057DC RID: 22492 RVA: 0x001AB15C File Offset: 0x001A935C
		public override IChunkData Promote()
		{
			Debug.Assert(this._promotionIndex + 1 < BitPaletteChunkData.promotionTable.Length, "Unable to promote: Invalid palette size.");
			this._promotionIndex++;
			this.Resize((int)BitPaletteChunkData.promotionTable[this._promotionIndex], true);
			return this;
		}

		// Token: 0x060057DD RID: 22493 RVA: 0x001AB1AD File Offset: 0x001A93AD
		private void CreateBlockId(short internalId, int blockId)
		{
			this._internalToExternal[(int)internalId] = blockId;
			this._externalToInternal[blockId] = internalId;
			this._typeCounts[internalId] = 1;
		}

		// Token: 0x060057DE RID: 22494 RVA: 0x001AB1D8 File Offset: 0x001A93D8
		private bool DecrementBlockCount(short internalId)
		{
			ushort num = this._typeCounts[internalId];
			bool flag = num == 1;
			bool result;
			if (flag)
			{
				this._typeCounts.Remove(internalId);
				this._externalToInternal.Remove(this._internalToExternal[(int)internalId]);
				this._internalToExternal[(int)internalId] = 0;
				result = true;
			}
			else
			{
				this._typeCounts[internalId] = num - 1;
				result = false;
			}
			return result;
		}

		// Token: 0x060057DF RID: 22495 RVA: 0x001AB244 File Offset: 0x001A9444
		private void IncrementBlockCount(short internalId)
		{
			ushort num = this._typeCounts[internalId];
			this._typeCounts[internalId] = num + 1;
		}

		// Token: 0x060057E0 RID: 22496 RVA: 0x001AB270 File Offset: 0x001A9470
		private int NextInternalId(short oldInternalId)
		{
			bool flag = this._typeCounts[oldInternalId] == 1;
			int result;
			if (flag)
			{
				result = (int)oldInternalId;
			}
			else
			{
				short num;
				bool flag2 = !this._externalToInternal.TryGetValue(0, out num);
				if (flag2)
				{
					num = -1;
				}
				int num2 = 0;
				while (num2 < this._internalToExternal.Length && (this._internalToExternal[num2] > 0 || num2 == (int)num))
				{
					num2++;
				}
				result = num2;
			}
			return result;
		}

		// Token: 0x060057E1 RID: 22497 RVA: 0x001AB2E4 File Offset: 0x001A94E4
		private bool IsValidInternalId(int internalId)
		{
			return ((long)internalId & (long)((ulong)this._bitMask)) == (long)internalId;
		}

		// Token: 0x060057E2 RID: 22498 RVA: 0x001AB304 File Offset: 0x001A9504
		public override void Deserialize(BinaryReader reader, int maxValidBlockTypeId, PaletteType paletteType)
		{
			int bitsPerBlock = 0;
			bool flag = false;
			int num = 1;
			int num2 = 32768;
			switch (paletteType)
			{
			case 1:
				bitsPerBlock = 4;
				flag = true;
				num2 = 16384;
				break;
			case 2:
				bitsPerBlock = 8;
				flag = true;
				break;
			case 3:
				bitsPerBlock = 16;
				num = 2;
				break;
			}
			this._promotionIndex = BitPaletteChunkData.PaletteTypeToPromotionIndex(paletteType);
			this.Resize(bitsPerBlock, false);
			ushort num3 = reader.ReadUInt16();
			for (int i = 0; i < (int)num3; i++)
			{
				short num4 = flag ? ((short)reader.ReadByte()) : reader.ReadInt16();
				int num5 = reader.ReadInt32();
				ushort value = reader.ReadUInt16();
				bool flag2 = num5 > maxValidBlockTypeId;
				if (flag2)
				{
					num5 = 1;
				}
				this._internalToExternal[(int)num4] = num5;
				this._externalToInternal[num5] = num4;
				this._typeCounts[num4] = value;
			}
			MemoryStream memoryStream = (MemoryStream)reader.BaseStream;
			Buffer.BlockCopy(memoryStream.GetBuffer(), (int)memoryStream.Position, this._data, 0, num2 * num);
			memoryStream.Position += (long)(num2 * num);
		}

		// Token: 0x04003655 RID: 13909
		private Dictionary<int, short> _externalToInternal = new Dictionary<int, short>(16);

		// Token: 0x04003656 RID: 13910
		private Dictionary<short, ushort> _typeCounts = new Dictionary<short, ushort>(16);

		// Token: 0x04003657 RID: 13911
		private uint _bitMask;

		// Token: 0x04003658 RID: 13912
		private int _promotionIndex;

		// Token: 0x04003659 RID: 13913
		private int _demoteSize;

		// Token: 0x0400365A RID: 13914
		private int _bitsPerBlock;

		// Token: 0x0400365B RID: 13915
		private int[] _internalToExternal;

		// Token: 0x0400365C RID: 13916
		private uint[] _data;

		// Token: 0x0400365D RID: 13917
		private const int WordSize = 32;

		// Token: 0x0400365E RID: 13918
		private static readonly byte[] promotionTable = new byte[]
		{
			0,
			4,
			5,
			6,
			7,
			8,
			9,
			10,
			11,
			12,
			13,
			14,
			15,
			16
		};
	}
}
