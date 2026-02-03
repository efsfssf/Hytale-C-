using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using HytaleClient.Protocol;

namespace HytaleClient.Data.Map.Chunk
{
	// Token: 0x02000AE2 RID: 2786
	public abstract class AbstractBytePaletteChunkData : IChunkData
	{
		// Token: 0x060057A6 RID: 22438 RVA: 0x001A9F10 File Offset: 0x001A8110
		protected AbstractBytePaletteChunkData(byte[] blocks) : this(new Dictionary<int, byte>(), new int[16], new BitArray(256), new Dictionary<byte, ushort>(), blocks)
		{
			this.externalToInternal[0] = 0;
			this.internalToExternal[0] = 0;
			this.internalIdSet[0] = true;
			this.internalIdCount[0] = 32768;
		}

		// Token: 0x060057A7 RID: 22439 RVA: 0x001A9F78 File Offset: 0x001A8178
		protected AbstractBytePaletteChunkData(Dictionary<int, byte> externalToInternal, int[] internalToExternal, BitArray internalIdSet, Dictionary<byte, ushort> internalIdCount, byte[] blocks)
		{
			this.externalToInternal = externalToInternal;
			this.internalToExternal = internalToExternal;
			this.internalIdSet = internalIdSet;
			this.internalIdCount = internalIdCount;
			this.blockData = blocks;
		}

		// Token: 0x060057A8 RID: 22440 RVA: 0x001A9FA8 File Offset: 0x001A81A8
		public override int Get(int blockIdx)
		{
			byte b = this.Get0(blockIdx);
			return this.internalToExternal[(int)b];
		}

