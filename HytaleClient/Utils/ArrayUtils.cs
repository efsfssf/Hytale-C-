using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace HytaleClient.Utils
{
	// Token: 0x020007B0 RID: 1968
	public static class ArrayUtils
	{
		// Token: 0x060032F8 RID: 13048 RVA: 0x0004C0CC File Offset: 0x0004A2CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GrowArrayIfNecessary<T>(ref T[] array, int itemCount, int growth)
		{
			bool flag = itemCount >= array.Length;
			if (flag)
			{
				Array.Resize<T>(ref array, itemCount + growth);
			}
		}

		// Token: 0x060032F9 RID: 13049 RVA: 0x0004C0F4 File Offset: 0x0004A2F4
		public static void RemoveAt<T>(ref T[] array, int index)
		{
			for (int i = index; i < array.Length - 1; i++)
			{
				array[i] = array[i + 1];
			}
			Array.Resize<T>(ref array, array.Length - 1);
		}

		// Token: 0x060032FA RID: 13050 RVA: 0x0004C138 File Offset: 0x0004A338
		public static int CompactArray<T>(ref T[] array, int startIndex, int count, ArrayUtils.IsRemovableDuringSparseCompression<T> isRemovableDuringSparseCompression) where T : struct
		{
			Debug.Assert(startIndex < array.Length);
			Debug.Assert(startIndex + count <= array.Length);
			int num = startIndex + count;
			int num2 = 0;
			for (int i = startIndex; i < num; i++)
			{
				bool flag = isRemovableDuringSparseCompression(ref array[i]);
				bool flag2 = !flag;
				if (!flag2)
				{
					for (int j = i + 1; j < num; j++)
					{
						bool flag3 = isRemovableDuringSparseCompression(ref array[j]);
						if (!flag3)
						{
							array[i++] = array[j];
							num2++;
						}
					}
					break;
				}
				num2++;
			}
			return num2;
		}

		// Token: 0x02000C0C RID: 3084
		// (Invoke) Token: 0x06006258 RID: 25176
		public delegate bool IsRemovableDuringSparseCompression<T>(ref T element) where T : struct;
	}
}
