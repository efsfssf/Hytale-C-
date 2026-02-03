using System;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x02000966 RID: 2406
	public static class ServerBlockIterator
	{
		// Token: 0x06004B2B RID: 19243 RVA: 0x00134A80 File Offset: 0x00132C80
		public static bool Iterate<T>(float sx, float sy, float sz, float dx, float dy, float dz, float maxDistance, ServerBlockIterator.BlockIteratorProcedurePlus1<T> procedure, T obj1)
		{
			ServerBlockIterator.CheckParameters(sx, sy, sz, dx, dy, dz);
			return ServerBlockIterator.Iterate0<T>(sx, sy, sz, dx, dy, dz, maxDistance, procedure, obj1);
		}

		// Token: 0x06004B2C RID: 19244 RVA: 0x00134AB4 File Offset: 0x00132CB4
		private static bool Iterate0<T>(float sx, float sy, float sz, float dx, float dy, float dz, float maxDistance, ServerBlockIterator.BlockIteratorProcedurePlus1<T> procedure, T obj1)
		{
			maxDistance /= (float)Math.Sqrt((double)(dx * dx + dy * dy + dz * dz));
			int num = ServerBlockIterator.FastMath.FastFloor(sx);
			int num2 = ServerBlockIterator.FastMath.FastFloor(sy);
			int num3 = ServerBlockIterator.FastMath.FastFloor(sz);
			float num4 = sx - (float)num;
			float num5 = sy - (float)num2;
			float num6 = sz - (float)num3;
			float num7 = 0f;
			while (num7 <= maxDistance)
			{
				float num8 = ServerBlockIterator.Intersection(num4, num5, num6, dx, dy, dz);
				float num9 = num4 + num8 * dx;
				float num10 = num5 + num8 * dy;
				float num11 = num6 + num8 * dz;
				bool flag = !procedure(num, num2, num3, num4, num5, num6, num9, num10, num11, obj1);
				if (flag)
				{
					return false;
				}
				bool flag2 = dx < 0f && ServerBlockIterator.FastMath.SEq(num9, 0f);
				if (flag2)
				{
					num9 += 1f;
					num--;
				}
				else
				{
					bool flag3 = dx > 0f && ServerBlockIterator.FastMath.GEq(num9, 1f);
					if (flag3)
					{
						num9 -= 1f;
						num++;
					}
				}
				bool flag4 = dy < 0f && ServerBlockIterator.FastMath.SEq(num10, 0f);
				if (flag4)
				{
					num10 += 1f;
					num2--;
				}
				else
				{
					bool flag5 = dy > 0f && ServerBlockIterator.FastMath.GEq(num10, 1f);
					if (flag5)
					{
						num10 -= 1f;
						num2++;
					}
				}
				bool flag6 = dz < 0f && ServerBlockIterator.FastMath.SEq(num11, 0f);
				if (flag6)
				{
					num11 += 1f;
					num3--;
				}
				else
				{
					bool flag7 = dz > 0f && ServerBlockIterator.FastMath.GEq(num11, 1f);
					if (flag7)
					{
						num11 -= 1f;
						num3++;
					}
				}
				num7 += num8;
				num4 = num9;
				num5 = num10;
				num6 = num11;
			}
			return true;
		}

		// Token: 0x06004B2D RID: 19245 RVA: 0x00134CAC File Offset: 0x00132EAC
		private static void CheckParameters(float sx, float sy, float sz, float dx, float dy, float dz)
		{
			bool flag = ServerBlockIterator.IsNonValidNumber(sx);
			if (flag)
			{
				throw new ArgumentException("sx is a non-valid number! Given: " + sx.ToString());
			}
			bool flag2 = ServerBlockIterator.IsNonValidNumber(sy);
			if (flag2)
			{
				throw new ArgumentException("sy is a non-valid number! Given: " + sy.ToString());
			}
			bool flag3 = ServerBlockIterator.IsNonValidNumber(sz);
			if (flag3)
			{
				throw new ArgumentException("sz is a non-valid number! Given: " + sz.ToString());
			}
			bool flag4 = ServerBlockIterator.IsNonValidNumber(dx);
			if (flag4)
			{
				throw new ArgumentException("dx is a non-valid number! Given: " + dx.ToString());
			}
			bool flag5 = ServerBlockIterator.IsNonValidNumber(dy);
			if (flag5)
			{
				throw new ArgumentException("dy is a non-valid number! Given: " + dy.ToString());
			}
			bool flag6 = ServerBlockIterator.IsNonValidNumber(dz);
			if (flag6)
			{
				throw new ArgumentException("dz is a non-valid number! Given: " + dz.ToString());
			}
			bool flag7 = ServerBlockIterator.IsZeroDirection(dx, dy, dz);
			if (flag7)
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"Direction is ZERO! Given: (",
					dx.ToString(),
					", ",
					dy.ToString(),
					", ",
					dz.ToString(),
					")"
				}));
			}
		}

		// Token: 0x06004B2E RID: 19246 RVA: 0x00134DE8 File Offset: 0x00132FE8
		public static bool IsNonValidNumber(float d)
		{
			return float.IsNaN(d) || float.IsInfinity(d);
		}

		// Token: 0x06004B2F RID: 19247 RVA: 0x00134E0C File Offset: 0x0013300C
		public static bool IsZeroDirection(float dx, float dy, float dz)
		{
			return ServerBlockIterator.FastMath.Eq(dx, 0f) && ServerBlockIterator.FastMath.Eq(dy, 0f) && ServerBlockIterator.FastMath.Eq(dz, 0f);
		}

		// Token: 0x06004B30 RID: 19248 RVA: 0x00134E48 File Offset: 0x00133048
		private static float Intersection(float px, float py, float pz, float dx, float dy, float dz)
		{
			float num = 0f;
			bool flag = dx < 0f;
			if (flag)
			{
				float num2 = -px / dx;
				float a = pz + dz * num2;
				float a2 = py + dy * num2;
				bool flag2 = num2 > num && ServerBlockIterator.FastMath.GEq(a, 0f) && ServerBlockIterator.FastMath.SEq(a, 1f) && ServerBlockIterator.FastMath.GEq(a2, 0f) && ServerBlockIterator.FastMath.SEq(a2, 1f);
				if (flag2)
				{
					num = num2;
				}
			}
			else
			{
				bool flag3 = dx > 0f;
				if (flag3)
				{
					float num2 = (1f - px) / dx;
					float a = pz + dz * num2;
					float a2 = py + dy * num2;
					bool flag4 = num2 > num && ServerBlockIterator.FastMath.GEq(a, 0f) && ServerBlockIterator.FastMath.SEq(a, 1f) && ServerBlockIterator.FastMath.GEq(a2, 0f) && ServerBlockIterator.FastMath.SEq(a2, 1f);
					if (flag4)
					{
						num = num2;
					}
				}
			}
			bool flag5 = dy < 0f;
			if (flag5)
			{
				float num2 = -py / dy;
				float a = px + dx * num2;
				float a2 = pz + dz * num2;
				bool flag6 = num2 > num && ServerBlockIterator.FastMath.GEq(a, 0f) && ServerBlockIterator.FastMath.SEq(a, 1f) && ServerBlockIterator.FastMath.GEq(a2, 0f) && ServerBlockIterator.FastMath.SEq(a2, 1f);
				if (flag6)
				{
					num = num2;
				}
			}
			else
			{
				bool flag7 = dy > 0f;
				if (flag7)
				{
					float num2 = (1f - py) / dy;
					float a = px + dx * num2;
					float a2 = pz + dz * num2;
					bool flag8 = num2 > num && ServerBlockIterator.FastMath.GEq(a, 0f) && ServerBlockIterator.FastMath.SEq(a, 1f) && ServerBlockIterator.FastMath.GEq(a2, 0f) && ServerBlockIterator.FastMath.SEq(a2, 1f);
					if (flag8)
					{
						num = num2;
					}
				}
			}
			bool flag9 = dz < 0f;
			if (flag9)
			{
				float num2 = -pz / dz;
				float a = px + dx * num2;
				float a2 = py + dy * num2;
				bool flag10 = num2 > num && ServerBlockIterator.FastMath.GEq(a, 0f) && ServerBlockIterator.FastMath.SEq(a, 1f) && ServerBlockIterator.FastMath.GEq(a2, 0f) && ServerBlockIterator.FastMath.SEq(a2, 1f);
				if (flag10)
				{
					num = num2;
				}
			}
			else
			{
				bool flag11 = dz > 0f;
				if (flag11)
				{
					float num2 = (1f - pz) / dz;
					float a = px + dx * num2;
					float a2 = py + dy * num2;
					bool flag12 = num2 > num && ServerBlockIterator.FastMath.GEq(a, 0f) && ServerBlockIterator.FastMath.SEq(a, 1f) && ServerBlockIterator.FastMath.GEq(a2, 0f) && ServerBlockIterator.FastMath.SEq(a2, 1f);
					if (flag12)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		// Token: 0x02000E50 RID: 3664
		// (Invoke) Token: 0x06006765 RID: 26469
		public delegate bool BlockIteratorProcedurePlus1<in T>(int x, int y, int z, float px, float py, float pz, float qx, float qy, float qz, T obj1);

		// Token: 0x02000E51 RID: 3665
		private static class FastMath
		{
			// Token: 0x06006768 RID: 26472 RVA: 0x00217964 File Offset: 0x00215B64
			public static bool Eq(float a, float b)
			{
				return ServerBlockIterator.FastMath.Abs(a - b) < 1.0000000036274937E-15;
			}

			// Token: 0x06006769 RID: 26473 RVA: 0x0021798C File Offset: 0x00215B8C
			public static bool SEq(float a, float b)
			{
				return a <= b + 1E-15f;
			}

			// Token: 0x0600676A RID: 26474 RVA: 0x002179AC File Offset: 0x00215BAC
			public static bool GEq(float a, float b)
			{
				return a >= b - 1E-15f;
			}

			// Token: 0x0600676B RID: 26475 RVA: 0x002179CC File Offset: 0x00215BCC
			public static double Abs(float x)
			{
				return (double)(((double)x < 0.0) ? (-(double)x) : x);
			}

			// Token: 0x0600676C RID: 26476 RVA: 0x002179F4 File Offset: 0x00215BF4
			public static int FastFloor(float x)
			{
				bool flag = x >= 4.5035996E+15f || x <= -4.5035996E+15f;
				int result;
				if (flag)
				{
					result = (int)x;
				}
				else
				{
					int num = (int)x;
					bool flag2 = x < 0f && (float)num != x;
					if (flag2)
					{
						num--;
					}
					bool flag3 = num == 0;
					if (flag3)
					{
						result = (int)(x * (float)num);
					}
					else
					{
						result = num;
					}
				}
				return result;
			}

			// Token: 0x040045FB RID: 17915
			public const float TwoPower52 = 4.5035996E+15f;

			// Token: 0x040045FC RID: 17916
			public const float RoundingError = 1E-15f;
		}
	}
}
