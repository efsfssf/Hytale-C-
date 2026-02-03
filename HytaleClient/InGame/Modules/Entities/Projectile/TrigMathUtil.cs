using System;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x02000967 RID: 2407
	public static class TrigMathUtil
	{
		// Token: 0x06004B31 RID: 19249 RVA: 0x001350E0 File Offset: 0x001332E0
		public static float Sin(float radians)
		{
			return TrigMathUtil.Riven.Sin(radians);
		}

		// Token: 0x06004B32 RID: 19250 RVA: 0x001350F8 File Offset: 0x001332F8
		public static float Cos(float radians)
		{
			return TrigMathUtil.Riven.Cos(radians);
		}

		// Token: 0x06004B33 RID: 19251 RVA: 0x00135110 File Offset: 0x00133310
		public static float Sin(double radians)
		{
			return TrigMathUtil.Riven.Sin((float)radians);
		}

		// Token: 0x06004B34 RID: 19252 RVA: 0x0013512C File Offset: 0x0013332C
		public static float Cos(double radians)
		{
			return TrigMathUtil.Riven.Cos((float)radians);
		}

		// Token: 0x06004B35 RID: 19253 RVA: 0x00135148 File Offset: 0x00133348
		public static float Atan2(float y, float x)
		{
			return TrigMathUtil.Icecore.Atan2(y, x);
		}

		// Token: 0x06004B36 RID: 19254 RVA: 0x00135164 File Offset: 0x00133364
		public static float Atan2(double y, double x)
		{
			return TrigMathUtil.Icecore.Atan2((float)y, (float)x);
		}

		// Token: 0x06004B37 RID: 19255 RVA: 0x00135180 File Offset: 0x00133380
		public static float Atan(double d)
		{
			return (float)Math.Atan(d);
		}

		// Token: 0x06004B38 RID: 19256 RVA: 0x0013519C File Offset: 0x0013339C
		public static float Asin(double d)
		{
			return (float)Math.Asin(d);
		}

		// Token: 0x040026DE RID: 9950
		public const float Pi = 3.1415927f;

		// Token: 0x040026DF RID: 9951
		public const float PiHalf = 1.5707964f;

		// Token: 0x040026E0 RID: 9952
		public const float PiQuarter = 0.7853982f;

		// Token: 0x040026E1 RID: 9953
		public const float Pi2 = 6.2831855f;

		// Token: 0x040026E2 RID: 9954
		public const float Pi4 = 12.566371f;

		// Token: 0x040026E3 RID: 9955
		public const float RadToDeg = 57.295776f;

		// Token: 0x040026E4 RID: 9956
		public const float DegToRad = 0.017453292f;

		// Token: 0x02000E52 RID: 3666
		private static class Riven
		{
			// Token: 0x0600676D RID: 26477 RVA: 0x00217A5C File Offset: 0x00215C5C
			static Riven()
			{
				TrigMathUtil.Riven.RadToIndex = (float)TrigMathUtil.Riven.SinCount / TrigMathUtil.Riven.RadFull;
				TrigMathUtil.Riven.DegToIndex = (float)TrigMathUtil.Riven.SinCount / TrigMathUtil.Riven.DegFull;
				TrigMathUtil.Riven.SIN = new float[TrigMathUtil.Riven.SinCount];
				TrigMathUtil.Riven.COS = new float[TrigMathUtil.Riven.SinCount];
				for (int i = 0; i < TrigMathUtil.Riven.SinCount; i++)
				{
					TrigMathUtil.Riven.SIN[i] = (float)Math.Sin((double)(((float)i + 0.5f) / (float)TrigMathUtil.Riven.SinCount * TrigMathUtil.Riven.RadFull));
					TrigMathUtil.Riven.COS[i] = (float)Math.Cos((double)(((float)i + 0.5f) / (float)TrigMathUtil.Riven.SinCount * TrigMathUtil.Riven.RadFull));
				}
				for (int j = 0; j < 360; j += 90)
				{
					TrigMathUtil.Riven.SIN[(int)((float)j * TrigMathUtil.Riven.DegToIndex) & TrigMathUtil.Riven.SinMask] = (float)Math.Sin((double)j * 3.141592653589793 / 180.0);
					TrigMathUtil.Riven.COS[(int)((float)j * TrigMathUtil.Riven.DegToIndex) & TrigMathUtil.Riven.SinMask] = (float)Math.Cos((double)j * 3.141592653589793 / 180.0);
				}
			}

			// Token: 0x0600676E RID: 26478 RVA: 0x00217BB8 File Offset: 0x00215DB8
			public static float Sin(float rad)
			{
				return TrigMathUtil.Riven.SIN[(int)(rad * TrigMathUtil.Riven.RadToIndex) & TrigMathUtil.Riven.SinMask];
			}

			// Token: 0x0600676F RID: 26479 RVA: 0x00217BE0 File Offset: 0x00215DE0
			public static float Cos(float rad)
			{
				return TrigMathUtil.Riven.COS[(int)(rad * TrigMathUtil.Riven.RadToIndex) & TrigMathUtil.Riven.SinMask];
			}

			// Token: 0x040045FD RID: 17917
			private static readonly int SinBits = 12;

			// Token: 0x040045FE RID: 17918
			private static readonly int SinMask = ~(-1 << TrigMathUtil.Riven.SinBits);

			// Token: 0x040045FF RID: 17919
			private static readonly int SinCount = TrigMathUtil.Riven.SinMask + 1;

			// Token: 0x04004600 RID: 17920
			private static readonly float RadFull = 6.2831855f;

			// Token: 0x04004601 RID: 17921
			private static readonly float RadToIndex;

			// Token: 0x04004602 RID: 17922
			private static readonly float DegFull = 360f;

			// Token: 0x04004603 RID: 17923
			private static readonly float DegToIndex;

			// Token: 0x04004604 RID: 17924
			private static readonly float[] SIN;

			// Token: 0x04004605 RID: 17925
			private static readonly float[] COS;
		}

		// Token: 0x02000E53 RID: 3667
		private static class Icecore
		{
			// Token: 0x06006770 RID: 26480 RVA: 0x00217C08 File Offset: 0x00215E08
			static Icecore()
			{
				for (int i = 0; i <= 100000; i++)
				{
					double num = (double)i / 100000.0;
					double num2 = 1.0;
					double y = num2 * num;
					float num3 = (float)Math.Atan2(y, num2);
					TrigMathUtil.Icecore.ATAN2[i] = num3;
				}
			}

			// Token: 0x06006771 RID: 26481 RVA: 0x00217C70 File Offset: 0x00215E70
			public static float Atan2(float y, float x)
			{
				bool flag = y < 0f;
				float result;
				if (flag)
				{
					bool flag2 = x < 0f;
					if (flag2)
					{
						bool flag3 = y < x;
						if (flag3)
						{
							result = -TrigMathUtil.Icecore.ATAN2[(int)(x / y * 100000f)] - 1.5707964f;
						}
						else
						{
							result = TrigMathUtil.Icecore.ATAN2[(int)(y / x * 100000f)] - 3.1415927f;
						}
					}
					else
					{
						y = -y;
						bool flag4 = y > x;
						if (flag4)
						{
							result = TrigMathUtil.Icecore.ATAN2[(int)(x / y * 100000f)] - 1.5707964f;
						}
						else
						{
							result = -TrigMathUtil.Icecore.ATAN2[(int)(y / x * 100000f)];
						}
					}
				}
				else
				{
					bool flag5 = x < 0f;
					if (flag5)
					{
						x = -x;
						bool flag6 = y > x;
						if (flag6)
						{
							result = TrigMathUtil.Icecore.ATAN2[(int)(x / y * 100000f)] + 1.5707964f;
						}
						else
						{
							result = -TrigMathUtil.Icecore.ATAN2[(int)(y / x * 100000f)] + 3.1415927f;
						}
					}
					else
					{
						bool flag7 = y > x;
						if (flag7)
						{
							result = -TrigMathUtil.Icecore.ATAN2[(int)(x / y * 100000f)] + 1.5707964f;
						}
						else
						{
							result = TrigMathUtil.Icecore.ATAN2[(int)(y / x * 100000f)];
						}
					}
				}
				return result;
			}

			// Token: 0x04004606 RID: 17926
			private const int SizeAc = 100000;

			// Token: 0x04004607 RID: 17927
			private const int SizeAr = 100001;

			// Token: 0x04004608 RID: 17928
			private static readonly float[] ATAN2 = new float[100001];
		}
	}
}
