using System;
using HytaleClient.InGame.Modules.Entities.Projectile;
using HytaleClient.Math;

namespace HytaleClient.Utils
{
	// Token: 0x020007CA RID: 1994
	internal static class PhysicsMath
	{
		// Token: 0x06003400 RID: 13312 RVA: 0x00052BAC File Offset: 0x00050DAC
		public static float GetAcceleration(float velocity, float terminalVelocity)
		{
			float num = Math.Abs(velocity / terminalVelocity);
			return 32f * (1f - num * num * num);
		}

		// Token: 0x06003401 RID: 13313 RVA: 0x00052BD8 File Offset: 0x00050DD8
		public static float GetTerminalVelocity(float mass, float density, float areaMillimetersSquared, float dragCoefficient)
		{
			float num = mass * 1000f;
			float num2 = areaMillimetersSquared * 1000000f;
			float num3 = 64f * num / (density * num2 * dragCoefficient);
			return (float)Math.Sqrt((double)num3);
		}

		// Token: 0x06003402 RID: 13314 RVA: 0x00052C10 File Offset: 0x00050E10
		public static float ComputeProjectedArea(float x, float y, float z, BoundingBox box)
		{
			float num = 0f;
			Vector3 size = box.GetSize();
			bool flag = x != 0f;
			if (flag)
			{
				num += Math.Abs(x) * size.Z * size.Y;
			}
			bool flag2 = y != 0f;
			if (flag2)
			{
				num += Math.Abs(y) * size.Z * size.X;
			}
			bool flag3 = z != 0f;
			if (flag3)
			{
				num += Math.Abs(z) * size.X * size.Y;
			}
			return num;
		}

		// Token: 0x06003403 RID: 13315 RVA: 0x00052CB0 File Offset: 0x00050EB0
		public static float ComputeProjectedArea(Vector3 direction, BoundingBox box)
		{
			return PhysicsMath.ComputeProjectedArea(direction.X, direction.Y, direction.Z, box);
		}

		// Token: 0x06003404 RID: 13316 RVA: 0x00052CDC File Offset: 0x00050EDC
		public static float VolumeOfIntersection(BoundingBox a, Vector3 posA, BoundingBox b, float posBX, float posBY, float posBZ)
		{
			posBX -= posA.X;
			posBY -= posA.Y;
			posBZ -= posA.Z;
			return PhysicsMath.LengthOfIntersection(a.Min.X, a.Max.X, posBX + b.Min.X, posBX + b.Max.X) * PhysicsMath.LengthOfIntersection(a.Min.Y, a.Max.Y, posBY + b.Min.Y, posBY + b.Max.Y) * PhysicsMath.LengthOfIntersection(a.Min.Z, a.Max.Z, posBZ + b.Min.Z, posBZ + b.Max.Z);
		}

		// Token: 0x06003405 RID: 13317 RVA: 0x00052DB4 File Offset: 0x00050FB4
		public static float LengthOfIntersection(float aMin, float aMax, float bMin, float bMax)
		{
			float num = Math.Max(aMin, bMin);
			float num2 = Math.Min(aMax, bMax);
			return Math.Max(0f, num2 - num);
		}

		// Token: 0x06003406 RID: 13318 RVA: 0x00052DE4 File Offset: 0x00050FE4
		public static float HeadingFromDirection(float x, float z)
		{
			return 1f * TrigMathUtil.Atan2(-x, -z);
		}

		// Token: 0x06003407 RID: 13319 RVA: 0x00052E08 File Offset: 0x00051008
		public static float NormalizeAngle(float rad)
		{
			rad %= 6.2831855f;
			bool flag = rad < 0f;
			if (flag)
			{
				rad += 6.2831855f;
			}
			return rad;
		}

		// Token: 0x06003408 RID: 13320 RVA: 0x00052E3C File Offset: 0x0005103C
		public static float NormalizeTurnAngle(float rad)
		{
			rad = PhysicsMath.NormalizeAngle(rad);
			bool flag = rad >= 3.1415927f;
			if (flag)
			{
				rad -= 6.2831855f;
			}
			return rad;
		}

		// Token: 0x06003409 RID: 13321 RVA: 0x00052E74 File Offset: 0x00051074
		public static float PitchFromDirection(float x, float y, float z)
		{
			return TrigMathUtil.Atan2((double)y, Math.Sqrt((double)(x * x + z * z)));
		}

		// Token: 0x0600340A RID: 13322 RVA: 0x00052E9C File Offset: 0x0005109C
		public static Vector3 VectorFromAngles(float heading, float pitch, ref Vector3 outDirection)
		{
			float num = PhysicsMath.PitchX(pitch);
			outDirection.Y = PhysicsMath.PitchY(pitch);
			outDirection.X = PhysicsMath.HeadingX(heading) * num;
			outDirection.Z = PhysicsMath.HeadingZ(heading) * num;
			return outDirection;
		}

		// Token: 0x0600340B RID: 13323 RVA: 0x00052EE4 File Offset: 0x000510E4
		public static float PitchX(float pitch)
		{
			return TrigMathUtil.Cos(pitch);
		}

		// Token: 0x0600340C RID: 13324 RVA: 0x00052EFC File Offset: 0x000510FC
		public static float PitchY(float pitch)
		{
			return TrigMathUtil.Sin(pitch);
		}

		// Token: 0x0600340D RID: 13325 RVA: 0x00052F14 File Offset: 0x00051114
		public static float HeadingX(float heading)
		{
			return -TrigMathUtil.Sin(1f * heading);
		}

		// Token: 0x0600340E RID: 13326 RVA: 0x00052F34 File Offset: 0x00051134
		public static float HeadingZ(float heading)
		{
			return -TrigMathUtil.Cos(1f * heading);
		}

		// Token: 0x0600340F RID: 13327 RVA: 0x00052F54 File Offset: 0x00051154
		public static float ComputeDragCoefficient(float terminalSpeed, float area, float mass, float gravity)
		{
			return mass * gravity / (area * terminalSpeed * terminalSpeed);
		}

		// Token: 0x04001751 RID: 5969
		public const float GRAVITY_ACCELERATION = 32f;

		// Token: 0x04001752 RID: 5970
		public const float DensityAir = 1.2f;

		// Token: 0x04001753 RID: 5971
		public const float DensityWater = 998f;

		// Token: 0x04001754 RID: 5972
		public const float AIR_DENSITY = 0.001225f;

		// Token: 0x04001755 RID: 5973
		public const float HeadingDirection = 1f;
	}
}
