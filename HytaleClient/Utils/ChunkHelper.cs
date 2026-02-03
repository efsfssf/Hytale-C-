using System;
using System.Runtime.CompilerServices;
using HytaleClient.Math;

namespace HytaleClient.Utils
{
	// Token: 0x020007B7 RID: 1975
	internal static class ChunkHelper
	{
		// Token: 0x17000F7B RID: 3963
		// (get) Token: 0x0600332A RID: 13098 RVA: 0x0004E37C File Offset: 0x0004C57C
		// (set) Token: 0x0600332B RID: 13099 RVA: 0x0004E383 File Offset: 0x0004C583
		public static int Height { get; private set; }

		// Token: 0x17000F7C RID: 3964
		// (get) Token: 0x0600332C RID: 13100 RVA: 0x0004E38B File Offset: 0x0004C58B
		// (set) Token: 0x0600332D RID: 13101 RVA: 0x0004E392 File Offset: 0x0004C592
		public static int ChunksPerColumn { get; private set; }

		// Token: 0x0600332E RID: 13102 RVA: 0x0004E39A File Offset: 0x0004C59A
		public static void SetHeight(int height)
		{
			ChunkHelper.Height = height;
			ChunkHelper.ChunksPerColumn = ChunkHelper.Height >> 5;
		}

		// Token: 0x0600332F RID: 13103 RVA: 0x0004E3B4 File Offset: 0x0004C5B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ChunkCoordinate(int block)
		{
			return block >> 5;
		}

		// Token: 0x06003330 RID: 13104 RVA: 0x0004E3CC File Offset: 0x0004C5CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int IndexSection(int block)
		{
			return block >> 5;
		}

		// Token: 0x06003331 RID: 13105 RVA: 0x0004E3E4 File Offset: 0x0004C5E4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long IndexOfChunkColumn(int x, int z)
		{
			return (long)x << 32 | ((long)z & (long)((ulong)-1));
		}

		// Token: 0x06003332 RID: 13106 RVA: 0x0004E404 File Offset: 0x0004C604
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int XOfChunkColumnIndex(long index)
		{
			return (int)(index >> 32);
		}

		// Token: 0x06003333 RID: 13107 RVA: 0x0004E41C File Offset: 0x0004C61C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ZOfChunkColumnIndex(long index)
		{
			return (int)index;
		}

		// Token: 0x06003334 RID: 13108 RVA: 0x0004E430 File Offset: 0x0004C630
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int IndexOfWorldBlockInChunk(int x, int y, int z)
		{
			return (y & 31) << 10 | (z & 31) << 5 | (x & 31);
		}

		// Token: 0x06003335 RID: 13109 RVA: 0x0004E458 File Offset: 0x0004C658
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int IndexOfBlockInChunk(int x, int y, int z)
		{
			return (y * 32 + z) * 32 + x;
		}

		// Token: 0x06003336 RID: 13110 RVA: 0x0004E478 File Offset: 0x0004C678
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int IndexOfBlockInBorderedChunk(int x, int y, int z)
		{
			return (y * 34 + z) * 34 + x;
		}

		// Token: 0x06003337 RID: 13111 RVA: 0x0004E498 File Offset: 0x0004C698
		public static int IndexOfBlockInBorderedChunk(int indexInChunk, int chunkOffsetX, int chunkOffsetY, int chunkOffsetZ)
		{
			int num = chunkOffsetX * 32 + indexInChunk % 32;
			bool flag = num < -1 || num >= 33;
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				int num2 = chunkOffsetY * 32 + indexInChunk / 32 / 32;
				bool flag2 = num2 < -1 || num2 >= 33;
				if (flag2)
				{
					result = -1;
				}
				else
				{
					int num3 = chunkOffsetZ * 32 + indexInChunk / 32 % 32;
					bool flag3 = num3 < -1 || num3 >= 33;
					if (flag3)
					{
						result = -1;
					}
					else
					{
						result = ChunkHelper.IndexOfBlockInBorderedChunk(num + 1, num2 + 1, num3 + 1);
					}
				}
			}
			return result;
		}

		// Token: 0x06003338 RID: 13112 RVA: 0x0004E52C File Offset: 0x0004C72C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int IndexInChunkColumn(int x, int z)
		{
			return z * 32 + x;
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x0004E544 File Offset: 0x0004C744
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int IndexInBorderedChunkColumn(int x, int z)
		{
			return z * 34 + x;
		}

		// Token: 0x0600333A RID: 13114 RVA: 0x0004E55C File Offset: 0x0004C75C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 WorldToChunk(Vector3 worldPos)
		{
			int num = (int)Math.Floor((double)worldPos.X);
			int num2 = (int)Math.Floor((double)worldPos.Y);
			int num3 = (int)Math.Floor((double)worldPos.Z);
			int num4 = num >> 5;
			int num5 = num2 >> 5;
			int num6 = num3 >> 5;
			int num7 = num - num4 * 32;
			int num8 = num2 - num5 * 32;
			int num9 = num3 - num6 * 32;
			return new Vector3((float)num7, (float)num8, (float)num9);
		}

		// Token: 0x0600333B RID: 13115 RVA: 0x0004E5D0 File Offset: 0x0004C7D0
		public static void GetEnvironmentId(ushort[] environmentsColumn, int worldY, ushort[] tracker, int trackerIndex)
		{
			int num = 1;
			ushort num2 = tracker[trackerIndex + 1];
			int num3 = 0;
			ushort num4 = num2;
			while ((int)num4 < environmentsColumn.Length)
			{
				bool flag = worldY < (int)environmentsColumn[(int)num4];
				if (flag)
				{
					break;
				}
				num = (int)(num4 + 1);
				num3 = (int)(((int)(num4 + 2) < environmentsColumn.Length) ? environmentsColumn[(int)(num4 + 2)] : ushort.MaxValue);
				num2 = num4;
				num4 += 2;
			}
			bool flag2 = (int)environmentsColumn[(int)num2] > worldY + 1;
			if (flag2)
			{
				tracker[trackerIndex] = (ushort)num3;
				tracker[trackerIndex + 1] = num2 + 2;
			}
			tracker[trackerIndex + 2] = environmentsColumn[num];
		}

		// Token: 0x0600333C RID: 13116 RVA: 0x0004E650 File Offset: 0x0004C850
		public static ushort GetEnvironmentId(ushort[][] environments, int chunkX, int chunkZ, int worldY)
		{
			ushort[] array = environments[(chunkZ << 5) + chunkX];
			int num = 1;
			for (int i = array.Length - 2; i >= 0; i -= 2)
			{
				bool flag = (int)array[i] <= worldY;
				if (flag)
				{
					num = i + 1;
					break;
				}
			}
			return array[num];
		}

		// Token: 0x040016F9 RID: 5881
		public const int Bits = 5;

		// Token: 0x040016FA RID: 5882
		public const int Bits2 = 10;

		// Token: 0x040016FB RID: 5883
		public const int Size = 32;

		// Token: 0x040016FC RID: 5884
		public const int BorderedSize = 34;

		// Token: 0x040016FD RID: 5885
		public const int SizeMask = 31;

		// Token: 0x040016FE RID: 5886
		public const int BlocksCount = 32768;

		// Token: 0x040016FF RID: 5887
		public const int BorderedBlocksCount = 39304;
	}
}
