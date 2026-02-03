using System;

namespace HytaleClient.Graphics.Gizmos.Models
{
	// Token: 0x02000AA7 RID: 2727
	public class PyramidModel
	{
		// Token: 0x060055C3 RID: 21955 RVA: 0x00198164 File Offset: 0x00196364
		public static PrimitiveModelData BuildModelData(float radius, float height, int segments)
		{
			float[] array = new float[3 * segments * 8];
			float num = (float)Math.Sqrt(2.0) - 1f;
			radius *= 10f * num / 2f - 0.5f;
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < segments; j++)
				{
					int num2 = (j + i * segments) * 8;
					switch (i)
					{
					case 0:
						array[num2] = 0f;
						array[num2 + 1] = -height / 2f;
						array[num2 + 2] = 0f;
						break;
					case 1:
					{
						float num3 = (float)j / (float)(segments - 1) * 6.2831855f + 0.7853982f;
						array[num2] = radius * (float)Math.Cos((double)num3);
						array[num2 + 1] = -height / 2f;
						array[num2 + 2] = radius * (float)Math.Sin((double)num3);
						break;
					}
					case 2:
						array[num2] = 0f;
						array[num2 + 1] = height / 2f;
						array[num2 + 2] = 0f;
						break;
					}
					array[num2 + 3] = 0f;
					array[num2 + 4] = 0f;
					array[num2 + 5] = 0f;
					array[num2 + 6] = 0f;
					array[num2 + 7] = 0f;
				}
			}
			return new PrimitiveModelData(array, PrimitiveModelData.MakeRadialIndices(segments, 3));
		}

		// Token: 0x060055C4 RID: 21956 RVA: 0x001982DC File Offset: 0x001964DC
		public static bool[,,] BuildVoxelData(int radiusX, int height, int radiusZ)
		{
			bool[,,] array = new bool[radiusX * 2 + 1, height + 1, radiusZ * 2 + 1];
			for (int i = height; i >= 0; i--)
			{
				double num = 1.0 - (double)i / (double)height;
				double num2 = (double)radiusX * num;
				int num4;
				int num3 = -(num4 = (int)num2);
				for (int j = num3; j <= num4; j++)
				{
					double num5 = (double)radiusZ * num;
					int num7;
					int num6 = -(num7 = (int)num5);
					for (int k = num6; k <= num7; k++)
					{
						array[j + radiusX, i, k + radiusZ] = true;
					}
				}
			}
			return array;
		}

		// Token: 0x060055C5 RID: 21957 RVA: 0x00198398 File Offset: 0x00196598
		public static bool[,,] BuildInvertedVoxelData(int radiusX, int height, int radiusZ)
		{
			bool[,,] array = new bool[radiusX * 2 + 1, height + 1, radiusZ * 2 + 1];
			for (int i = height; i >= 0; i--)
			{
				double num = (double)i / (double)height;
				double num2 = (double)radiusX * num;
				int num4;
				int num3 = -(num4 = (int)num2);
				for (int j = num3; j <= num4; j++)
				{
					double num5 = (double)radiusZ * num;
					int num7;
					int num6 = -(num7 = (int)num5);
					for (int k = num6; k <= num7; k++)
					{
						array[j + radiusX, i, k + radiusZ] = true;
					}
				}
			}
			return array;
		}
	}
}