		// Token: 0x060057A9 RID: 22441 RVA: 0x001A9FCC File Offset: 0x001A81CC
		public override BlockSetResult Set(int blockIdx, int blockId)
		{
			byte b = this.Get0(blockIdx);
			bool flag = this.externalToInternal.ContainsKey(blockId);
			BlockSetResult result;
			if (flag)
			{
				byte b2 = this.externalToInternal[blockId];
				bool flag2 = b2 != b;
				if (flag2)
				{
					bool flag3 = this.DecrementBlockCount(b);
					this.IncrementBlockCount(b2);
					this.Set0(blockIdx, b2);
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
				int num = this.NextInternalId(b);
				bool flag5 = !this.IsValidInternalId(num);
				if (flag5)
				{
					result = BlockSetResult.REQUIRES_PROMOTE;
				}
				else
				{
					this.DecrementBlockCount(b);
					byte b3 = (byte)num;
					this.CreateBlockId(b3, blockId);
					this.Set0(blockIdx, b3);
					result = BlockSetResult.BLOCK_ADDED_OR_REMOVED;
				}
			}
			return result;
		}

		// Token: 0x060057AA RID: 22442
		internal abstract byte Get0(int idx);

		// Token: 0x060057AB RID: 22443
		internal abstract void Set0(int idx, byte b);

		// Token: 0x060057AC RID: 22444 RVA: 0x001AA084 File Offset: 0x001A8284
		public override bool Contains(int blockId)
		{
			return this.externalToInternal.ContainsKey(blockId);
		}

		// Token: 0x060057AD RID: 22445 RVA: 0x001AA0A4 File Offset: 0x001A82A4
		public override int BlockCount()
		{
			return this.internalIdCount.Count;
		}

		// Token: 0x060057AE RID: 22446 RVA: 0x001AA0C4 File Offset: 0x001A82C4
		public override int Count(int blockId)
		{
			bool flag = this.externalToInternal.ContainsKey(blockId);
			int result;
			if (flag)
			{
				byte key = this.externalToInternal[blockId];
				result = (int)this.internalIdCount[key];
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x060057AF RID: 22447 RVA: 0x001AA104 File Offset: 0x001A8304
		public override HashSet<int> Blocks()
		{
			return new HashSet<int>(this.externalToInternal.Keys);
		}

		// Token: 0x060057B0 RID: 22448 RVA: 0x001AA128 File Offset: 0x001A8328
		public override Dictionary<int, ushort> BlockCounts()
		{
			Dictionary<int, ushort> dictionary = new Dictionary<int, ushort>();
			foreach (KeyValuePair<byte, ushort> keyValuePair in this.internalIdCount)
			{
				byte key = keyValuePair.Key;
				ushort value = keyValuePair.Value;
				int key2 = this.internalToExternal[(int)key];
				dictionary[key2] = value;
			}
			return dictionary;
		}

		// Token: 0x060057B1 RID: 22449 RVA: 0x001AA1AC File Offset: 0x001A83AC
		private void CreateBlockId(byte internalId, int blockId)
		{
			bool flag = (int)internalId >= this.internalToExternal.Length;
			if (flag)
			{
				Array.Resize<int>(ref this.internalToExternal, this.internalToExternal.Length * 2);
			}
			this.internalToExternal[(int)internalId] = blockId;
			this.externalToInternal[blockId] = internalId;
			this.internalIdSet[this.UnsignedInternalId(internalId)] = true;
			this.internalIdCount[internalId] = 1;
		}

		// Token: 0x060057B2 RID: 22450 RVA: 0x001AA21C File Offset: 0x001A841C
		private bool DecrementBlockCount(byte internalId)
		{
			ushort num = this.internalIdCount[internalId];
			bool flag = num == 1;
			bool result;
			if (flag)
			{
				this.internalIdCount.Remove(internalId);
				this.externalToInternal.Remove(this.internalToExternal[(int)internalId]);
				this.internalToExternal[(int)internalId] = -1;
				this.internalIdSet[this.UnsignedInternalId(internalId)] = false;
				result = true;
			}
			else
			{
				this.internalIdCount[internalId] = num - 1;
				result = false;
			}
			return result;
		}

		// Token: 0x060057B3 RID: 22451 RVA: 0x001AA29C File Offset: 0x001A849C
		private void IncrementBlockCount(byte internalId)
		{
			ushort num = this.internalIdCount[internalId];
			this.internalIdCount[internalId] = num + 1;
		}

		// Token: 0x060057B4 RID: 22452 RVA: 0x001AA2C8 File Offset: 0x001A84C8
		private int NextInternalId(byte oldInternalId)
		{
			bool flag = this.internalIdCount[oldInternalId] == 1;
			int result;
			if (flag)
			{
				result = this.UnsignedInternalId(oldInternalId);
			}
			else
			{
				int num = 0;
				while (num < 256 && this.internalIdSet[num])
				{
					num++;
				}
				result = num;
			}
			return result;
		}

		// Token: 0x060057B5 RID: 22453
		protected abstract bool IsValidInternalId(int internalId);

		// Token: 0x060057B6 RID: 22454
		protected abstract int UnsignedInternalId(byte internalId);

		// Token: 0x060057B7 RID: 22455 RVA: 0x001AA320 File Offset: 0x001A8520
		public override void Deserialize(BinaryReader reader, int maxValidBlockTypeId, PaletteType paletteType)
		{
			this.externalToInternal.Clear();
			this.internalIdCount.Clear();
			this.internalIdSet.SetAll(false);
			ushort num = reader.ReadUInt16();
			this.internalToExternal = new int[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				byte b = reader.ReadByte();
				int num2 = reader.ReadInt32();
				ushort value = reader.ReadUInt16();
				bool flag = num2 > maxValidBlockTypeId;
				if (flag)
				{
					num2 = 1;
				}
				this.externalToInternal[num2] = b;
				bool flag2 = (int)b >= this.internalToExternal.Length;
				if (flag2)
				{
					Array.Resize<int>(ref this.internalToExternal, (int)(b + 1));
				}
				this.internalToExternal[(int)b] = num2;
				this.internalIdSet[(int)b] = true;
				this.internalIdCount[b] = value;
			}
			reader.Read(this.blockData, 0, this.blockData.Length);
		}

		// Token: 0x0400364B RID: 13899
		internal Dictionary<int, byte> externalToInternal;

		// Token: 0x0400364C RID: 13900
		internal int[] internalToExternal;

		// Token: 0x0400364D RID: 13901
		internal BitArray internalIdSet;

		// Token: 0x0400364E RID: 13902
		internal Dictionary<byte, ushort> internalIdCount;

		// Token: 0x0400364F RID: 13903
		internal byte[] blockData;
	}
}
