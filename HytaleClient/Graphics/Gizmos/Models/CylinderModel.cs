using System;

namespace HytaleClient.Graphics.Gizmos.Models
{
	// Token: 0x02000AA5 RID: 2725
	public class CylinderModel
	{
		// Token: 0x060055BB RID: 21947 RVA: 0x00197BFC File Offset: 0x00195DFC
		public static PrimitiveModelData BuildModelData(float radius, float height, int segments)
		{
			float[] array = new float[4 * segments * 8];
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < segments; j++)
				{
					int num = (j + i * segments) * 8;
					float num2 = (float)j / (float)(segments - 1) * 6.2831855f;
					switch (i)
					{
					case 0:
						array[num] = 0f;
						array[num + 1] = -height / 2f;
						array[num + 2] = 0f;
						break;
					case 1:
						array[num] = (float)((double)radius * Math.Cos((double)num2));
						array[num + 1] = -height / 2f;
						array[num + 2] = (float)((double)radius * Math.Sin((double)num2));
						break;
					case 2:
						array[num] = (float)((double)radius * Math.Cos((double)num2));
						array[num + 1] = height / 2f;
						array[num + 2] = (float)((double)radius * Math.Sin((double)num2));
						break;
					case 3:
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
			return new PrimitiveModelData(array, PrimitiveModelData.MakeRadialIndices(segments, 4));
		}

		// Token: 0x060055BC RID: 21948 RVA: 0x00197D68 File Offset: 0x00195F68
		public static PrimitiveModelData BuildHollowModelData(float radius, float height, int segments)
		{
			float[] array = new float[2 * segments * 8];
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < segments; j++)
				{
					int num = (j + i * segments) * 8;
					float num2 = (float)j / (float)(segments - 1) * 6.2831855f;
					array[num] = (float)((double)radius * Math.Cos((double)num2));
					array[num + 1] = ((i == 0) ? (-height / 2f) : (height / 2f));
					array[num + 2] = (float)((double)radius * Math.Sin((double)num2));
					array[num + 3] = 0f;
					array[num + 4] = 0f;
					array[num + 5] = 0f;
					array[num + 6] = 0f;
					array[num + 7] = 0f;
				}
			}
			return new PrimitiveModelData(array, PrimitiveModelData.MakeRadialIndices(segments, 2));
		}

		// Token: 0x060055BD RID: 21949 RVA: 0x00197E4C File Offset: 0x0019604C
		public static bool[,,] BuildVoxelData(int radiusX, int height, int radiusZ)
		{
			bool[,,] array = new bool[radiusX * 2 + 1, height + 1, radiusZ * 2 + 1];
			float num = 0.41f;
			float num2 = (float)radiusX + num;
			float num3 = (float)radiusZ + num;
			double num4 = 1.0 / (double)(num2 * num2);
			for (int i = -radiusX; i <= radiusX; i++)
			{
				double d = 1.0 - (double)(i * i) * num4;
				double num5 = Math.Sqrt(d) * (double)num3;
				int num7;
				int num6 = -(num7 = (int)num5);
				for (int j = num6; j <= num7; j++)
				{
					for (int k = height; k >= 0; k--)
					{
						array[i + radiusX, k, j + radiusZ] = true;
					}
				}
			}
			return array;
		}
	}
}
