using System;
using System.Diagnostics;
using Coherent.UI.Binding;

namespace HytaleClient.Math
{
	// Token: 0x020007F7 RID: 2039
	[CoherentType]
	[DebuggerDisplay("{DebugDisplayString,nq}")]
	[Serializable]
	public struct Vector2 : IEquatable<Vector2>
	{
		// Token: 0x17000FE6 RID: 4070
		// (get) Token: 0x0600375A RID: 14170 RVA: 0x0006E57C File Offset: 0x0006C77C
		public static Vector2 Zero
		{
			get
			{
				return Vector2.zeroVector;
			}
		}

		// Token: 0x17000FE7 RID: 4071
		// (get) Token: 0x0600375B RID: 14171 RVA: 0x0006E594 File Offset: 0x0006C794
		public static Vector2 One
		{
			get
			{
				return Vector2.unitVector;
			}
		}

		// Token: 0x17000FE8 RID: 4072
		// (get) Token: 0x0600375C RID: 14172 RVA: 0x0006E5AC File Offset: 0x0006C7AC
		public static Vector2 UnitX
		{
			get
			{
				return Vector2.unitXVector;
			}
		}

		// Token: 0x17000FE9 RID: 4073
		// (get) Token: 0x0600375D RID: 14173 RVA: 0x0006E5C4 File Offset: 0x0006C7C4
		public static Vector2 UnitY
		{
			get
			{
				return Vector2.unitYVector;
			}
		}

		// Token: 0x17000FEA RID: 4074
		// (get) Token: 0x0600375E RID: 14174 RVA: 0x0006E5DC File Offset: 0x0006C7DC
		internal string DebugDisplayString
		{
			get
			{
				return this.X.ToString() + " " + this.Y.ToString();
			}
		}

		// Token: 0x0600375F RID: 14175 RVA: 0x0006E60E File Offset: 0x0006C80E
		public Vector2(float x, float y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x06003760 RID: 14176 RVA: 0x0006E61F File Offset: 0x0006C81F
		public Vector2(float value)
		{
			this.X = value;
			this.Y = value;
		}

		// Token: 0x06003761 RID: 14177 RVA: 0x0006E630 File Offset: 0x0006C830
		public override bool Equals(object obj)
		{
			return obj is Vector2 && this.Equals((Vector2)obj);
		}

		// Token: 0x06003762 RID: 14178 RVA: 0x0006E65C File Offset: 0x0006C85C
		public bool Equals(Vector2 other)
		{
			return this.X == other.X && this.Y == other.Y;
		}

		// Token: 0x06003763 RID: 14179 RVA: 0x0006E690 File Offset: 0x0006C890
		public override int GetHashCode()
		{
			return this.X.GetHashCode() + this.Y.GetHashCode();
		}

		// Token: 0x06003764 RID: 14180 RVA: 0x0006E6BC File Offset: 0x0006C8BC
		public float Length()
		{
			return (float)Math.Sqrt((double)(this.X * this.X + this.Y * this.Y));
		}

		// Token: 0x06003765 RID: 14181 RVA: 0x0006E6F0 File Offset: 0x0006C8F0
		public float LengthSquared()
		{
			return this.X * this.X + this.Y * this.Y;
		}

		// Token: 0x06003766 RID: 14182 RVA: 0x0006E720 File Offset: 0x0006C920
		public void Normalize()
		{
			float num = 1f / (float)Math.Sqrt((double)(this.X * this.X + this.Y * this.Y));
			this.X *= num;
			this.Y *= num;
		}

		// Token: 0x06003767 RID: 14183 RVA: 0x0006E774 File Offset: 0x0006C974
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{X:",
				this.X.ToString(),
				" Y:",
				this.Y.ToString(),
				"}"
			});
		}

		// Token: 0x06003768 RID: 14184 RVA: 0x0006E7C8 File Offset: 0x0006C9C8
		public static Vector2 Add(Vector2 value1, Vector2 value2)
		{
			value1.X += value2.X;
			value1.Y += value2.Y;
			return value1;
		}

