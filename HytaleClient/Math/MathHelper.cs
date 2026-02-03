using System;
using System.Runtime.CompilerServices;
using HytaleClient.Protocol;

namespace HytaleClient.Math
{
	// Token: 0x020007E7 RID: 2023
	public static class MathHelper
	{
		// Token: 0x060035F9 RID: 13817 RVA: 0x00064BA0 File Offset: 0x00062DA0
		public static float Barycentric(float value1, float value2, float value3, float amount1, float amount2)
		{
			return value1 + (value2 - value1) * amount1 + (value3 - value1) * amount2;
		}

		// Token: 0x060035FA RID: 13818 RVA: 0x00064BC0 File Offset: 0x00062DC0
		public static float CatmullRom(float value1, float value2, float value3, float value4, float amount)
		{
			double num = (double)(amount * amount);
			double num2 = num * (double)amount;
			return (float)(0.5 * (2.0 * (double)value2 + (double)((value3 - value1) * amount) + (2.0 * (double)value1 - 5.0 * (double)value2 + 4.0 * (double)value3 - (double)value4) * num + (3.0 * (double)value2 - (double)value1 - 3.0 * (double)value3 + (double)value4) * num2));
		}

		// Token: 0x060035FB RID: 13819 RVA: 0x00064C4C File Offset: 0x00062E4C
		public static float Clamp(float value, float min, float max)
		{
			value = ((value > max) ? max : value);
			value = ((value < min) ? min : value);
			return value;
		}

		// Token: 0x060035FC RID: 13820 RVA: 0x00064C74 File Offset: 0x00062E74
		public static float Distance(float value1, float value2)
		{
			return Math.Abs(value1 - value2);
		}

		// Token: 0x060035FD RID: 13821 RVA: 0x00064C90 File Offset: 0x00062E90
		public static float Hermite(float value1, float tangent1, float value2, float tangent2, float amount)
		{
			double num = (double)value1;
			double num2 = (double)value2;
			double num3 = (double)tangent1;
			double num4 = (double)tangent2;
			double num5 = (double)amount;
			double num6 = num5 * num5 * num5;
			double num7 = num5 * num5;
			bool flag = MathHelper.WithinEpsilon(amount, 0f);
			double num8;
			if (flag)
			{
				num8 = (double)value1;
			}
			else
			{
				bool flag2 = MathHelper.WithinEpsilon(amount, 1f);
				if (flag2)
				{
					num8 = (double)value2;
				}
				else
				{
					num8 = (2.0 * num - 2.0 * num2 + num4 + num3) * num6 + (3.0 * num2 - 3.0 * num - 2.0 * num3 - num4) * num7 + num3 * num5 + num;
				}
			}
			return (float)num8;
		}

		// Token: 0x060035FE RID: 13822 RVA: 0x00064D50 File Offset: 0x00062F50
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Lerp(float value1, float value2, float amount)
		{
			return value1 + (value2 - value1) * amount;
		}

		// Token: 0x060035FF RID: 13823 RVA: 0x00064D6C File Offset: 0x00062F6C
		public static float Max(float value1, float value2)
		{
			return (value1 > value2) ? value1 : value2;
		}

		// Token: 0x06003600 RID: 13824 RVA: 0x00064D88 File Offset: 0x00062F88
		public static float Min(float value1, float value2)
		{
			return (value1 < value2) ? value1 : value2;
		}

		// Token: 0x06003601 RID: 13825 RVA: 0x00064DA4 File Offset: 0x00062FA4
		public static float Min(float v, float a, float c)
		{
			bool flag = a < v;
			if (flag)
			{
				v = a;
			}
			bool flag2 = c < v;
			if (flag2)
			{
				v = c;
			}
			return v;
		}

		// Token: 0x06003602 RID: 13826 RVA: 0x00064DD0 File Offset: 0x00062FD0
		public static float Max(float v, float a, float b)
		{
			bool flag = a > v;
			if (flag)
			{
				v = a;
			}
			bool flag2 = b > v;
			if (flag2)
			{
				v = b;
			}
			return v;
		}

