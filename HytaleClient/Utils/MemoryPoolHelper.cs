using System;
using System.Diagnostics;
using System.Threading;

namespace HytaleClient.Utils
{
	// Token: 0x020007C6 RID: 1990
	public struct MemoryPoolHelper
	{
		// Token: 0x17000F7F RID: 3967
		// (get) Token: 0x0600338B RID: 13195 RVA: 0x00050614 File Offset: 0x0004E814
		public int MemorySlotCount
		{
			get
			{
				return this._usedSlotsBlocks.Length * 64;
			}
		}

		// Token: 0x0600338C RID: 13196 RVA: 0x00050624 File Offset: 0x0004E824
		public MemoryPoolHelper(int memorySlotsCount)
		{
			int num = memorySlotsCount / 64;
			num += ((memorySlotsCount % 64 == 0) ? 0 : 1);
			this._usedSlotsBlocks = new ulong[num];
		}

		// Token: 0x0600338D RID: 13197 RVA: 0x00050650 File Offset: 0x0004E850
		public void ClearMemorySlots()
		{
			for (int i = 0; i < this._usedSlotsBlocks.Length; i++)
			{
				this._usedSlotsBlocks[i] = 0UL;
			}
		}

		// Token: 0x0600338E RID: 13198 RVA: 0x00050680 File Offset: 0x0004E880
		public void ReleaseMemorySlots(int slot, int slotCount)
		{
			int num = slot / 64;
			int bitId = slot % 64;
			ulong num2 = this._usedSlotsBlocks[num];
			BitUtils.SwitchOffConsecutiveBits(bitId, slotCount, ref num2);
			this._usedSlotsBlocks[num] = num2;
		}

		// Token: 0x0600338F RID: 13199 RVA: 0x000506BC File Offset: 0x0004E8BC
		public void ThreadSafeReleaseMemorySlot(int slot, int slotCount)
		{
			int num = slot / 64;
			int bitId = slot % 64;
			ulong num2;
			ulong value;
			do
			{
				num2 = this._usedSlotsBlocks[num];
				value = num2;
				BitUtils.SwitchOffConsecutiveBits(bitId, slotCount, ref value);
			}
			while (num2 != MemoryPoolHelper.InterlockedCompareExchange(ref this._usedSlotsBlocks[num], value, num2));
		}