		// Token: 0x06003769 RID: 14185 RVA: 0x0006E7FD File Offset: 0x0006C9FD
		public static void Add(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
		{
			result.X = value1.X + value2.X;
			result.Y = value1.Y + value2.Y;
		}

		// Token: 0x0600376A RID: 14186 RVA: 0x0006E828 File Offset: 0x0006CA28
		public static Vector2 Barycentric(Vector2 value1, Vector2 value2, Vector2 value3, float amount1, float amount2)
		{
			return new Vector2(MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2), MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2));
		}

		// Token: 0x0600376B RID: 14187 RVA: 0x0006E874 File Offset: 0x0006CA74
		public static void Barycentric(ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, float amount1, float amount2, out Vector2 result)
		{
			result.X = MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2);
			result.Y = MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2);
		}

		// Token: 0x0600376C RID: 14188 RVA: 0x0006E8C4 File Offset: 0x0006CAC4
		public static Vector2 CatmullRom(Vector2 value1, Vector2 value2, Vector2 value3, Vector2 value4, float amount)
		{
			return new Vector2(MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount), MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount));
		}

		// Token: 0x0600376D RID: 14189 RVA: 0x0006E91C File Offset: 0x0006CB1C
		public static void CatmullRom(ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, ref Vector2 value4, float amount, out Vector2 result)
		{
			result.X = MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount);
			result.Y = MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount);
		}

		// Token: 0x0600376E RID: 14190 RVA: 0x0006E978 File Offset: 0x0006CB78
		public static Vector2 Clamp(Vector2 value1, Vector2 min, Vector2 max)
		{
			return new Vector2(MathHelper.Clamp(value1.X, min.X, max.X), MathHelper.Clamp(value1.Y, min.Y, max.Y));
		}

		// Token: 0x0600376F RID: 14191 RVA: 0x0006E9BD File Offset: 0x0006CBBD
		public static void Clamp(ref Vector2 value1, ref Vector2 min, ref Vector2 max, out Vector2 result)
		{
			result.X = MathHelper.Clamp(value1.X, min.X, max.X);
			result.Y = MathHelper.Clamp(value1.Y, min.Y, max.Y);
		}

		// Token: 0x06003770 RID: 14192 RVA: 0x0006E9FC File Offset: 0x0006CBFC
		public static float Distance(Vector2 value1, Vector2 value2)
		{
			float num = value1.X - value2.X;
			float num2 = value1.Y - value2.Y;
			return (float)Math.Sqrt((double)(num * num + num2 * num2));
		}

		// Token: 0x06003771 RID: 14193 RVA: 0x0006EA38 File Offset: 0x0006CC38
		public static void Distance(ref Vector2 value1, ref Vector2 value2, out float result)
		{
			float num = value1.X - value2.X;
			float num2 = value1.Y - value2.Y;
			result = (float)Math.Sqrt((double)(num * num + num2 * num2));
		}

		// Token: 0x06003772 RID: 14194 RVA: 0x0006EA74 File Offset: 0x0006CC74
		public static float DistanceSquared(Vector2 value1, Vector2 value2)
		{
			float num = value1.X - value2.X;
			float num2 = value1.Y - value2.Y;
			return num * num + num2 * num2;
		}

		// Token: 0x06003773 RID: 14195 RVA: 0x0006EAAC File Offset: 0x0006CCAC
		public static void DistanceSquared(ref Vector2 value1, ref Vector2 value2, out float result)
		{
			float num = value1.X - value2.X;
			float num2 = value1.Y - value2.Y;
			result = num * num + num2 * num2;
		}

		// Token: 0x06003774 RID: 14196 RVA: 0x0006EAE0 File Offset: 0x0006CCE0
		public static Vector2 Divide(Vector2 value1, Vector2 value2)
		{
			value1.X /= value2.X;
			value1.Y /= value2.Y;
			return value1;
		}

		// Token: 0x06003775 RID: 14197 RVA: 0x0006EB15 File Offset: 0x0006CD15
		public static void Divide(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
		{
			result.X = value1.X / value2.X;
			result.Y = value1.Y / value2.Y;
		}

		// Token: 0x06003776 RID: 14198 RVA: 0x0006EB40 File Offset: 0x0006CD40
		public static Vector2 Divide(Vector2 value1, float divider)
		{
			float num = 1f / divider;
			value1.X *= num;
			value1.Y *= num;
			return value1;
		}

		// Token: 0x06003777 RID: 14199 RVA: 0x0006EB74 File Offset: 0x0006CD74
		public static void Divide(ref Vector2 value1, float divider, out Vector2 result)
		{
			float num = 1f / divider;
			result.X = value1.X * num;
			result.Y = value1.Y * num;
		}

		// Token: 0x06003778 RID: 14200 RVA: 0x0006EBA8 File Offset: 0x0006CDA8
		public static float Dot(Vector2 value1, Vector2 value2)
		{
			return value1.X * value2.X + value1.Y * value2.Y;
		}

		// Token: 0x06003779 RID: 14201 RVA: 0x0006EBD5 File Offset: 0x0006CDD5
		public static void Dot(ref Vector2 value1, ref Vector2 value2, out float result)
		{
			result = value1.X * value2.X + value1.Y * value2.Y;
		}

		// Token: 0x0600377A RID: 14202 RVA: 0x0006EBF8 File Offset: 0x0006CDF8
		public static Vector2 Hermite(Vector2 value1, Vector2 tangent1, Vector2 value2, Vector2 tangent2, float amount)
		{
			Vector2 result = default(Vector2);
			Vector2.Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
			return result;
		}

		// Token: 0x0600377B RID: 14203 RVA: 0x0006EC28 File Offset: 0x0006CE28
		public static void Hermite(ref Vector2 value1, ref Vector2 tangent1, ref Vector2 value2, ref Vector2 tangent2, float amount, out Vector2 result)
		{
			result.X = MathHelper.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount);
			result.Y = MathHelper.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount);
		}

		// Token: 0x0600377C RID: 14204 RVA: 0x0006EC84 File Offset: 0x0006CE84
		public static Vector2 Lerp(Vector2 value1, Vector2 value2, float amount)
		{
			return new Vector2(MathHelper.Lerp(value1.X, value2.X, amount), MathHelper.Lerp(value1.Y, value2.Y, amount));
		}

		// Token: 0x0600377D RID: 14205 RVA: 0x0006ECBF File Offset: 0x0006CEBF
		public static void Lerp(ref Vector2 value1, ref Vector2 value2, float amount, out Vector2 result)
		{
			result.X = MathHelper.Lerp(value1.X, value2.X, amount);
			result.Y = MathHelper.Lerp(value1.Y, value2.Y, amount);
		}

		// Token: 0x0600377E RID: 14206 RVA: 0x0006ECF4 File Offset: 0x0006CEF4
		public static Vector2 Max(Vector2 value1, Vector2 value2)
		{
			return new Vector2((value1.X > value2.X) ? value1.X : value2.X, (value1.Y > value2.Y) ? value1.Y : value2.Y);
		}

		// Token: 0x0600377F RID: 14207 RVA: 0x0006ED44 File Offset: 0x0006CF44
		public static void Max(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
		{
			result.X = ((value1.X > value2.X) ? value1.X : value2.X);
			result.Y = ((value1.Y > value2.Y) ? value1.Y : value2.Y);
		}

		// Token: 0x06003780 RID: 14208 RVA: 0x0006ED98 File Offset: 0x0006CF98
		public static Vector2 Min(Vector2 value1, Vector2 value2)
		{
			return new Vector2((value1.X < value2.X) ? value1.X : value2.X, (value1.Y < value2.Y) ? value1.Y : value2.Y);
		}

		// Token: 0x06003781 RID: 14209 RVA: 0x0006EDE8 File Offset: 0x0006CFE8
		public static void Min(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
		{
			result.X = ((value1.X < value2.X) ? value1.X : value2.X);
			result.Y = ((value1.Y < value2.Y) ? value1.Y : value2.Y);
		}

		// Token: 0x06003782 RID: 14210 RVA: 0x0006EE3C File Offset: 0x0006D03C
		public static Vector2 Multiply(Vector2 value1, Vector2 value2)
		{
			value1.X *= value2.X;
			value1.Y *= value2.Y;
			return value1;
		}

		// Token: 0x06003783 RID: 14211 RVA: 0x0006EE74 File Offset: 0x0006D074
		public static Vector2 Multiply(Vector2 value1, float scaleFactor)
		{
			value1.X *= scaleFactor;
			value1.Y *= scaleFactor;
			return value1;
		}

		// Token: 0x06003784 RID: 14212 RVA: 0x0006EE9F File Offset: 0x0006D09F
		public static void Multiply(ref Vector2 value1, float scaleFactor, out Vector2 result)
		{
			result.X = value1.X * scaleFactor;
			result.Y = value1.Y * scaleFactor;
		}

		// Token: 0x06003785 RID: 14213 RVA: 0x0006EEBE File Offset: 0x0006D0BE
		public static void Multiply(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
		{
			result.X = value1.X * value2.X;
			result.Y = value1.Y * value2.Y;
		}

		// Token: 0x06003786 RID: 14214 RVA: 0x0006EEE8 File Offset: 0x0006D0E8
		public static Vector2 Negate(Vector2 value)
		{
			value.X = -value.X;
			value.Y = -value.Y;
			return value;
		}

		// Token: 0x06003787 RID: 14215 RVA: 0x0006EF17 File Offset: 0x0006D117
		public static void Negate(ref Vector2 value, out Vector2 result)
		{
			result.X = -value.X;
			result.Y = -value.Y;
		}

		// Token: 0x06003788 RID: 14216 RVA: 0x0006EF34 File Offset: 0x0006D134
		public static Vector2 Normalize(Vector2 value)
		{
			float num = 1f / (float)Math.Sqrt((double)(value.X * value.X + value.Y * value.Y));
			value.X *= num;
			value.Y *= num;
			return value;
		}

		// Token: 0x06003789 RID: 14217 RVA: 0x0006EF88 File Offset: 0x0006D188
		public static void Normalize(ref Vector2 value, out Vector2 result)
		{
			float num = 1f / (float)Math.Sqrt((double)(value.X * value.X + value.Y * value.Y));
			result.X = value.X * num;
			result.Y = value.Y * num;
		}

		// Token: 0x0600378A RID: 14218 RVA: 0x0006EFDC File Offset: 0x0006D1DC
		public static Vector2 Reflect(Vector2 vector, Vector2 normal)
		{
			float num = 2f * (vector.X * normal.X + vector.Y * normal.Y);
			Vector2 result;
			result.X = vector.X - normal.X * num;
			result.Y = vector.Y - normal.Y * num;
			return result;
		}

		// Token: 0x0600378B RID: 14219 RVA: 0x0006F040 File Offset: 0x0006D240
		public static void Reflect(ref Vector2 vector, ref Vector2 normal, out Vector2 result)
		{
			float num = 2f * (vector.X * normal.X + vector.Y * normal.Y);
			result.X = vector.X - normal.X * num;
			result.Y = vector.Y - normal.Y * num;
		}

		// Token: 0x0600378C RID: 14220 RVA: 0x0006F09C File Offset: 0x0006D29C
		public static Vector2 SmoothStep(Vector2 value1, Vector2 value2, float amount)
		{
			return new Vector2(MathHelper.SmoothStep(value1.X, value2.X, amount), MathHelper.SmoothStep(value1.Y, value2.Y, amount));
		}

		// Token: 0x0600378D RID: 14221 RVA: 0x0006F0D7 File Offset: 0x0006D2D7
		public static void SmoothStep(ref Vector2 value1, ref Vector2 value2, float amount, out Vector2 result)
		{
			result.X = MathHelper.SmoothStep(value1.X, value2.X, amount);
			result.Y = MathHelper.SmoothStep(value1.Y, value2.Y, amount);
		}

		// Token: 0x0600378E RID: 14222 RVA: 0x0006F10C File Offset: 0x0006D30C
		public static Vector2 Subtract(Vector2 value1, Vector2 value2)
		{
			value1.X -= value2.X;
			value1.Y -= value2.Y;
			return value1;
		}

		// Token: 0x0600378F RID: 14223 RVA: 0x0006F141 File Offset: 0x0006D341
		public static void Subtract(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
		{
			result.X = value1.X - value2.X;
			result.Y = value1.Y - value2.Y;
		}

		// Token: 0x06003790 RID: 14224 RVA: 0x0006F16C File Offset: 0x0006D36C
		public static Vector2 Transform(Vector2 position, Matrix matrix)
		{
			return new Vector2(position.X * matrix.M11 + position.Y * matrix.M21 + matrix.M41, position.X * matrix.M12 + position.Y * matrix.M22 + matrix.M42);
		}

		// Token: 0x06003791 RID: 14225 RVA: 0x0006F1C8 File Offset: 0x0006D3C8
		public static void Transform(ref Vector2 position, ref Matrix matrix, out Vector2 result)
		{
			float x = position.X * matrix.M11 + position.Y * matrix.M21 + matrix.M41;
			float y = position.X * matrix.M12 + position.Y * matrix.M22 + matrix.M42;
			result.X = x;
			result.Y = y;
		}

		// Token: 0x06003792 RID: 14226 RVA: 0x0006F22C File Offset: 0x0006D42C
		public static Vector2 Transform(Vector2 value, Quaternion rotation)
		{
			Vector2.Transform(ref value, ref rotation, out value);
			return value;
		}

		// Token: 0x06003793 RID: 14227 RVA: 0x0006F24C File Offset: 0x0006D44C
		public static void Transform(ref Vector2 value, ref Quaternion rotation, out Vector2 result)
		{
			float num = 2f * -(rotation.Z * value.Y);
			float num2 = 2f * (rotation.Z * value.X);
			float num3 = 2f * (rotation.X * value.Y - rotation.Y * value.X);
			result.X = value.X + num * rotation.W + (rotation.Y * num3 - rotation.Z * num2);
			result.Y = value.Y + num2 * rotation.W + (rotation.Z * num - rotation.X * num3);
		}

		// Token: 0x06003794 RID: 14228 RVA: 0x0006F2F3 File Offset: 0x0006D4F3
		public static void Transform(Vector2[] sourceArray, ref Matrix matrix, Vector2[] destinationArray)
		{
			Vector2.Transform(sourceArray, 0, ref matrix, destinationArray, 0, sourceArray.Length);
		}

		// Token: 0x06003795 RID: 14229 RVA: 0x0006F304 File Offset: 0x0006D504
		public static void Transform(Vector2[] sourceArray, int sourceIndex, ref Matrix matrix, Vector2[] destinationArray, int destinationIndex, int length)
		{
			for (int i = 0; i < length; i++)
			{
				Vector2 vector = sourceArray[sourceIndex + i];
				Vector2 vector2 = destinationArray[destinationIndex + i];
				vector2.X = vector.X * matrix.M11 + vector.Y * matrix.M21 + matrix.M41;
				vector2.Y = vector.X * matrix.M12 + vector.Y * matrix.M22 + matrix.M42;
				destinationArray[destinationIndex + i] = vector2;
			}
		}

		// Token: 0x06003796 RID: 14230 RVA: 0x0006F39A File Offset: 0x0006D59A
		public static void Transform(Vector2[] sourceArray, ref Quaternion rotation, Vector2[] destinationArray)
		{
			Vector2.Transform(sourceArray, 0, ref rotation, destinationArray, 0, sourceArray.Length);
		}

		// Token: 0x06003797 RID: 14231 RVA: 0x0006F3AC File Offset: 0x0006D5AC
		public static void Transform(Vector2[] sourceArray, int sourceIndex, ref Quaternion rotation, Vector2[] destinationArray, int destinationIndex, int length)
		{
			for (int i = 0; i < length; i++)
			{
				Vector2 vector = sourceArray[sourceIndex + i];
				Vector2 vector2;
				Vector2.Transform(ref vector, ref rotation, out vector2);
				destinationArray[destinationIndex + i] = vector2;
			}
		}

		// Token: 0x06003798 RID: 14232 RVA: 0x0006F3F0 File Offset: 0x0006D5F0
		public static Vector2 TransformNormal(Vector2 normal, Matrix matrix)
		{
			return new Vector2(normal.X * matrix.M11 + normal.Y * matrix.M21, normal.X * matrix.M12 + normal.Y * matrix.M22);
		}

		// Token: 0x06003799 RID: 14233 RVA: 0x0006F440 File Offset: 0x0006D640
		public static void TransformNormal(ref Vector2 normal, ref Matrix matrix, out Vector2 result)
		{
			float x = normal.X * matrix.M11 + normal.Y * matrix.M21;
			float y = normal.X * matrix.M12 + normal.Y * matrix.M22;
			result.X = x;
			result.Y = y;
		}

		// Token: 0x0600379A RID: 14234 RVA: 0x0006F494 File Offset: 0x0006D694
		public static void TransformNormal(Vector2[] sourceArray, ref Matrix matrix, Vector2[] destinationArray)
		{
			Vector2.TransformNormal(sourceArray, 0, ref matrix, destinationArray, 0, sourceArray.Length);
		}

		// Token: 0x0600379B RID: 14235 RVA: 0x0006F4A8 File Offset: 0x0006D6A8
		public static void TransformNormal(Vector2[] sourceArray, int sourceIndex, ref Matrix matrix, Vector2[] destinationArray, int destinationIndex, int length)
		{
			for (int i = 0; i < length; i++)
			{
				Vector2 vector = sourceArray[sourceIndex + i];
				Vector2 vector2;
				vector2.X = vector.X * matrix.M11 + vector.Y * matrix.M21;
				vector2.Y = vector.X * matrix.M12 + vector.Y * matrix.M22;
				destinationArray[destinationIndex + i] = vector2;
			}
		}

		// Token: 0x0600379C RID: 14236 RVA: 0x0006F524 File Offset: 0x0006D724
		public static Vector2 operator -(Vector2 value)
		{
			value.X = -value.X;
			value.Y = -value.Y;
			return value;
		}

		// Token: 0x0600379D RID: 14237 RVA: 0x0006F554 File Offset: 0x0006D754
		public static bool operator ==(Vector2 value1, Vector2 value2)
		{
			return value1.X == value2.X && value1.Y == value2.Y;
		}

		// Token: 0x0600379E RID: 14238 RVA: 0x0006F588 File Offset: 0x0006D788
		public static bool operator !=(Vector2 value1, Vector2 value2)
		{
			return !(value1 == value2);
		}

		// Token: 0x0600379F RID: 14239 RVA: 0x0006F5A4 File Offset: 0x0006D7A4
		public static Vector2 operator +(Vector2 value1, Vector2 value2)
		{
			value1.X += value2.X;
			value1.Y += value2.Y;
			return value1;
		}

		// Token: 0x060037A0 RID: 14240 RVA: 0x0006F5DC File Offset: 0x0006D7DC
		public static Vector2 operator -(Vector2 value1, Vector2 value2)
		{
			value1.X -= value2.X;
			value1.Y -= value2.Y;
			return value1;
		}

		// Token: 0x060037A1 RID: 14241 RVA: 0x0006F614 File Offset: 0x0006D814
		public static Vector2 operator *(Vector2 value1, Vector2 value2)
		{
			value1.X *= value2.X;
			value1.Y *= value2.Y;
			return value1;
		}

		// Token: 0x060037A2 RID: 14242 RVA: 0x0006F64C File Offset: 0x0006D84C
		public static Vector2 operator *(Vector2 value, float scaleFactor)
		{
			value.X *= scaleFactor;
			value.Y *= scaleFactor;
			return value;
		}

		// Token: 0x060037A3 RID: 14243 RVA: 0x0006F678 File Offset: 0x0006D878
		public static Vector2 operator *(float scaleFactor, Vector2 value)
		{
			value.X *= scaleFactor;
			value.Y *= scaleFactor;
			return value;
		}

		// Token: 0x060037A4 RID: 14244 RVA: 0x0006F6A4 File Offset: 0x0006D8A4
		public static Vector2 operator /(Vector2 value1, Vector2 value2)
		{
			value1.X /= value2.X;
			value1.Y /= value2.Y;
			return value1;
		}

		// Token: 0x060037A5 RID: 14245 RVA: 0x0006F6DC File Offset: 0x0006D8DC
		public static Vector2 operator /(Vector2 value1, float divider)
		{
			float num = 1f / divider;
			value1.X *= num;
			value1.Y *= num;
			return value1;
		}

		// Token: 0x060037A6 RID: 14246 RVA: 0x0006F710 File Offset: 0x0006D910
		public static Vector2 Spline(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
		{
			Vector2 result = default(Vector2);
			Vector2.Spline(ref t, ref p0, ref p1, ref p2, ref p3, out result);
			return result;
		}

		// Token: 0x060037A7 RID: 14247 RVA: 0x0006F740 File Offset: 0x0006D940
		public static void Spline(ref float t, ref Vector2 p0, ref Vector2 p1, ref Vector2 p2, ref Vector2 p3, out Vector2 result)
		{
			result.X = MathHelper.Spline(t, p0.X, p1.X, p2.X, p3.X);
			result.Y = MathHelper.Spline(t, p0.Y, p1.Y, p2.Y, p3.Y);
		}

		// Token: 0x060037A8 RID: 14248 RVA: 0x0006F79C File Offset: 0x0006D99C
		public static Vector2 Slerp(Vector2 p0, Vector2 p1, float t)
		{
			Vector2 result = default(Vector2);
			Vector2.Slerp(ref p0, ref p1, ref t, out result);
			return result;
		}

		// Token: 0x060037A9 RID: 14249 RVA: 0x0006F7C5 File Offset: 0x0006D9C5
		public static void Slerp(ref Vector2 p0, ref Vector2 p1, ref float t, out Vector2 result)
		{
			result.X = MathHelper.Slerp(p0.X, p1.X, t);
			result.Y = MathHelper.Slerp(p0.Y, p1.Y, t);
		}

		// Token: 0x17000FEB RID: 4075
		// (get) Token: 0x060037AA RID: 14250 RVA: 0x0006F7FC File Offset: 0x0006D9FC
		public static Vector2 NaN
		{
			get
			{
				return Vector2.nan;
			}
		}

		// Token: 0x060037AB RID: 14251 RVA: 0x0006F814 File Offset: 0x0006DA14
		public bool IsNaN()
		{
			return float.IsNaN(this.X) && float.IsNaN(this.Y);
		}

		// Token: 0x04001849 RID: 6217
		[CoherentProperty("x")]
		public float X;

		// Token: 0x0400184A RID: 6218
		[CoherentProperty("y")]
		public float Y;

		// Token: 0x0400184B RID: 6219
		private static readonly Vector2 zeroVector = new Vector2(0f, 0f);

		// Token: 0x0400184C RID: 6220
		private static readonly Vector2 unitVector = new Vector2(1f, 1f);

		// Token: 0x0400184D RID: 6221
		private static readonly Vector2 unitXVector = new Vector2(1f, 0f);

		// Token: 0x0400184E RID: 6222
		private static readonly Vector2 unitYVector = new Vector2(0f, 1f);

		// Token: 0x0400184F RID: 6223
		private static readonly Vector2 nan = new Vector2(float.NaN, float.NaN);
	}
}