		// Token: 0x06003603 RID: 13827 RVA: 0x00064DFC File Offset: 0x00062FFC
		public static float SmoothStep(float value1, float value2, float amount)
		{
			float amount2 = MathHelper.Clamp(amount, 0f, 1f);
			return MathHelper.Hermite(value1, 0f, value2, 0f, amount2);
		}

		// Token: 0x06003604 RID: 13828 RVA: 0x00064E34 File Offset: 0x00063034
		public static float Sin(float a)
		{
			return (float)Math.Sin((double)a);
		}

		// Token: 0x06003605 RID: 13829 RVA: 0x00064E50 File Offset: 0x00063050
		public static float Cos(float a)
		{
			return (float)Math.Cos((double)a);
		}

		// Token: 0x06003606 RID: 13830 RVA: 0x00064E6C File Offset: 0x0006306C
		public static float ToDegrees(float radians)
		{
			return (float)((double)radians * 57.29577951308232);
		}

		// Token: 0x06003607 RID: 13831 RVA: 0x00064E8C File Offset: 0x0006308C
		public static float ToRadians(float degrees)
		{
			return (float)((double)degrees * 0.017453292519943295);
		}

		// Token: 0x06003608 RID: 13832 RVA: 0x00064EAC File Offset: 0x000630AC
		public static float WrapAngle(float angle)
		{
			angle = (float)Math.IEEERemainder((double)angle, 6.2831854820251465);
			bool flag = angle <= -3.1415927f;
			if (flag)
			{
				angle += 6.2831855f;
			}
			else
			{
				bool flag2 = angle > 3.1415927f;
				if (flag2)
				{
					angle -= 6.2831855f;
				}
			}
			return angle;
		}

		// Token: 0x06003609 RID: 13833 RVA: 0x00064F08 File Offset: 0x00063108
		internal static int Clamp(int value, int min, int max)
		{
			value = ((value > max) ? max : value);
			value = ((value < min) ? min : value);
			return value;
		}

		// Token: 0x0600360A RID: 13834 RVA: 0x00064F30 File Offset: 0x00063130
		internal static bool WithinEpsilon(float floatA, float floatB)
		{
			return Math.Abs(floatA - floatB) < MathHelper.MachineEpsilonFloat;
		}

		// Token: 0x0600360B RID: 13835 RVA: 0x00064F54 File Offset: 0x00063154
		internal static int ClosestMSAAPower(int value)
		{
			bool flag = value == 1;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int num = value - 1;
				num |= num >> 1;
				num |= num >> 2;
				num |= num >> 4;
				num |= num >> 8;
				num |= num >> 16;
				num++;
				bool flag2 = num == value;
				if (flag2)
				{
					result = num;
				}
				else
				{
					result = num >> 1;
				}
			}
			return result;
		}

		// Token: 0x0600360C RID: 13836 RVA: 0x00064FAC File Offset: 0x000631AC
		private static float GetMachineEpsilonFloat()
		{
			float num = 1f;
			float num2;
			do
			{
				num *= 0.5f;
				num2 = 1f + num;
			}
			while (num2 > 1f);
			return num;
		}

		// Token: 0x0600360D RID: 13837 RVA: 0x00064FE3 File Offset: 0x000631E3
		public static int Round(float value)
		{
			return (int)Math.Round((double)value, MidpointRounding.AwayFromZero);
		}

		// Token: 0x0600360E RID: 13838 RVA: 0x00064FEE File Offset: 0x000631EE
		public static double FloorMod(double x, double y)
		{
			return x - Math.Floor(x / y) * y;
		}

