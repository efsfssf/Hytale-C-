using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using HytaleClient.Protocol;

namespace HytaleClient.Data.Map.Chunk
{
	// Token: 0x02000AE3 RID: 2787
	public abstract class AbstractShortPaletteChunkData : IChunkData
	{
		// Token: 0x060057B8 RID: 22456 RVA: 0x001AA414 File Offset: 0x001A8614
		public AbstractShortPaletteChunkData(short[] blocks) : this(new Dictionary<int, short>(), new int[512], new BitArray(32768), new Dictionary<short, ushort>(), blocks)
		{
			this.externalToInternal[0] = 0;
			this.internalToExternal[0] = 0;
			this.internalIdSet[0] = true;
			this.internalIdCount[0] = 32768;
		}

		// Token: 0x060057B9 RID: 22457 RVA: 0x001AA47F File Offset: 0x001A867F
		protected AbstractShortPaletteChunkData(Dictionary<int, short> externalToInternal, int[] internalToExternal, BitArray internalIdSet, Dictionary<short, ushort> internalIdCount, short[] blocks)
		{
			this.externalToInternal = externalToInternal;
			this.internalToExternal = internalToExternal;
			this.internalIdSet = internalIdSet;
			this.internalIdCount = internalIdCount;
			this.blockData = blocks;
		}

		// Token: 0x060057BA RID: 22458 RVA: 0x001AA4B0 File Offset: 0x001A86B0
		public override int Get(int blockIdx)
		{
			short num = this.Get0(blockIdx);
			return this.internalToExternal[(int)num];
		}

		// Token: 0x060057BB RID: 22459 RVA: 0x001AA4D4 File Offset: 0x001A86D4
		public override BlockSetResult Set(int blockIdx, int blockId)
		{
			short num = this.Get0(blockIdx);
			bool flag = this.externalToInternal.ContainsKey(blockId);
			BlockSetResult result;
			if (flag)
			{
				short num2 = this.externalToInternal[blockId];
				bool flag2 = num2 != num;
				if (flag2)
				{
					bool flag3 = this.DecrementBlockCount(num);
					this.IncrementBlockCount(num2);
					this.Set0(blockIdx, num2);
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
				int num3 = this.NextInternalId(num);
				bool flag5 = !this.IsValidInternalId(num3);
				if (flag5)
				{
					result = BlockSetResult.REQUIRES_PROMOTE;
				}
				else
				{
					this.DecrementBlockCount(num);
					short num4 = (short)num3;
					this.CreateBlockId(num4, blockId);
					this.Set0(blockIdx, num4);
					result = BlockSetResult.BLOCK_ADDED_OR_REMOVED;
				}
			}
			return result;
		}

		// Token: 0x060057BC RID: 22460
		internal abstract short Get0(int idx);

		// Token: 0x060057BD RID: 22461
		internal abstract void Set0(int idx, short s);

		// Token: 0x060057BE RID: 22462 RVA: 0x001AA58C File Offset: 0x001A878C
		public override bool Contains(int blockId)
		{
			return this.externalToInternal.ContainsKey(blockId);
		}

		// Token: 0x060057BF RID: 22463 RVA: 0x001AA5AC File Offset: 0x001A87AC
		public override int BlockCount()
		{
			return this.internalIdCount.Count;
		}

		// Token: 0x060057C0 RID: 22464 RVA: 0x001AA5CC File Offset: 0x001A87CC
		public override int Count(int blockId)
		{
			bool flag = this.externalToInternal.ContainsKey(blockId);
			int result;
			if (flag)
			{
				short key = this.externalToInternal[blockId];
				result = (int)this.internalIdCount[key];
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x060057C1 RID: 22465 RVA: 0x001AA60C File Offset: 0x001A880C
		public override HashSet<int> Blocks()
		{
			return new HashSet<int>(this.externalToInternal.Keys);
		}

		// Token: 0x060057C2 RID: 22466 RVA: 0x001AA630 File Offset: 0x001A8830
		public override Dictionary<int, ushort> BlockCounts()
		{
			Dictionary<int, ushort> dictionary = new Dictionary<int, ushort>();
			foreach (KeyValuePair<short, ushort> keyValuePair in this.internalIdCount)
			{
				short key = keyValuePair.Key;
				ushort value = keyValuePair.Value;
				int key2 = this.internalToExternal[(int)key];
				dictionary[key2] = value;
			}
			return dictionary;
		}

		// Token: 0x060057C3 RID: 22467 RVA: 0x001AA6B4 File Offset: 0x001A88B4
		private void CreateBlockId(short internalId, int blockId)
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

		// Token: 0x060057C4 RID: 22468 RVA: 0x001AA724 File Offset: 0x001A8924
		private bool DecrementBlockCount(short internalId)
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

		// Token: 0x060057C5 RID: 22469 RVA: 0x001AA7A4 File Offset: 0x001A89A4
		private void IncrementBlockCount(short internalId)
		{
			ushort num = this.internalIdCount[internalId];
			this.internalIdCount[internalId] = num + 1;
		}

		// Token: 0x060057C6 RID: 22470 RVA: 0x001AA7D0 File Offset: 0x001A89D0
		private int NextInternalId(short oldInternalId)
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
				while (num < this.internalIdCount.Count && this.internalIdSet[num])
				{
					num++;
				}
				result = num;
			}
			return result;
		}

		// Token: 0x060057C7 RID: 22471
		protected abstract bool IsValidInternalId(int internalId);

		// Token: 0x060057C8 RID: 22472
		protected abstract int UnsignedInternalId(short internalId);

		// Token: 0x060057C9 RID: 22473 RVA: 0x001AA82C File Offset: 0x001A8A2C
		public override void Deserialize(BinaryReader reader, int maxValidBlockTypeId, PaletteType paletteType)
		{
			this.externalToInternal.Clear();
			this.internalIdCount.Clear();
			this.internalIdSet.SetAll(false);
			ushort num = reader.ReadUInt16();
			this.internalToExternal = new int[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				short num2 = reader.ReadInt16();
				int num3 = reader.ReadInt32();
				ushort value = reader.ReadUInt16();
				bool flag = num3 > maxValidBlockTypeId;
				if (flag)
				{
					num3 = 1;
				}
				this.externalToInternal[num3] = num2;
				bool flag2 = (int)num2 >= this.internalToExternal.Length;
				if (flag2)
				{
					Array.Resize<int>(ref this.internalToExternal, (int)(num2 + 1));
				}
				this.internalToExternal[(int)num2] = num3;
				this.internalIdSet[(int)num2] = true;
				this.internalIdCount[num2] = value;
			}
			MemoryStream memoryStream = (MemoryStream)reader.BaseStream;
			Buffer.BlockCopy(memoryStream.GetBuffer(), (int)memoryStream.Position, this.blockData, 0, this.blockData.Length * 2);
			memoryStream.Position += (long)(this.blockData.Length * 2);
		}

		// Token: 0x04003650 RID: 13904
		internal Dictionary<int, short> externalToInternal;

		// Token: 0x04003651 RID: 13905
		internal int[] internalToExternal;

		// Token: 0x04003652 RID: 13906
		internal BitArray internalIdSet;

		// Token: 0x04003653 RID: 13907
		internal Dictionary<short, ushort> internalIdCount;

		// Token: 0x04003654 RID: 13908
		internal short[] blockData;
	}
}