		// Token: 0x06003390 RID: 13200 RVA: 0x00050710 File Offset: 0x0004E910
		public int TakeMemorySlots(int slotCount)
		{
			bool flag = slotCount > 64;
			if (flag)
			{
				throw new Exception("Cannot allocate more than 64 contiguous MemorySlot slots.");
			}
			int result = -1;
			bool flag2 = slotCount > 32;
			if (flag2)
			{
				for (int i = 0; i < this._usedSlotsBlocks.Length; i++)
				{
					ulong num = this._usedSlotsBlocks[i];
					bool flag3 = num == 0UL;
					if (flag3)
					{
						result = i * 64;
						num = (ulong)-1;
						BitUtils.SwitchOnConsecutiveBits(32, slotCount - 32, ref num);
						this._usedSlotsBlocks[i] = num;
						break;
					}
				}
			}
			else
			{
				for (int j = 0; j < this._usedSlotsBlocks.Length; j++)
				{
					ulong num2 = this._usedSlotsBlocks[j];
					bool flag4 = num2 != ulong.MaxValue;
					if (flag4)
					{
						uint bitfield = (uint)(num2 & (ulong)-1);
						uint bitfield2 = (uint)((num2 & 18446744069414584320UL) >> 32);
						uint num3 = BitUtils.CountBitsOn(bitfield);
						uint num4 = BitUtils.CountBitsOn(bitfield2);
						bool flag5 = (ulong)(32U - num3) >= (ulong)((long)slotCount);
						if (flag5)
						{
							int num5 = BitUtils.FindFirstContiguousBitsOff(bitfield, slotCount);
							bool flag6 = num5 >= 0;
							if (flag6)
							{
								num5 = num5;
								result = j * 64 + num5;
								BitUtils.SwitchOnConsecutiveBits(num5, slotCount, ref num2);
								this._usedSlotsBlocks[j] = num2;
								break;
							}
						}
						else
						{
							bool flag7 = (ulong)(32U - num4) >= (ulong)((long)slotCount);
							if (flag7)
							{
								int num5 = BitUtils.FindFirstContiguousBitsOff(bitfield2, slotCount);
								bool flag8 = num5 >= 0;
								if (flag8)
								{
									num5 += 32;
									result = j * 64 + num5;
									BitUtils.SwitchOnConsecutiveBits(num5, slotCount, ref num2);
									this._usedSlotsBlocks[j] = num2;
									break;
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06003391 RID: 13201 RVA: 0x000508CC File Offset: 0x0004EACC
		public int ThreadSafeTakeMemorySlot(int slotCount)
		{
			bool flag = slotCount > 64;
			if (flag)
			{
				throw new Exception("Cannot allocate more than 32 contiguous MemorySlot slots.");
			}
			int result = -1;
			bool flag2 = slotCount > 32;
			if (flag2)
			{
				bool flag3 = false;
				do
				{
					for (int i = 0; i < this._usedSlotsBlocks.Length; i++)
					{
						ulong num = this._usedSlotsBlocks[i];
						bool flag4 = num == 0UL;
						if (flag4)
						{
							ulong value = num;
							result = i * 64;
							value = (ulong)-1;
							BitUtils.SwitchOnConsecutiveBits(32, slotCount - 32, ref value);
							flag3 = (num != MemoryPoolHelper.InterlockedCompareExchange(ref this._usedSlotsBlocks[i], value, num));
							break;
						}
					}
				}
				while (flag3);
			}
			else
			{
				bool flag5 = false;
				do
				{
					for (int j = 0; j < this._usedSlotsBlocks.Length; j++)
					{
						ulong num2 = this._usedSlotsBlocks[j];
						bool flag6 = num2 != ulong.MaxValue;
						if (flag6)
						{
							ulong num3 = num2;
							uint bitfield = (uint)(num3 & (ulong)-1);
							uint bitfield2 = (uint)((num3 & 18446744069414584320UL) >> 32);
							uint num4 = BitUtils.CountBitsOn(bitfield);
							uint num5 = BitUtils.CountBitsOn(bitfield2);
							bool flag7 = (ulong)(32U - num4) >= (ulong)((long)slotCount);
							if (flag7)
							{
								int num6 = BitUtils.FindFirstContiguousBitsOff(bitfield, slotCount);
								bool flag8 = num6 >= 0;
								if (flag8)
								{
									num6 = num6;
									result = j * 64 + num6;
									BitUtils.SwitchOnConsecutiveBits(num6, slotCount, ref num3);
									flag5 = (num2 != MemoryPoolHelper.InterlockedCompareExchange(ref this._usedSlotsBlocks[j], num3, num2));
									break;
								}
							}
							else
							{
								bool flag9 = (ulong)(32U - num5) >= (ulong)((long)slotCount);
								if (flag9)
								{
									num3 = num2;
									int num6 = BitUtils.FindFirstContiguousBitsOff(bitfield2, slotCount);
									bool flag10 = num6 >= 0;
									if (flag10)
									{
										num6 += 32;
										result = j * 64 + num6;
										BitUtils.SwitchOnConsecutiveBits(num6, slotCount, ref num3);
										flag5 = (num2 != MemoryPoolHelper.InterlockedCompareExchange(ref this._usedSlotsBlocks[j], num3, num2));
										break;
									}
								}
							}
						}
					}
				}
				while (flag5);
			}
			return result;
		}

		// Token: 0x06003392 RID: 13202 RVA: 0x00050AF0 File Offset: 0x0004ECF0
		private unsafe static ulong InterlockedCompareExchange(ref ulong location, ulong value, ulong comparand)
		{
			fixed (ulong* ptr = &location)
			{
				ulong* location2 = ptr;
				return (ulong)Interlocked.CompareExchange(ref *(long*)location2, (long)value, (long)comparand);
			}
		}

		// Token: 0x06003393 RID: 13203 RVA: 0x00050B10 File Offset: 0x0004ED10
		public static void UnitTest()
		{
			MemoryPoolHelper.TestData[] array = new MemoryPoolHelper.TestData[4];
			MemoryPoolHelper memoryPoolHelper = new MemoryPoolHelper(1024);
			array[0] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 1,
				ExpectedResultInt = 0
			};
			array[1] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 1,
				ExpectedResultInt = 0
			};
			array[2] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 4,
				ExpectedResultInt = 0
			};
			array[3] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 4,
				ExpectedResultInt = 0
			};
			array[0].ResultInt = memoryPoolHelper.TakeMemorySlots(array[0].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[0].ResultInt, array[0].InputInt0);
			array[1].ResultInt = memoryPoolHelper.TakeMemorySlots(array[1].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[1].ResultInt, array[1].InputInt0);
			array[2].ResultInt = memoryPoolHelper.TakeMemorySlots(array[2].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[2].ResultInt, array[2].InputInt0);
			array[3].ResultInt = memoryPoolHelper.TakeMemorySlots(array[3].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[3].ResultInt, array[3].InputInt0);
			for (int i = 0; i < 4; i++)
			{
				Debug.Assert(array[i].ResultInt == array[i].ExpectedResultInt, string.Format("Error in test data {0}.", i));
			}
			array[0] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 1,
				ExpectedResultInt = 0
			};
			array[1] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 1,
				ExpectedResultInt = 1
			};
			array[2] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 4,
				ExpectedResultInt = 2
			};
			array[3] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 4,
				ExpectedResultInt = 6
			};
			array[0].ResultInt = memoryPoolHelper.TakeMemorySlots(array[0].InputInt0);
			array[1].ResultInt = memoryPoolHelper.TakeMemorySlots(array[1].InputInt0);
			array[2].ResultInt = memoryPoolHelper.TakeMemorySlots(array[2].InputInt0);
			array[3].ResultInt = memoryPoolHelper.TakeMemorySlots(array[3].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[0].ResultInt, array[0].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[1].ResultInt, array[1].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[2].ResultInt, array[2].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[3].ResultInt, array[3].InputInt0);
			for (int j = 0; j < 4; j++)
			{
				Debug.Assert(array[j].ResultInt == array[j].ExpectedResultInt, string.Format("Error in test data {0}.", j));
			}
			array[0] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 4,
				ExpectedResultInt = 0
			};
			array[1] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 4,
				ExpectedResultInt = 4
			};
			array[2] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 1,
				ExpectedResultInt = 0
			};
			array[3] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 1,
				ExpectedResultInt = 1
			};
			array[0].ResultInt = memoryPoolHelper.TakeMemorySlots(array[0].InputInt0);
			array[1].ResultInt = memoryPoolHelper.TakeMemorySlots(array[1].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[0].ResultInt, array[0].InputInt0);
			array[2].ResultInt = memoryPoolHelper.TakeMemorySlots(array[2].InputInt0);
			array[3].ResultInt = memoryPoolHelper.TakeMemorySlots(array[3].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[1].ResultInt, array[1].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[2].ResultInt, array[2].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[3].ResultInt, array[3].InputInt0);
			for (int k = 0; k < 4; k++)
			{
				Debug.Assert(array[k].ResultInt == array[k].ExpectedResultInt, string.Format("Error in test data {0}.", k));
			}
			array[0] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 4,
				ExpectedResultInt = 0
			};
			array[1] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 4,
				ExpectedResultInt = 4
			};
			array[2] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 5,
				ExpectedResultInt = 8
			};
			array[3] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 1,
				ExpectedResultInt = 0
			};
			array[0].ResultInt = memoryPoolHelper.TakeMemorySlots(array[0].InputInt0);
			array[1].ResultInt = memoryPoolHelper.TakeMemorySlots(array[1].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[0].ResultInt, array[0].InputInt0);
			array[2].ResultInt = memoryPoolHelper.TakeMemorySlots(array[2].InputInt0);
			array[3].ResultInt = memoryPoolHelper.TakeMemorySlots(array[3].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[1].ResultInt, array[1].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[2].ResultInt, array[2].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[3].ResultInt, array[3].InputInt0);
			for (int l = 0; l < 4; l++)
			{
				Debug.Assert(array[l].ResultInt == array[l].ExpectedResultInt, string.Format("Error in test data {0}.", l));
			}
			array[0] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 16,
				ExpectedResultInt = 0
			};
			array[1] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 16,
				ExpectedResultInt = 16
			};
			array[2] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 32,
				ExpectedResultInt = 32
			};
			array[3] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 1,
				ExpectedResultInt = 0
			};
			array[0].ResultInt = memoryPoolHelper.TakeMemorySlots(array[0].InputInt0);
			array[1].ResultInt = memoryPoolHelper.TakeMemorySlots(array[1].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[0].ResultInt, array[0].InputInt0);
			array[2].ResultInt = memoryPoolHelper.TakeMemorySlots(array[2].InputInt0);
			array[3].ResultInt = memoryPoolHelper.TakeMemorySlots(array[3].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[1].ResultInt, array[1].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[2].ResultInt, array[2].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[3].ResultInt, array[3].InputInt0);
			for (int m = 0; m < 4; m++)
			{
				Debug.Assert(array[m].ResultInt == array[m].ExpectedResultInt, string.Format("Error in test data {0}.", m));
			}
			array[0] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 32,
				ExpectedResultInt = 0
			};
			array[1] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 32,
				ExpectedResultInt = 32
			};
			array[2] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 32,
				ExpectedResultInt = 64
			};
			array[3] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 1,
				ExpectedResultInt = 32
			};
			array[0].ResultInt = memoryPoolHelper.TakeMemorySlots(array[0].InputInt0);
			array[1].ResultInt = memoryPoolHelper.TakeMemorySlots(array[1].InputInt0);
			array[2].ResultInt = memoryPoolHelper.TakeMemorySlots(array[2].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[1].ResultInt, array[1].InputInt0);
			array[3].ResultInt = memoryPoolHelper.TakeMemorySlots(array[3].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[0].ResultInt, array[0].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[2].ResultInt, array[2].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[3].ResultInt, array[3].InputInt0);
			for (int n = 0; n < 4; n++)
			{
				Debug.Assert(array[n].ResultInt == array[n].ExpectedResultInt, string.Format("Error in test data {0}.", n));
			}
			array[0] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 32,
				ExpectedResultInt = 0
			};
			array[1] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 33,
				ExpectedResultInt = 64
			};
			array[2] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 64,
				ExpectedResultInt = 128
			};
			array[3] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 4,
				ExpectedResultInt = 32
			};
			array[0].ResultInt = memoryPoolHelper.TakeMemorySlots(array[0].InputInt0);
			array[1].ResultInt = memoryPoolHelper.TakeMemorySlots(array[1].InputInt0);
			array[2].ResultInt = memoryPoolHelper.TakeMemorySlots(array[2].InputInt0);
			array[3].ResultInt = memoryPoolHelper.TakeMemorySlots(array[3].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[0].ResultInt, array[0].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[1].ResultInt, array[1].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[2].ResultInt, array[2].InputInt0);
			memoryPoolHelper.ReleaseMemorySlots(array[3].ResultInt, array[3].InputInt0);
			for (int num = 0; num < 4; num++)
			{
				Debug.Assert(array[num].ResultInt == array[num].ExpectedResultInt, string.Format("Error in test data {0}.", num));
			}
			array[0] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 32,
				ExpectedResultInt = 0
			};
			array[1] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 33,
				ExpectedResultInt = 0
			};
			array[2] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 64,
				ExpectedResultInt = 0
			};
			array[3] = new MemoryPoolHelper.TestData
			{
				InputInt0 = 65,
				ExpectedResultInt = 1
			};
			for (int num2 = 0; num2 < 4; num2++)
			{
				try
				{
					array[num2].ResultInt = 0;
					int num3 = memoryPoolHelper.TakeMemorySlots(array[num2].InputInt0);
				}
				catch (Exception)
				{
					array[num2].ResultInt = 1;
				}
			}
			for (int num4 = 0; num4 < 4; num4++)
			{
				Debug.Assert(array[num4].ResultInt == array[num4].ExpectedResultInt, string.Format("Error in test data {0}.", num4));
			}
		}

		// Token: 0x04001726 RID: 5926
		private ulong[] _usedSlotsBlocks;

		// Token: 0x02000C16 RID: 3094
		private struct TestData
		{
			// Token: 0x04003D69 RID: 15721
			public int InputInt0;

			// Token: 0x04003D6A RID: 15722
			public int InputInt1;

			// Token: 0x04003D6B RID: 15723
			public int ResultInt;

			// Token: 0x04003D6C RID: 15724
			public int ExpectedResultInt;

			// Token: 0x04003D6D RID: 15725
			public ulong InputULong0;

			// Token: 0x04003D6E RID: 15726
			public ulong InputULong1;

			// Token: 0x04003D6F RID: 15727
			public ulong ResultULong;

			// Token: 0x04003D70 RID: 15728
			public ulong ExpectedResultULong;
		}
	}
}