		// Token: 0x0600360F RID: 13839 RVA: 0x00064FFC File Offset: 0x000631FC
		public static float Step(float value, float goal, float step)
		{
			bool flag = value < goal;
			float result;
			if (flag)
			{
				result = Math.Min(value + step, goal);
			}
			else
			{
				bool flag2 = value > goal;
				if (flag2)
				{
					result = Math.Max(value - step, goal);
				}
				else
				{
					result = value;
				}
			}
			return result;
		}

		// Token: 0x06003610 RID: 13840 RVA: 0x00065038 File Offset: 0x00063238
		public static float ShortAngleDistance(float value1, float value2)
		{
			float num = (value2 - value1) % 6.2831855f;
			return 2f * num % 6.2831855f - num;
		}

		// Token: 0x06003611 RID: 13841 RVA: 0x00065064 File Offset: 0x00063264
		public static float LerpAngle(float value1, float value2, float amount)
		{
			return value1 + MathHelper.ShortAngleDistance(value1, value2) * amount;
		}

		// Token: 0x06003612 RID: 13842 RVA: 0x00065084 File Offset: 0x00063284
		public static float SnapRadianTo90Degrees(float radian)
		{
			return (float)(1.5707963705062866 * Math.Round((double)(radian / 1.5707964f)));
		}

		// Token: 0x06003613 RID: 13843 RVA: 0x000650B0 File Offset: 0x000632B0
		public static int WrapAngleDegrees(int angle)
		{
			angle = ((angle + 180) % 360 + 360) % 360;
			return angle - 180;
		}

		// Token: 0x06003614 RID: 13844 RVA: 0x000650E4 File Offset: 0x000632E4
		public static double CompareAngle(double a, double b)
		{
			double num = b - a;
			return MathHelper.FloorMod(num + 3.1415927410125732, 6.2831854820251465) - 3.1415927410125732;
		}

		// Token: 0x06003615 RID: 13845 RVA: 0x00065120 File Offset: 0x00063320
		public static uint HashUnsigned(uint value)
		{
			value = (value >> 16 ^ value) * 73244475U;
			value = (value >> 16 ^ value) * 73244475U;
			value = (value >> 16 ^ value);
			return value;
		}

		// Token: 0x06003616 RID: 13846 RVA: 0x00065158 File Offset: 0x00063358
		public static int Hash(int value)
		{
			return (int)MathHelper.HashUnsigned((uint)value);
		}

		// Token: 0x06003617 RID: 13847 RVA: 0x00065170 File Offset: 0x00063370
		public static uint HashUnsigned(uint x, uint y, uint z)
		{
			uint num = (MathHelper.HashUnsigned(x) >> 16 ^ x) * 73244475U;
			num = (MathHelper.HashUnsigned(y) >> 16 ^ num) * 73244475U;
			num = (MathHelper.HashUnsigned(z) >> 16 ^ num);
			return MathHelper.HashUnsigned(num);
		}

		// Token: 0x06003618 RID: 13848 RVA: 0x000651BC File Offset: 0x000633BC
		public static int Hash(int x, int y, int z)
		{
			return (int)MathHelper.HashUnsigned((uint)x, (uint)y, (uint)z);
		}

		// Token: 0x06003619 RID: 13849 RVA: 0x000651D8 File Offset: 0x000633D8
		public static uint HashUnsigned(uint x, uint z)
		{
			uint num = (MathHelper.HashUnsigned(x) >> 16 ^ x) * 73244475U;
			num = (MathHelper.HashUnsigned(z) >> 16 ^ num);
			return MathHelper.HashUnsigned(num);
		}

		// Token: 0x0600361A RID: 13850 RVA: 0x00065210 File Offset: 0x00063410
		public static int Hash(int x, int z)
		{
			return (int)MathHelper.HashUnsigned((uint)x, (uint)z);
		}

		// Token: 0x0600361B RID: 13851 RVA: 0x0006522C File Offset: 0x0006342C
		public static float Spline(float t, float before, float start, float end, float after)
		{
			return 0.5f * (2f * start + (-before + end) * t + (2f * before - 5f * start + 4f * end - after) * (t * t) + (-before + 3f * start - 3f * end + after) * (t * t * t));
		}

