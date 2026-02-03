using System;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x02000956 RID: 2390
	internal class CollisionMath
	{
		// Token: 0x06004ABA RID: 19130 RVA: 0x001313D8 File Offset: 0x0012F5D8
		public static bool IsDisjoint(int code)
		{
			return code == 0;
		}

		// Token: 0x06004ABB RID: 19131 RVA: 0x001313F0 File Offset: 0x0012F5F0
		public static bool IsOverlapping(int code)
		{
			return code == 56;
		}

		// Token: 0x06004ABC RID: 19132 RVA: 0x00131408 File Offset: 0x0012F608
		public static bool IsTouching(int code)
		{
			return (code & 7) != 0;
		}

		// Token: 0x06004ABD RID: 19133 RVA: 0x00131420 File Offset: 0x0012F620
		public static bool IntersectSweptAABBs(Vector3 posP, Vector3 vP, BoundingBox p, Vector3 posQ, BoundingBox q, ref Vector2 minMax)
		{
			return CollisionMath.IntersectSweptAABBs(posP, vP, p, posQ.X, posQ.Y, posQ.Z, q, ref minMax);
		}

		// Token: 0x06004ABE RID: 19134 RVA: 0x00131450 File Offset: 0x0012F650
		public static bool IntersectSweptAABBs(Vector3 posP, Vector3 vP, BoundingBox p, float qx, float qy, float qz, BoundingBox q, ref Vector2 minMax)
		{
			return CollisionMath.IntersectVectorAABB(posP, vP, qx, qy, qz, q.MinkowskiSum(p), ref minMax);
		}

		// Token: 0x06004ABF RID: 19135 RVA: 0x00131478 File Offset: 0x0012F678
		public static bool IntersectVectorAABB(Vector3 pos, Vector3 vec, float x, float y, float z, BoundingBox box, ref Vector2 minMax)
		{
			return CollisionMath.IntersectRayAABB(pos, vec, x, y, z, box, ref minMax) && minMax.X <= 1f;
		}

		// Token: 0x06004AC0 RID: 19136 RVA: 0x001314B0 File Offset: 0x0012F6B0
		public static bool IntersectRayAABB(Vector3 pos, Vector3 ray, float x, float y, float z, BoundingBox box, ref Vector2 minMax)
		{
			minMax.X = 0f;
			minMax.Y = float.MaxValue;
			Vector3 min = box.Min;
			Vector3 max = box.Max;
			return CollisionMath.Intersect1D(pos.X, ray.X, x + min.X, x + max.X, ref minMax) && CollisionMath.Intersect1D(pos.Y, ray.Y, y + min.Y, y + max.Y, ref minMax) && CollisionMath.Intersect1D(pos.Z, ray.Z, z + min.Z, z + max.Z, ref minMax) && minMax.X >= 0f;
		}

		// Token: 0x06004AC1 RID: 19137 RVA: 0x00131570 File Offset: 0x0012F770
		public static bool Intersect1D(float p, float s, float min, float max, ref Vector2 minMax)
		{
			bool flag = Math.Abs(s) < 1E-05f;
			bool result;
			if (flag)
			{
				result = (p >= min && p <= max);
			}
			else
			{
				float num = (min - p) / s;
				float num2 = (max - p) / s;
				bool flag2 = num2 >= num;
				if (flag2)
				{
					bool flag3 = num > minMax.X;
					if (flag3)
					{
						minMax.X = num;
					}
					bool flag4 = num2 < minMax.Y;
					if (flag4)
					{
						minMax.Y = num2;
					}
				}
				else
				{
					bool flag5 = num2 > minMax.X;
					if (flag5)
					{
						minMax.X = num2;
					}
					bool flag6 = num < minMax.Y;
					if (flag6)
					{
						minMax.Y = num;
					}
				}
				result = (minMax.X <= minMax.Y);
			}
			return result;
		}

		// Token: 0x06004AC2 RID: 19138 RVA: 0x00131644 File Offset: 0x0012F844
		public static int IntersectAABBs(float px, float py, float pz, BoundingBox bbP, float qx, float qy, float qz, BoundingBox bbQ)
		{
			int num = CollisionMath.Intersect1D(px, bbP.Min.X, bbP.Max.X, qx, bbQ.Min.X, bbQ.Max.X, 1E-05f);
			bool flag = num == 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				num &= 9;
				int num2 = CollisionMath.Intersect1D(py, bbP.Min.Y, bbP.Max.Y, qy, bbQ.Min.Y, bbQ.Max.Y, 1E-05f);
				bool flag2 = num2 == 0;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					num2 &= 18;
					int num3 = CollisionMath.Intersect1D(pz, bbP.Min.Z, bbP.Max.Z, qz, bbQ.Min.Z, bbQ.Max.Z, 1E-05f);
					bool flag3 = num3 == 0;
					if (flag3)
					{
						result = 0;
					}
					else
					{
						num3 &= 36;
						result = (num | num2 | num3);
					}
				}
			}
			return result;
		}

		// Token: 0x06004AC3 RID: 19139 RVA: 0x00131750 File Offset: 0x0012F950
		public static int Intersect1D(float p, float pMin, float pMax, float q, float qMin, float qMax, float thickness = 1E-05f)
		{
			double num = (double)(q - p);
			double num2 = (double)(pMin - qMax) - num;
			bool flag = num2 > (double)thickness;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = num2 > (double)(-(double)thickness);
				if (flag2)
				{
					result = 7;
				}
				else
				{
					num2 = (double)(qMin - pMax) + num;
					bool flag3 = num2 > (double)thickness;
					if (flag3)
					{
						result = 0;
					}
					else
					{
						bool flag4 = num2 > (double)(-(double)thickness);
						if (flag4)
						{
							result = 7;
						}
						else
						{
							result = 56;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06004AC4 RID: 19140 RVA: 0x001317BC File Offset: 0x0012F9BC
		public static bool IsBelowMovementThreshold(Vector3 v)
		{
			return v.LengthSquared() < 9.9999994E-11f;
		}

		// Token: 0x0400266A RID: 9834
		public const float MovementThreshold = 1E-05f;

		// Token: 0x0400266B RID: 9835
		public const float MovementThresholdSquared = 9.9999994E-11f;

		// Token: 0x0400266C RID: 9836
		public const float Extent = 1E-05f;

		// Token: 0x0400266D RID: 9837
		public const int Disjoint = 0;

		// Token: 0x0400266E RID: 9838
		public const int TouchX = 1;

		// Token: 0x0400266F RID: 9839
		public const int TouchY = 2;

		// Token: 0x04002670 RID: 9840
		public const int TouchZ = 4;

		// Token: 0x04002671 RID: 9841
		public const int TouchAny = 7;

		// Token: 0x04002672 RID: 9842
		public const int OverlapX = 8;

		// Token: 0x04002673 RID: 9843
		public const int OverlapY = 16;

		// Token: 0x04002674 RID: 9844
		public const int OverlapZ = 32;

		// Token: 0x04002675 RID: 9845
		public const int OverlapAny = 56;

		// Token: 0x04002676 RID: 9846
		public const int OverlapAll = 56;
	}
}
