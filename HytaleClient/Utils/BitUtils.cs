using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace HytaleClient.Utils
{
	// Token: 0x020007B3 RID: 1971
	public static class BitUtils
	{
		// Token: 0x06003317 RID: 13079 RVA: 0x0004C944 File Offset: 0x0004AB44
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int FindFirstContiguousBitsOff(uint bitfield, int requestedConsecutiveBits)
		{
			int result = -1;
			uint num = 0U;
			int num2 = 0;
			uint num3 = bitfield;
			int num4 = 0;
			while (num4 < 32 && (ulong)num < (ulong)((long)requestedConsecutiveBits))
			{
				bool flag = (num3 & 1U) == 1U;
				if (flag)
				{
					num = 0U;
					num2 = num4 + 1;
				}
				else
				{
					num += 1U;
				}
				num3 >>= 1;
				num4++;
			}
			bool flag2 = (ulong)num > (ulong)((long)requestedConsecutiveBits);
			if (flag2)
			{
				throw new Exception("This should never happen : 'consecutiveBits > requestedConsecutiveBits'.");
			}
			bool flag3 = (ulong)num == (ulong)((long)requestedConsecutiveBits);
			if (flag3)
			{
				result = num2;
			}
			return result;
		}

		// Token: 0x06003318 RID: 13080 RVA: 0x0004C9CC File Offset: 0x0004ABCC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SwitchOnConsecutiveBits(int bitId, int consecutiveBits, ref ulong bitfield)
		{
			for (int i = bitId; i < bitId + consecutiveBits; i++)
			{
				bitfield |= 1UL << i;
			}
		}

		// Token: 0x06003319 RID: 13081 RVA: 0x0004C9FC File Offset: 0x0004ABFC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SwitchOffConsecutiveBits(int bitId, int consecutiveBits, ref ulong bitfield)
		{
			for (int i = bitId; i < bitId + consecutiveBits; i++)
			{
				bitfield &= ~(1UL << i);
			}
		}

		// Token: 0x0600331A RID: 13082 RVA: 0x0004CA2C File Offset: 0x0004AC2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint FindFirstBitOff(uint bitfield)
		{
			bitfield = ~bitfield;
			return BitUtils.CountBitsOn((uint)(((ulong)bitfield & -(ulong)bitfield) - 1UL));
		}

		// Token: 0x0600331B RID: 13083 RVA: 0x0004CA54 File Offset: 0x0004AC54
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint FindFirstBitOn(uint bitfield)
		{
			return BitUtils.CountBitsOn((uint)(((ulong)bitfield & -(ulong)bitfield) - 1UL));
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x0004CA78 File Offset: 0x0004AC78
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SwitchOnBit(int bitId, ref uint bitfield)
		{
			uint num = 1U << bitId % 32;
			bitfield |= num;
		}

		// Token: 0x0600331D RID: 13085 RVA: 0x0004CA96 File Offset: 0x0004AC96
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SwitchOnBit(int bitId, ref byte bitfield)
		{
			bitfield |= (byte)(1 << bitId);
		}

		// Token: 0x0600331E RID: 13086 RVA: 0x0004CAA8 File Offset: 0x0004ACA8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SwitchOffBit(int bitId, ref uint bitfield)
		{
			uint num = 1U << bitId % 32;
			bitfield &= ~num;
		}

		// Token: 0x0600331F RID: 13087 RVA: 0x0004CAC7 File Offset: 0x0004ACC7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SwitchOffBit(int bitId, ref byte bitfield)
		{
			bitfield &= (byte)(~(byte)(1 << bitId));
		}

		// Token: 0x06003320 RID: 13088 RVA: 0x0004CAD8 File Offset: 0x0004ACD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsBitOn(int bitId, uint bitfield)
		{
			uint num = 1U << bitId % 32;
			return (bitfield & num) > 0U;
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x0004CAFC File Offset: 0x0004ACFC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsBitOn(int bitId, byte bitfield)
		{
			return ((int)bitfield & 1 << bitId) != 0;
		}

		// Token: 0x06003322 RID: 13090 RVA: 0x0004CB1C File Offset: 0x0004AD1C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint CountBitsOn(uint bitfield)
		{
			uint num = bitfield - (bitfield >> 1 & 1431655765U);
			num = (num & 858993459U) + (num >> 2 & 858993459U);
			return (num + (num >> 4) & 252645135U) * 16843009U >> 24;
		}

		// Token: 0x06003323 RID: 13091 RVA: 0x0004CB64 File Offset: 0x0004AD64
		public static void UnitTest()
		{
			BitUtils.TestData[] array = new BitUtils.TestData[]
			{
				new BitUtils.TestData
				{
					InputInt0 = 0,
					ExpectedResultInt = 0
				},
				new BitUtils.TestData
				{
					InputInt0 = 1,
					ExpectedResultInt = 1
				},
				new BitUtils.TestData
				{
					InputInt0 = 31,
					ExpectedResultInt = 5
				},
				new BitUtils.TestData
				{
					InputInt0 = 95,
					ExpectedResultInt = 6
				}
			};
			array[0].ResultInt = (int)BitUtils.CountBitsOn((uint)array[0].InputInt0);
			array[1].ResultInt = (int)BitUtils.CountBitsOn((uint)array[1].InputInt0);
			array[2].ResultInt = (int)BitUtils.CountBitsOn((uint)array[2].InputInt0);
			array[3].ResultInt = (int)BitUtils.CountBitsOn((uint)array[3].InputInt0);
			for (int i = 0; i < 4; i++)
			{
				Debug.Assert(array[i].ResultInt == array[i].ExpectedResultInt, string.Format("Error in test data {0}.", i));
			}
			array[0] = new BitUtils.TestData
			{
				InputUInt0 = 0U,
				ExpectedResultInt = 0
			};
			array[1] = new BitUtils.TestData
			{
				InputUInt0 = 1U,
				ExpectedResultInt = 1
			};
			array[2] = new BitUtils.TestData
			{
				InputUInt0 = 95U,
				ExpectedResultInt = 5
			};
			array[3] = new BitUtils.TestData
			{
				InputUInt0 = uint.MaxValue,
				ExpectedResultInt = 32
			};
			array[0].ResultInt = (int)BitUtils.FindFirstBitOff(array[0].InputUInt0);
			array[1].ResultInt = (int)BitUtils.FindFirstBitOff(array[1].InputUInt0);
			array[2].ResultInt = (int)BitUtils.FindFirstBitOff(array[2].InputUInt0);
			array[3].ResultInt = (int)BitUtils.FindFirstBitOff(array[3].InputUInt0);
			for (int j = 0; j < 4; j++)
			{
				Debug.Assert(array[j].ResultInt == array[j].ExpectedResultInt, string.Format("Error in test data {0}.", j));
			}
			array[0] = new BitUtils.TestData
			{
				InputUInt0 = 0U,
				ExpectedResultInt = 32
			};
			array[1] = new BitUtils.TestData
			{
				InputUInt0 = 1U,
				ExpectedResultInt = 0
			};
			array[2] = new BitUtils.TestData
			{
				InputUInt0 = 80U,
				ExpectedResultInt = 4
			};
			array[3] = new BitUtils.TestData
			{
				InputUInt0 = 16773376U,
				ExpectedResultInt = 8
			};
			array[0].ResultInt = (int)BitUtils.FindFirstBitOn(array[0].InputUInt0);
			array[1].ResultInt = (int)BitUtils.FindFirstBitOn(array[1].InputUInt0);
			array[2].ResultInt = (int)BitUtils.FindFirstBitOn(array[2].InputUInt0);
			array[3].ResultInt = (int)BitUtils.FindFirstBitOn(array[3].InputUInt0);
			for (int k = 0; k < 4; k++)
			{
				Debug.Assert(array[k].ResultInt == array[k].ExpectedResultInt, string.Format("Error in test data {0}.", k));
			}
			array[0] = new BitUtils.TestData
			{
				InputInt0 = 0,
				InputUInt0 = 0U,
				ExpectedResultInt = 0
			};
			array[1] = new BitUtils.TestData
			{
				InputInt0 = 0,
				InputUInt0 = 1U,
				ExpectedResultInt = 1
			};
			array[2] = new BitUtils.TestData
			{
				InputInt0 = 5,
				InputUInt0 = 95U,
				ExpectedResultInt = 0
			};
			array[3] = new BitUtils.TestData
			{
				InputInt0 = 1,
				InputUInt0 = uint.MaxValue,
				ExpectedResultInt = 1
			};
			array[0].ResultInt = (BitUtils.IsBitOn(array[0].InputInt0, array[0].InputUInt0) ? 1 : 0);
			array[1].ResultInt = (BitUtils.IsBitOn(array[1].InputInt0, array[1].InputUInt0) ? 1 : 0);
			array[2].ResultInt = (BitUtils.IsBitOn(array[2].InputInt0, array[2].InputUInt0) ? 1 : 0);
			array[3].ResultInt = (BitUtils.IsBitOn(array[3].InputInt0, array[3].InputUInt0) ? 1 : 0);
			for (int l = 0; l < 4; l++)
			{
				Debug.Assert(array[l].ResultInt == array[l].ExpectedResultInt, string.Format("Error in test data {0}.", l));
			}
			array[0] = new BitUtils.TestData
			{
				InputInt0 = 0,
				InputByte = 14,
				ExpectedResultInt = 0
			};
			array[1] = new BitUtils.TestData
			{
				InputInt0 = 0,
				InputByte = 15,
				ExpectedResultInt = 1
			};
			array[2] = new BitUtils.TestData
			{
				InputInt0 = 2,
				InputByte = 11,
				ExpectedResultInt = 0
			};
			array[3] = new BitUtils.TestData
			{
				InputInt0 = 2,
				InputByte = 15,
				ExpectedResultInt = 1
			};
			array[0].ResultInt = (BitUtils.IsBitOn(array[0].InputInt0, array[0].InputByte) ? 1 : 0);
			array[1].ResultInt = (BitUtils.IsBitOn(array[1].InputInt0, array[1].InputByte) ? 1 : 0);
			array[2].ResultInt = (BitUtils.IsBitOn(array[2].InputInt0, array[2].InputByte) ? 1 : 0);
			array[3].ResultInt = (BitUtils.IsBitOn(array[3].InputInt0, array[3].InputByte) ? 1 : 0);
			for (int m = 0; m < 4; m++)
			{
				Debug.Assert(array[m].ResultInt == array[m].ExpectedResultInt, string.Format("Error in test data {0}.", m));
			}
			array[0] = new BitUtils.TestData
			{
				InputInt0 = 0,
				InputUInt0 = 0U,
				ExpectedResultUInt = 1U
			};
			array[1] = new BitUtils.TestData
			{
				InputInt0 = 1,
				InputUInt0 = 1U,
				ExpectedResultUInt = 3U
			};
			array[2] = new BitUtils.TestData
			{
				InputInt0 = 8,
				InputUInt0 = 95U,
				ExpectedResultUInt = 351U
			};
			array[3] = new BitUtils.TestData
			{
				InputInt0 = 1,
				InputUInt0 = uint.MaxValue,
				ExpectedResultUInt = uint.MaxValue
			};
			uint inputUInt = array[0].InputUInt0;
			BitUtils.SwitchOnBit(array[0].InputInt0, ref inputUInt);
			array[0].ResultUInt = inputUInt;
			inputUInt = array[1].InputUInt0;
			BitUtils.SwitchOnBit(array[1].InputInt0, ref inputUInt);
			array[1].ResultUInt = inputUInt;
			inputUInt = array[2].InputUInt0;
			BitUtils.SwitchOnBit(array[2].InputInt0, ref inputUInt);
			array[2].ResultUInt = inputUInt;
			inputUInt = array[3].InputUInt0;
			BitUtils.SwitchOnBit(array[3].InputInt0, ref inputUInt);
			array[3].ResultUInt = inputUInt;
			for (int n = 0; n < 4; n++)
			{
				Debug.Assert(array[n].ResultUInt == array[n].ExpectedResultUInt, string.Format("Error in test data {0}.", n));
			}
			array[0] = new BitUtils.TestData
			{
				InputInt0 = 0,
				InputByte = 0,
				ExpectedResultByte = 1
			};
			array[1] = new BitUtils.TestData
			{
				InputInt0 = 1,
				InputByte = 2,
				ExpectedResultByte = 2
			};
			array[2] = new BitUtils.TestData
			{
				InputInt0 = 2,
				InputByte = 0,
				ExpectedResultByte = 4
			};
			array[3] = new BitUtils.TestData
			{
				InputInt0 = 3,
				InputByte = 8,
				ExpectedResultByte = 8
			};
			byte inputByte = array[0].InputByte;
			BitUtils.SwitchOnBit(array[0].InputInt0, ref inputByte);
			array[0].ResultByte = inputByte;
			inputByte = array[1].InputByte;
			BitUtils.SwitchOnBit(array[1].InputInt0, ref inputByte);
			array[1].ResultByte = inputByte;
			inputByte = array[2].InputByte;
			BitUtils.SwitchOnBit(array[2].InputInt0, ref inputByte);
			array[2].ResultByte = inputByte;
			inputByte = array[3].InputByte;
			BitUtils.SwitchOnBit(array[3].InputInt0, ref inputByte);
			array[3].ResultByte = inputByte;
			for (int num = 0; num < 4; num++)
			{
				Debug.Assert(array[num].ResultByte == array[num].ExpectedResultByte, string.Format("Error in test data {0}.", num));
			}
			array[0] = new BitUtils.TestData
			{
				InputInt0 = 0,
				InputUInt0 = 1U,
				ExpectedResultUInt = 0U
			};
			array[1] = new BitUtils.TestData
			{
				InputInt0 = 1,
				InputUInt0 = 3U,
				ExpectedResultUInt = 1U
			};
			array[2] = new BitUtils.TestData
			{
				InputInt0 = 8,
				InputUInt0 = 351U,
				ExpectedResultUInt = 95U
			};
			array[3] = new BitUtils.TestData
			{
				InputInt0 = 1,
				InputUInt0 = 0U,
				ExpectedResultUInt = 0U
			};
			uint inputUInt2 = array[0].InputUInt0;
			BitUtils.SwitchOffBit(array[0].InputInt0, ref inputUInt2);
			array[0].ResultUInt = inputUInt2;
			inputUInt2 = array[1].InputUInt0;
			BitUtils.SwitchOffBit(array[1].InputInt0, ref inputUInt2);
			array[1].ResultUInt = inputUInt2;
			inputUInt2 = array[2].InputUInt0;
			BitUtils.SwitchOffBit(array[2].InputInt0, ref inputUInt2);
			array[2].ResultUInt = inputUInt2;
			inputUInt2 = array[3].InputUInt0;
			BitUtils.SwitchOffBit(array[3].InputInt0, ref inputUInt2);
			array[3].ResultUInt = inputUInt2;
			for (int num2 = 0; num2 < 4; num2++)
			{
				Debug.Assert(array[num2].ResultUInt == array[num2].ExpectedResultUInt, string.Format("Error in test data {0}.", num2));
			}
			array[0] = new BitUtils.TestData
			{
				InputInt0 = 0,
				InputByte = 0,
				ExpectedResultByte = 0
			};
			array[1] = new BitUtils.TestData
			{
				InputInt0 = 1,
				InputByte = 2,
				ExpectedResultByte = 0
			};
			array[2] = new BitUtils.TestData
			{
				InputInt0 = 2,
				InputByte = 11,
				ExpectedResultByte = 11
			};
			array[3] = new BitUtils.TestData
			{
				InputInt0 = 3,
				InputByte = 15,
				ExpectedResultByte = 7
			};
			byte inputByte2 = array[0].InputByte;
			BitUtils.SwitchOffBit(array[0].InputInt0, ref inputByte2);
			array[0].ResultByte = inputByte2;
			inputByte2 = array[1].InputByte;
			BitUtils.SwitchOffBit(array[1].InputInt0, ref inputByte2);
			array[1].ResultByte = inputByte2;
			inputByte2 = array[2].InputByte;
			BitUtils.SwitchOffBit(array[2].InputInt0, ref inputByte2);
			array[2].ResultByte = inputByte2;
			inputByte2 = array[3].InputByte;
			BitUtils.SwitchOffBit(array[3].InputInt0, ref inputByte2);
			array[3].ResultByte = inputByte2;
			for (int num3 = 0; num3 < 4; num3++)
			{
				Debug.Assert(array[num3].ResultByte == array[num3].ExpectedResultByte, string.Format("Error in test data {0}.", num3));
			}
			array[0] = new BitUtils.TestData
			{
				InputInt0 = 0,
				InputInt1 = 1,
				InputULong0 = 0UL,
				ExpectedResultULong = 1UL
			};
			array[1] = new BitUtils.TestData
			{
				InputInt0 = 2,
				InputInt1 = 5,
				InputULong0 = 0UL,
				ExpectedResultULong = 124UL
			};
			array[2] = new BitUtils.TestData
			{
				InputInt0 = 2,
				InputInt1 = 20,
				InputULong0 = 0UL,
				ExpectedResultULong = 4194300UL
			};
			array[3] = new BitUtils.TestData
			{
				InputInt0 = 4,
				InputInt1 = 4,
				InputULong0 = 17293822569102704640UL,
				ExpectedResultULong = 17293822569102704880UL
			};
			ulong inputULong = array[0].InputULong0;
			BitUtils.SwitchOnConsecutiveBits(array[0].InputInt0, array[0].InputInt1, ref inputULong);
			array[0].ResultULong = inputULong;
			inputULong = array[1].InputULong0;
			BitUtils.SwitchOnConsecutiveBits(array[1].InputInt0, array[1].InputInt1, ref inputULong);
			array[1].ResultULong = inputULong;
			inputULong = array[2].InputULong0;
			BitUtils.SwitchOnConsecutiveBits(array[2].InputInt0, array[2].InputInt1, ref inputULong);
			array[2].ResultULong = inputULong;
			inputULong = array[3].InputULong0;
			BitUtils.SwitchOnConsecutiveBits(array[3].InputInt0, array[3].InputInt1, ref inputULong);
			array[3].ResultULong = inputULong;
			for (int num4 = 0; num4 < 4; num4++)
			{
				Debug.Assert(array[num4].ResultULong == array[num4].ExpectedResultULong, string.Format("Error in test data {0}.", num4));
			}
			array[0] = new BitUtils.TestData
			{
				InputInt0 = 0,
				InputInt1 = 1,
				InputULong0 = 1UL,
				ExpectedResultULong = 0UL
			};
			array[1] = new BitUtils.TestData
			{
				InputInt0 = 2,
				InputInt1 = 5,
				InputULong0 = 124UL,
				ExpectedResultULong = 0UL
			};
			array[2] = new BitUtils.TestData
			{
				InputInt0 = 2,
				InputInt1 = 20,
				InputULong0 = 4194300UL,
				ExpectedResultULong = 0UL
			};
			array[3] = new BitUtils.TestData
			{
				InputInt0 = 4,
				InputInt1 = 4,
				InputULong0 = 17293822569102704880UL,
				ExpectedResultULong = 17293822569102704640UL
			};
			ulong inputULong2 = array[0].InputULong0;
			BitUtils.SwitchOffConsecutiveBits(array[0].InputInt0, array[0].InputInt1, ref inputULong2);
			array[0].ResultULong = inputULong2;
			inputULong2 = array[1].InputULong0;
			BitUtils.SwitchOffConsecutiveBits(array[1].InputInt0, array[1].InputInt1, ref inputULong2);
			array[1].ResultULong = inputULong2;
			inputULong2 = array[2].InputULong0;
			BitUtils.SwitchOffConsecutiveBits(array[2].InputInt0, array[2].InputInt1, ref inputULong2);
			array[2].ResultULong = inputULong2;
			inputULong2 = array[3].InputULong0;
			BitUtils.SwitchOffConsecutiveBits(array[3].InputInt0, array[3].InputInt1, ref inputULong2);
			array[3].ResultULong = inputULong2;
			for (int num5 = 0; num5 < 4; num5++)
			{
				Debug.Assert(array[num5].ResultULong == array[num5].ExpectedResultULong, string.Format("Error in test data {0}.", num5));
			}
			array[0] = new BitUtils.TestData
			{
				InputInt0 = 0,
				InputInt1 = 6,
				ExpectedResultInt = 0
			};
			array[1] = new BitUtils.TestData
			{
				InputInt0 = 1,
				InputInt1 = 6,
				ExpectedResultInt = 1
			};
			array[2] = new BitUtils.TestData
			{
				InputInt0 = 8,
				InputInt1 = 8,
				ExpectedResultInt = 4
			};
			array[3] = new BitUtils.TestData
			{
				InputInt0 = 11184810,
				InputInt1 = 8,
				ExpectedResultInt = 24
			};
			array[0].ResultInt = BitUtils.FindFirstContiguousBitsOff((uint)array[0].InputInt0, array[0].InputInt1);
			array[1].ResultInt = BitUtils.FindFirstContiguousBitsOff((uint)array[1].InputInt0, array[1].InputInt1);
			array[2].ResultInt = BitUtils.FindFirstContiguousBitsOff((uint)array[2].InputInt0, array[2].InputInt1);
			array[3].ResultInt = BitUtils.FindFirstContiguousBitsOff((uint)array[3].InputInt0, array[3].InputInt1);
			for (int num6 = 0; num6 < 4; num6++)
			{
				Debug.Assert(array[num6].ResultInt == array[num6].ExpectedResultInt, string.Format("Error in test data {0}.", num6));
			}
		}

		// Token: 0x02000C0D RID: 3085
		private struct TestData
		{
			// Token: 0x04003D4A RID: 15690
			public byte InputByte;

			// Token: 0x04003D4B RID: 15691
			public int InputInt0;

			// Token: 0x04003D4C RID: 15692
			public int InputInt1;

			// Token: 0x04003D4D RID: 15693
			public uint InputUInt0;

			// Token: 0x04003D4E RID: 15694
			public ulong InputULong0;

			// Token: 0x04003D4F RID: 15695
			public byte ResultByte;

			// Token: 0x04003D50 RID: 15696
			public int ResultInt;

			// Token: 0x04003D51 RID: 15697
			public uint ResultUInt;

			// Token: 0x04003D52 RID: 15698
			public ulong ResultULong;

			// Token: 0x04003D53 RID: 15699
			public byte ExpectedResultByte;

			// Token: 0x04003D54 RID: 15700
			public int ExpectedResultInt;

			// Token: 0x04003D55 RID: 15701
			public uint ExpectedResultUInt;

			// Token: 0x04003D56 RID: 15702
			public ulong ExpectedResultULong;
		}
	}
}