		// Token: 0x0600361C RID: 13852 RVA: 0x00065290 File Offset: 0x00063490
		public static float CubicBezierCurve(float t, float start, float control0, float control1, float end)
		{
			return start + (-start * 3f + t * (3f * start - start * t)) * t + (3f * control0 + t * (-6f * control0 + control0 * 3f * t)) * t + (control1 * 3f - control1 * 3f * t) * t * t + end * t * t * t;
		}

		// Token: 0x0600361D RID: 13853 RVA: 0x000652FC File Offset: 0x000634FC
		public static float CubicBezierCurveTangent(float t, float start, float control0, float control1, float end)
		{
			return 3f * (end - 3f * control1 + 3f * control0 - start) * t * t + (2f * (3f * control1) - 6f * control0 + 3f * start * t) + 3f * control0 - 3f * start;
		}

		// Token: 0x0600361E RID: 13854 RVA: 0x0006535C File Offset: 0x0006355C
		public static void RotateAroundPoint(ref int x, ref int y, float radians, int originX, int originY)
		{
			double num = Math.Cos((double)radians);
			double num2 = Math.Sin((double)radians);
			int num3 = x - originX;
			int num4 = y - originY;
			x = (int)(num * (double)num3 - num2 * (double)num4 + (double)originX);
			y = (int)(num2 * (double)num3 + num * (double)num4 + (double)originY);
		}

		// Token: 0x0600361F RID: 13855 RVA: 0x000653A4 File Offset: 0x000635A4
		public static float SplineAngle(float t, float p0, float p1, float p2, float p3)
		{
			bool flag = p1 - p0 > 3.1415927f;
			if (flag)
			{
				p1 -= 6.2831855f;
			}
			else
			{
				bool flag2 = p1 - p0 < -3.1415927f;
				if (flag2)
				{
					p1 += 6.2831855f;
				}
			}
			bool flag3 = p2 - p1 > 3.1415927f;
			if (flag3)
			{
				p2 -= 6.2831855f;
				p3 -= 6.2831855f;
			}
			else
			{
				bool flag4 = p2 - p1 < -3.1415927f;
				if (flag4)
				{
					p2 += 6.2831855f;
					p3 += 6.2831855f;
				}
			}
			bool flag5 = p3 - p2 > 3.1415927f;
			if (flag5)
			{
				p3 -= 6.2831855f;
			}
			else
			{
				bool flag6 = p3 - p2 < -3.1415927f;
				if (flag6)
				{
					p3 += 6.2831855f;
				}
			}
			return MathHelper.Spline(t, p0, p1, p2, p3);
		}

		// Token: 0x06003620 RID: 13856 RVA: 0x00065474 File Offset: 0x00063674
		public static float SnapValue(float value, float interval)
		{
			float num = value % interval;
			bool flag = num == 0f;
			float result;
			if (flag)
			{
				result = value;
			}
			else
			{
				value -= num;
				bool flag2 = num * 2f >= interval;
				if (flag2)
				{
					value += interval;
				}
				else
				{
					bool flag3 = num * 2f < -interval;
					if (flag3)
					{
						value -= interval;
					}
				}
				result = value;
			}
			return result;
		}

		// Token: 0x06003621 RID: 13857 RVA: 0x000654D0 File Offset: 0x000636D0
		public static float Slerp(float a, float b, float t)
		{
			return MathHelper.Lerp(a, b, MathHelper.GetSlerpRatio(t));
		}

		// Token: 0x06003622 RID: 13858 RVA: 0x000654F0 File Offset: 0x000636F0
		public static float GetSlerpRatio(float t)
		{
			return (float)((Math.Sin((double)(t * 3.1415927f - 1.5707964f)) + 1.0) / 2.0);
		}

		// Token: 0x06003623 RID: 13859 RVA: 0x0006552C File Offset: 0x0006372C
		public static bool IsPowerOfTwo(int x)
		{
			return (x & x - 1) == 0;
		}

