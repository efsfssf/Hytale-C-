using System;

namespace HytaleClient.Graphics.Gizmos.Models
{
	// Token: 0x02000AA3 RID: 2723
	public class ConeModel
	{
		// Token: 0x060055B3 RID: 21939 RVA: 0x0019772C File Offset: 0x0019592C
		public static PrimitiveModelData BuildModelData(float radius, float height, int segments)
		{
			float[] array = new float[3 * segments * 8];
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < segments; j++)
				{
					int num = (j + i * segments) * 8;
					switch (i)
					{
					case 0:
						array[num] = 0f;
						array[num + 1] = -height / 2f;
						array[num + 2] = 0f;
						break;
					case 1:
					{
						float num2 = (float)j / (float)(segments - 1) * 6.2831855f;
						array[num] = radius * (float)Math.Cos((double)num2);
						array[num + 1] = -height / 2f;
						array[num + 2] = radius * (float)Math.Sin((double)num2);
						break;
					}
					case 2:
						array[num] = 0f;
						array[num + 1] = height / 2f;
						array[num + 2] = 0f;
						break;
					}
					array[num + 3] = 0f;
					array[num + 4] = 0f;
					array[num + 5] = 0f;
					array[num + 6] = 0f;
					array[num + 7] = 0f;
				}
			}
			return new PrimitiveModelData(array, PrimitiveModelData.MakeRadialIndices(segments, 3));
		}

		// Token: 0x060055B4 RID: 21940 RVA: 0x00197860 File Offset: 0x00195A60
		public static bool[,,] BuildVoxelData(int radiusX, int height, int radiusZ)
		{
			bool[,,] array = new bool[radiusX * 2 + 1, height + 1, radiusZ * 2 + 1];
			float num = 0.41f;
			float num2 = (float)radiusX + num;
			float num3 = (float)radiusZ + num;
			for (int i = height; i >= 0; i--)
			{
				double num4 = 1.0 - (double)i / (double)height;
				double num5 = (double)num2 * num4;
				int num7;
				int num6 = -(num7 = (int)num5);
				for (int j = num6; j <= num7; j++)
				{
					double d = 1.0 - (double)(j * j) / (num5 * num5);
					double num8 = Math.Sqrt(d) * (double)num3 * num4;
					int num10;
					int num9 = -(num10 = (int)(double.IsNaN(num8) ? 0.0 : num8));
					for (int k = num9; k <= num10; k++)
					{
						array[j + radiusX, i, k + radiusZ] = true;
					}
				}
			}
			return array;
		}

		// Token: 0x060055B5 RID: 21941 RVA: 0x00197970 File Offset: 0x00195B70
		public static bool[,,] BuildInvertedVoxelData(int radiusX, int height, int radiusZ)
		{
			bool[,,] array = new bool[radiusX * 2 + 1, height + 1, radiusZ * 2 + 1];
			float num = 0.41f;
			float num2 = (float)radiusX + num;
			float num3 = (float)radiusZ + num;
			for (int i = height; i >= 0; i--)
			{
				double num4 = (double)i / (double)height;
				double num5 = (double)num2 * num4;
				double num6 = 1.0 / (num5 * num5);
				double num7 = (double)num3 * num4;
				int num9;
				int num8 = -(num9 = (int)num5);
				for (int j = num8; j <= num9; j++)
				{
					double num10 = Math.Sqrt(1.0 - (double)(j * j) * num6) * num7;
					int num12;
					int num11 = -(num12 = (int)(double.IsNaN(num10) ? 0.0 : num10));
					for (int k = num11; k <= num12; k++)
					{
						array[j + radiusX, i, k + radiusZ] = true;
					}
				}
			}
			return array;
		}
	}
}
