using System;

namespace HytaleClient.Graphics.Gizmos.Models
{
	// Token: 0x02000AA8 RID: 2728
	public class SphereModel
	{
		// Token: 0x060055C7 RID: 21959 RVA: 0x00198454 File Offset: 0x00196654
		public static PrimitiveModelData BuildModelData(float radius, float height, int segments, int rings, float depth = 0f)
		{
			float[] array = new float[rings * segments * 8];
			bool flag = depth == 0f;
			if (flag)
			{
				depth = radius;
			}
			for (int i = 0; i < rings; i++)
			{
				float num = (float)i / (float)(rings - 1) * 3.1415927f;
				for (int j = 0; j < segments; j++)
				{
					int num2 = (j + i * segments) * 8;
					float num3 = (1f - (float)j / (float)(segments - 1)) * 6.2831855f;
					array[num2] = (float)((double)radius * Math.Sin((double)num) * Math.Cos((double)num3));
					array[num2 + 1] = (float)((double)(height / 2f) * Math.Cos((double)num));
					array[num2 + 2] = (float)((double)depth * Math.Sin((double)num) * Math.Sin((double)num3));
					array[num2 + 3] = 0f;
					array[num2 + 4] = 0f;
					array[num2 + 5] = 0f;
					array[num2 + 6] = 0f;
					array[num2 + 7] = 0f;
				}
			}
			return new PrimitiveModelData(array, PrimitiveModelData.MakeRadialIndices(segments, rings));
		}

		// Token: 0x060055C8 RID: 21960 RVA: 0x00198578 File Offset: 0x00196778
		public static bool[,,] BuildVoxelData(int radiusX, int radiusY, int radiusZ)
		{
			bool[,,] array = new bool[radiusX * 2 + 1, radiusY * 2 + 1, radiusZ * 2 + 1];
			float num = 0.41f;
			float num2 = (float)radiusX + num;
			float num3 = (float)radiusY + num;
			float num4 = (float)radiusZ + num;
			double num5 = 1.0 / (double)(num2 * num2);
			double num6 = 1.0 / (double)(num3 * num3);
			for (int i = -radiusX; i <= radiusX; i++)
			{
				double num7 = 1.0 - (double)(i * i) * num5;
				double num8 = Math.Sqrt(num7) * (double)num3;
				int num10;
				int num9 = -(num10 = (int)num8);
				for (int j = num10; j >= num9; j--)
				{
					double num11 = Math.Sqrt(num7 - (double)(j * j) * num6) * (double)num4;
					int num13;
					int num12 = -(num13 = (int)num11);
					for (int k = num12; k <= num13; k++)
					{
						array[i + radiusX, j + radiusY, k + radiusZ] = true;
					}
				}
			}
			return array;
		}
	}
}