		// Token: 0x06003624 RID: 13860 RVA: 0x00065548 File Offset: 0x00063748
		public static float RotationToRadians(Rotation rotation)
		{
			return rotation * 1.5707964f;
		}

		// Token: 0x06003625 RID: 13861 RVA: 0x00065564 File Offset: 0x00063764
		public static int RotationToDegrees(Rotation rotation)
		{
			return rotation * 90;
		}

		// Token: 0x06003626 RID: 13862 RVA: 0x0006557C File Offset: 0x0006377C
		public static uint NumberOfLeadingZeros(uint i)
		{
			bool flag = i == 0U;
			uint result;
			if (flag)
			{
				result = 32U;
			}
			else
			{
				uint num = 1U;
				bool flag2 = i >> 16 == 0U;
				if (flag2)
				{
					num += 16U;
					i <<= 16;
				}
				bool flag3 = i >> 24 == 0U;
				if (flag3)
				{
					num += 8U;
					i <<= 8;
				}
				bool flag4 = i >> 28 == 0U;
				if (flag4)
				{
					num += 4U;
					i <<= 4;
				}
				bool flag5 = i >> 30 == 0U;
				if (flag5)
				{
					num += 2U;
					i <<= 2;
				}
				num -= i >> 31;
				result = num;
			}
			return result;
		}

		// Token: 0x06003627 RID: 13863 RVA: 0x00065608 File Offset: 0x00063808
		public static float Square(float value)
		{
			return value * value;
		}

		// Token: 0x06003628 RID: 13864 RVA: 0x00065620 File Offset: 0x00063820
		public static void ComputeScreenArea(Vector3 position, Vector3 extent, ref Matrix viewProjectionMatrix, out Vector4 screenArea)
		{
			Vector4 vector = new Vector4(position + new Vector3(extent.X, 0f, 0f), 1f);
			Vector4 vector2 = new Vector4(position + new Vector3(extent.X, extent.Y, 0f), 1f);
			Vector4 vector3 = new Vector4(position + new Vector3(extent.X, extent.Y, extent.Z), 1f);
			Vector4 vector4 = new Vector4(position + new Vector3(extent.X, 0f, extent.Z), 1f);
			Vector4 vector5 = new Vector4(position + new Vector3(0f, 0f, 0f), 1f);
			Vector4 vector6 = new Vector4(position + new Vector3(0f, extent.Y, 0f), 1f);
			Vector4 vector7 = new Vector4(position + new Vector3(0f, extent.Y, extent.Z), 1f);
			Vector4 vector8 = new Vector4(position + new Vector3(0f, 0f, extent.Z), 1f);
			Vector4 vector9;
			Vector4.Transform(ref vector, ref viewProjectionMatrix, out vector9);
			Vector4 vector10;
			Vector4.Transform(ref vector2, ref viewProjectionMatrix, out vector10);
			Vector4 vector11;
			Vector4.Transform(ref vector3, ref viewProjectionMatrix, out vector11);
			Vector4 vector12;
			Vector4.Transform(ref vector4, ref viewProjectionMatrix, out vector12);
			Vector4 vector13;
			Vector4.Transform(ref vector5, ref viewProjectionMatrix, out vector13);
			Vector4 vector14;
			Vector4.Transform(ref vector6, ref viewProjectionMatrix, out vector14);
			Vector4 vector15;
			Vector4.Transform(ref vector7, ref viewProjectionMatrix, out vector15);
			Vector4 vector16;
			Vector4.Transform(ref vector8, ref viewProjectionMatrix, out vector16);
			vector9.X /= vector9.W;
			vector9.Y /= vector9.W;
			vector10.X /= vector10.W;
			vector10.Y /= vector10.W;
			vector11.X /= vector11.W;
			vector11.Y /= vector11.W;
			vector12.X /= vector12.W;
			vector12.Y /= vector12.W;
			vector13.X /= vector13.W;
			vector13.Y /= vector13.W;
			vector14.X /= vector14.W;
			vector14.Y /= vector14.W;
			vector15.X /= vector15.W;
			vector15.Y /= vector15.W;
			vector16.X /= vector16.W;
			vector16.Y /= vector16.W;
			screenArea.X = Math.Min(vector9.X, Math.Min(vector10.X, Math.Min(vector11.X, Math.Min(vector12.X, Math.Min(vector13.X, Math.Min(vector14.X, Math.Min(vector15.X, vector16.X)))))));
			screenArea.Y = Math.Min(vector9.Y, Math.Min(vector10.Y, Math.Min(vector11.Y, Math.Min(vector12.Y, Math.Min(vector13.Y, Math.Min(vector14.Y, Math.Min(vector15.Y, vector16.Y)))))));
			screenArea.Z = Math.Max(vector9.X, Math.Max(vector10.X, Math.Max(vector11.X, Math.Max(vector12.X, Math.Max(vector13.X, Math.Max(vector14.X, Math.Max(vector15.X, vector16.X)))))));
			screenArea.W = Math.Max(vector9.Y, Math.Max(vector10.Y, Math.Max(vector11.Y, Math.Max(vector12.Y, Math.Max(vector13.Y, Math.Max(vector14.Y, Math.Max(vector15.Y, vector16.Y)))))));
			Vector4 value = new Vector4(0.5f);
			screenArea = screenArea * value + value;
		}

