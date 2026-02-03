using System;
using HytaleClient.Math;

namespace HytaleClient.Utils
{
	// Token: 0x020007B1 RID: 1969
	internal class BilinearFilter
	{
		// Token: 0x060032FB RID: 13051 RVA: 0x0004C1F4 File Offset: 0x0004A3F4
		private static float CubicHermite(float A, float B, float C, float D, float t)
		{
			float num = -A / 2f + 3f * B / 2f - 3f * C / 2f + D / 2f;
			float num2 = A - 5f * B / 2f + 2f * C - D / 2f;
			float num3 = -A / 2f + C / 2f;
			return num * t * t * t + num2 * t * t + num3 * t + B;
		}

		// Token: 0x060032FC RID: 13052 RVA: 0x0004C284 File Offset: 0x0004A484
		public static byte[] ApplyFilter(byte[] pixels, int originalSize, int scaledSize)
		{
			byte[] array = new byte[scaledSize * scaledSize * 4];
			float num = (float)(originalSize - 1) / (float)scaledSize;
			int num2 = 0;
			byte[][] array2 = new byte[16][];
			for (int i = 0; i < 16; i++)
			{
				array2[i] = new byte[4];
			}
			for (int j = 0; j < scaledSize; j++)
			{
				float num3 = (float)(j / (scaledSize - 1));
				for (int k = 0; k < scaledSize; k++)
				{
					float num4 = (float)(k / (scaledSize - 1));
					int num5 = (int)(num * (float)k);
					int num6 = (int)(num * (float)j);
					int index = (num6 * originalSize + num5) * 4;
					float num7 = num4 * (float)originalSize - 0.5f;
					float t = num7 - (float)Math.Floor((double)num7);
					float num8 = num3 * (float)originalSize - 0.5f;
					float t2 = num8 - (float)Math.Floor((double)num8);
					BilinearFilter.GetColorByte(pixels, index, originalSize, -1, -1, ref array2[0]);
					BilinearFilter.GetColorByte(pixels, index, originalSize, 0, -1, ref array2[1]);
					BilinearFilter.GetColorByte(pixels, index, originalSize, 1, -1, ref array2[2]);
					BilinearFilter.GetColorByte(pixels, index, originalSize, 2, -1, ref array2[3]);
					BilinearFilter.GetColorByte(pixels, index, originalSize, -1, 0, ref array2[4]);
					BilinearFilter.GetColorByte(pixels, index, originalSize, 0, 0, ref array2[5]);
					BilinearFilter.GetColorByte(pixels, index, originalSize, 1, 0, ref array2[6]);
					BilinearFilter.GetColorByte(pixels, index, originalSize, 2, 0, ref array2[7]);
					BilinearFilter.GetColorByte(pixels, index, originalSize, -1, 1, ref array2[8]);
					BilinearFilter.GetColorByte(pixels, index, originalSize, 0, 1, ref array2[9]);
					BilinearFilter.GetColorByte(pixels, index, originalSize, 1, 1, ref array2[10]);
					BilinearFilter.GetColorByte(pixels, index, originalSize, 2, 1, ref array2[11]);
					BilinearFilter.GetColorByte(pixels, index, originalSize, -1, 2, ref array2[12]);
					BilinearFilter.GetColorByte(pixels, index, originalSize, 0, 2, ref array2[13]);
					BilinearFilter.GetColorByte(pixels, index, originalSize, 1, 2, ref array2[14]);
					BilinearFilter.GetColorByte(pixels, index, originalSize, 2, 2, ref array2[15]);
					for (int l = 0; l < 4; l++)
					{
						float a = BilinearFilter.CubicHermite((float)array2[0][l], (float)array2[1][l], (float)array2[2][l], (float)array2[3][l], t);
						float b = BilinearFilter.CubicHermite((float)array2[4][l], (float)array2[5][l], (float)array2[6][l], (float)array2[7][l], t);
						float c = BilinearFilter.CubicHermite((float)array2[8][l], (float)array2[9][l], (float)array2[10][l], (float)array2[11][l], t);
						float d = BilinearFilter.CubicHermite((float)array2[12][l], (float)array2[13][l], (float)array2[14][l], (float)array2[15][l], t);
						float num9 = BilinearFilter.CubicHermite(a, b, c, d, t2);
						num9 = MathHelper.Clamp(num9, 0f, 255f);
						array[num2 + l] = (byte)num9;
					}
					num2 += 4;
				}
			}
			return array;
		}

		// Token: 0x060032FD RID: 13053 RVA: 0x0004C598 File Offset: 0x0004A798
		private static void GetColorByte(byte[] pixels, int index, int originalSize, int x, int y, ref byte[] colorBytes)
		{
			int num = index + originalSize * 4 * y + 4 * x;
			int max = originalSize * originalSize * 4 - 1;
			colorBytes[0] = pixels[MathHelper.Clamp(num, 0, max)];
			colorBytes[1] = pixels[MathHelper.Clamp(num + 1, 0, max)];
			colorBytes[2] = pixels[MathHelper.Clamp(num + 2, 0, max)];
			colorBytes[3] = pixels[MathHelper.Clamp(num + 3, 0, max)];
		}
	}
}