		// Token: 0x06003629 RID: 13865 RVA: 0x00065A8C File Offset: 0x00063C8C
		public static float ConvertToNewRange(float value, float oldMinRange, float oldMaxRange, float newMinRange, float newMaxRange)
		{
			bool flag = newMinRange == newMaxRange || oldMinRange == oldMaxRange;
			float result;
			if (flag)
			{
				result = newMinRange;
			}
			else
			{
				float value2 = (value - oldMinRange) * (newMaxRange - newMinRange) / (oldMaxRange - oldMinRange) + newMinRange;
				result = MathHelper.Clamp(value2, MathHelper.Min(newMinRange, newMaxRange), MathHelper.Max(newMinRange, newMaxRange));
			}
			return result;
		}

		// Token: 0x0600362A RID: 13866 RVA: 0x00065AD8 File Offset: 0x00063CD8
		public static float ClipToZero(float v, float epsilon)
		{
			return (v >= -epsilon && v <= epsilon) ? 0f : v;
		}

		// Token: 0x04001801 RID: 6145
		public const float E = 2.7182817f;

		// Token: 0x04001802 RID: 6146
		public const float Log10E = 0.4342945f;

		// Token: 0x04001803 RID: 6147
		public const float Log2E = 1.442695f;

		// Token: 0x04001804 RID: 6148
		public const float Pi = 3.1415927f;

		// Token: 0x04001805 RID: 6149
		public const float PiOver2 = 1.5707964f;

		// Token: 0x04001806 RID: 6150
		public const float PiOver4 = 0.7853982f;

		// Token: 0x04001807 RID: 6151
		public const float TwoPi = 6.2831855f;

		// Token: 0x04001808 RID: 6152
		internal static readonly float MachineEpsilonFloat = MathHelper.GetMachineEpsilonFloat();

		// Token: 0x04001809 RID: 6153
		public const float TwoPiOver3 = 2.0943952f;

		// Token: 0x0400180A RID: 6154
		public static readonly int HashOne = MathHelper.Hash(1);

		// Token: 0x0400180B RID: 6155
		public static readonly int HashTwo = MathHelper.Hash(2);

		// Token: 0x0400180C RID: 6156
		public static readonly int HashThree = MathHelper.Hash(3);

		// Token: 0x0400180D RID: 6157
		public static readonly int HashFour = MathHelper.Hash(4);

		// Token: 0x0400180E RID: 6158
		public static readonly int HashFive = MathHelper.Hash(5);
	}
}
