using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Coherent.UI.Binding;

namespace HytaleClient.Math
{
	// Token: 0x020007F9 RID: 2041
	[CoherentType]
	[DebuggerDisplay("{DebugDisplayString,nq}")]
	[Serializable]
	public struct Vector4 : IEquatable<Vector4>
	{
		// Token: 0x17000FFC RID: 4092
		// (get) Token: 0x06003820 RID: 14368 RVA: 0x00071C04 File Offset: 0x0006FE04
		public static Vector4 Zero
		{
			get
			{
				return Vector4.zero;
			}
		}

		// Token: 0x17000FFD RID: 4093
		// (get) Token: 0x06003821 RID: 14369 RVA: 0x00071C1C File Offset: 0x0006FE1C
		public static Vector4 One
		{
			get
			{
				return Vector4.unit;
			}
		}

		// Token: 0x17000FFE RID: 4094
		// (get) Token: 0x06003822 RID: 14370 RVA: 0x00071C34 File Offset: 0x0006FE34
		public static Vector4 UnitX
		{
			get
			{
				return Vector4.unitX;
			}
		}

		// Token: 0x17000FFF RID: 4095
		// (get) Token: 0x06003823 RID: 14371 RVA: 0x00071C4C File Offset: 0x0006FE4C
		public static Vector4 UnitY
		{
			get
			{
				return Vector4.unitY;
			}
		}

		// Token: 0x17001000 RID: 4096
		// (get) Token: 0x06003824 RID: 14372 RVA: 0x00071C64 File Offset: 0x0006FE64
		public static Vector4 UnitZ
		{
			get
			{
				return Vector4.unitZ;
			}
		}

		// Token: 0x17001001 RID: 4097
		// (get) Token: 0x06003825 RID: 14373 RVA: 0x00071C7C File Offset: 0x0006FE7C
		public static Vector4 UnitW
		{
			get
			{
				return Vector4.unitW;
			}
		}

		// Token: 0x17001002 RID: 4098
		// (get) Token: 0x06003826 RID: 14374 RVA: 0x00071C94 File Offset: 0x0006FE94
		internal string DebugDisplayString
		{
			get
			{
				return string.Concat(new string[]
				{
					this.X.ToString(),
					" ",
					this.Y.ToString(),
					" ",
					this.Z.ToString(),
					" ",
					this.W.ToString()
				});
			}
		}

		// Token: 0x06003827 RID: 14375 RVA: 0x00071D01 File Offset: 0x0006FF01
		public Vector4(float x, float y, float z, float w)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.W = w;
		}

		// Token: 0x06003828 RID: 14376 RVA: 0x00071D21 File Offset: 0x0006FF21
		public Vector4(Vector2 value, float z, float w)
		{
			this.X = value.X;
			this.Y = value.Y;
			this.Z = z;
			this.W = w;
		}

		// Token: 0x06003829 RID: 14377 RVA: 0x00071D4A File Offset: 0x0006FF4A
		public Vector4(Vector3 value, float w)
		{
			this.X = value.X;
			this.Y = value.Y;
			this.Z = value.Z;
			this.W = w;
		}

		// Token: 0x0600382A RID: 14378 RVA: 0x00071D78 File Offset: 0x0006FF78
		public Vector4(float value)
		{
			this.X = value;
			this.Y = value;
			this.Z = value;
			this.W = value;
		}

		// Token: 0x0600382B RID: 14379 RVA: 0x00071D98 File Offset: 0x0006FF98
		public override bool Equals(object obj)
		{
			return obj is Vector4 && this.Equals((Vector4)obj);
		}

		// Token: 0x0600382C RID: 14380 RVA: 0x00071DC4 File Offset: 0x0006FFC4
		public bool Equals(Vector4 other)
		{
			return this.X == other.X && this.Y == other.Y && this.Z == other.Z && this.W == other.W;
		}

		// Token: 0x0600382D RID: 14381 RVA: 0x00071E14 File Offset: 0x00070014
		public override int GetHashCode()
		{
			return (int)(this.W + this.X + this.Y + this.Y);
		}

		// Token: 0x0600382E RID: 14382 RVA: 0x00071E44 File Offset: 0x00070044
		public float Length()
		{
			float num;
			Vector4.DistanceSquared(ref this, ref Vector4.zero, out num);
			return (float)Math.Sqrt((double)num);
		}

		// Token: 0x0600382F RID: 14383 RVA: 0x00071E6C File Offset: 0x0007006C
		public float LengthSquared()
		{
			float result;
			Vector4.DistanceSquared(ref this, ref Vector4.zero, out result);
			return result;
		}

		// Token: 0x06003830 RID: 14384 RVA: 0x00071E8D File Offset: 0x0007008D
		public void Normalize()
		{
			Vector4.Normalize(ref this, out this);
		}

		// Token: 0x06003831 RID: 14385 RVA: 0x00071E98 File Offset: 0x00070098
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{X:",
				this.X.ToString(),
				" Y:",
				this.Y.ToString(),
				" Z:",
				this.Z.ToString(),
				" W:",
				this.W.ToString(),
				"}"
			});
		}

		// Token: 0x06003832 RID: 14386 RVA: 0x00071F18 File Offset: 0x00070118
		public static Vector4 Add(Vector4 value1, Vector4 value2)
		{
			value1.W += value2.W;
			value1.X += value2.X;
			value1.Y += value2.Y;
			value1.Z += value2.Z;
			return value1;
		}

		// Token: 0x06003833 RID: 14387 RVA: 0x00071F70 File Offset: 0x00070170
		public static void Add(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
		{
			result.W = value1.W + value2.W;
			result.X = value1.X + value2.X;
			result.Y = value1.Y + value2.Y;
			result.Z = value1.Z + value2.Z;
		}

		// Token: 0x06003834 RID: 14388 RVA: 0x00071FCC File Offset: 0x000701CC
		public static Vector4 Barycentric(Vector4 value1, Vector4 value2, Vector4 value3, float amount1, float amount2)
		{
			return new Vector4(MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2), MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2), MathHelper.Barycentric(value1.Z, value2.Z, value3.Z, amount1, amount2), MathHelper.Barycentric(value1.W, value2.W, value3.W, amount1, amount2));
		}

		// Token: 0x06003835 RID: 14389 RVA: 0x0007204C File Offset: 0x0007024C
		public static void Barycentric(ref Vector4 value1, ref Vector4 value2, ref Vector4 value3, float amount1, float amount2, out Vector4 result)
		{
			result.X = MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2);
			result.Y = MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2);
			result.Z = MathHelper.Barycentric(value1.Z, value2.Z, value3.Z, amount1, amount2);
			result.W = MathHelper.Barycentric(value1.W, value2.W, value3.W, amount1, amount2);
		}

		// Token: 0x06003836 RID: 14390 RVA: 0x000720E0 File Offset: 0x000702E0
		public static Vector4 CatmullRom(Vector4 value1, Vector4 value2, Vector4 value3, Vector4 value4, float amount)
		{
			return new Vector4(MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount), MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount), MathHelper.CatmullRom(value1.Z, value2.Z, value3.Z, value4.Z, amount), MathHelper.CatmullRom(value1.W, value2.W, value3.W, value4.W, amount));
		}

		// Token: 0x06003837 RID: 14391 RVA: 0x00072174 File Offset: 0x00070374
		public static void CatmullRom(ref Vector4 value1, ref Vector4 value2, ref Vector4 value3, ref Vector4 value4, float amount, out Vector4 result)
		{
			result.X = MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount);
			result.Y = MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount);
			result.Z = MathHelper.CatmullRom(value1.Z, value2.Z, value3.Z, value4.Z, amount);
			result.W = MathHelper.CatmullRom(value1.W, value2.W, value3.W, value4.W, amount);
		}

		// Token: 0x06003838 RID: 14392 RVA: 0x0007221C File Offset: 0x0007041C
		public static Vector4 Clamp(Vector4 value1, Vector4 min, Vector4 max)
		{
			return new Vector4(MathHelper.Clamp(value1.X, min.X, max.X), MathHelper.Clamp(value1.Y, min.Y, max.Y), MathHelper.Clamp(value1.Z, min.Z, max.Z), MathHelper.Clamp(value1.W, min.W, max.W));
		}

		// Token: 0x06003839 RID: 14393 RVA: 0x00072290 File Offset: 0x00070490
		public static void Clamp(ref Vector4 value1, ref Vector4 min, ref Vector4 max, out Vector4 result)
		{
			result.X = MathHelper.Clamp(value1.X, min.X, max.X);
			result.Y = MathHelper.Clamp(value1.Y, min.Y, max.Y);
			result.Z = MathHelper.Clamp(value1.Z, min.Z, max.Z);
			result.W = MathHelper.Clamp(value1.W, min.W, max.W);
		}

		// Token: 0x0600383A RID: 14394 RVA: 0x00072314 File Offset: 0x00070514
		public static float Distance(Vector4 value1, Vector4 value2)
		{
			return (float)Math.Sqrt((double)Vector4.DistanceSquared(value1, value2));
		}

		// Token: 0x0600383B RID: 14395 RVA: 0x00072334 File Offset: 0x00070534
		public static void Distance(ref Vector4 value1, ref Vector4 value2, out float result)
		{
			result = (float)Math.Sqrt((double)Vector4.DistanceSquared(value1, value2));
		}

		// Token: 0x0600383C RID: 14396 RVA: 0x00072354 File Offset: 0x00070554
		public static float DistanceSquared(Vector4 value1, Vector4 value2)
		{
			return (value1.W - value2.W) * (value1.W - value2.W) + (value1.X - value2.X) * (value1.X - value2.X) + (value1.Y - value2.Y) * (value1.Y - value2.Y) + (value1.Z - value2.Z) * (value1.Z - value2.Z);
		}

		// Token: 0x0600383D RID: 14397 RVA: 0x000723D8 File Offset: 0x000705D8
		public static void DistanceSquared(ref Vector4 value1, ref Vector4 value2, out float result)
		{
			result = (value1.W - value2.W) * (value1.W - value2.W) + (value1.X - value2.X) * (value1.X - value2.X) + (value1.Y - value2.Y) * (value1.Y - value2.Y) + (value1.Z - value2.Z) * (value1.Z - value2.Z);
		}

		// Token: 0x0600383E RID: 14398 RVA: 0x00072458 File Offset: 0x00070658
		public static Vector4 Divide(Vector4 value1, Vector4 value2)
		{
			value1.W /= value2.W;
			value1.X /= value2.X;
			value1.Y /= value2.Y;
			value1.Z /= value2.Z;
			return value1;
		}

		// Token: 0x0600383F RID: 14399 RVA: 0x000724B0 File Offset: 0x000706B0
		public static Vector4 Divide(Vector4 value1, float divider)
		{
			float num = 1f / divider;
			value1.W *= num;
			value1.X *= num;
			value1.Y *= num;
			value1.Z *= num;
			return value1;
		}

		// Token: 0x06003840 RID: 14400 RVA: 0x000724FC File Offset: 0x000706FC
		public static void Divide(ref Vector4 value1, float divider, out Vector4 result)
		{
			float num = 1f / divider;
			result.W = value1.W * num;
			result.X = value1.X * num;
			result.Y = value1.Y * num;
			result.Z = value1.Z * num;
		}

		// Token: 0x06003841 RID: 14401 RVA: 0x0007254C File Offset: 0x0007074C
		public static void Divide(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
		{
			result.W = value1.W / value2.W;
			result.X = value1.X / value2.X;
			result.Y = value1.Y / value2.Y;
			result.Z = value1.Z / value2.Z;
		}

		// Token: 0x06003842 RID: 14402 RVA: 0x000725A8 File Offset: 0x000707A8
		public static float Dot(Vector4 vector1, Vector4 vector2)
		{
			return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z + vector1.W * vector2.W;
		}

		// Token: 0x06003843 RID: 14403 RVA: 0x000725F1 File Offset: 0x000707F1
		public static void Dot(ref Vector4 vector1, ref Vector4 vector2, out float result)
		{
			result = vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z + vector1.W * vector2.W;
		}

		// Token: 0x06003844 RID: 14404 RVA: 0x00072630 File Offset: 0x00070830
		public static Vector4 Hermite(Vector4 value1, Vector4 tangent1, Vector4 value2, Vector4 tangent2, float amount)
		{
			return new Vector4(MathHelper.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount), MathHelper.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount), MathHelper.Hermite(value1.Z, tangent1.Z, value2.Z, tangent2.Z, amount), MathHelper.Hermite(value1.W, tangent1.W, value2.W, tangent2.W, amount));
		}

		// Token: 0x06003845 RID: 14405 RVA: 0x000726C4 File Offset: 0x000708C4
		public static void Hermite(ref Vector4 value1, ref Vector4 tangent1, ref Vector4 value2, ref Vector4 tangent2, float amount, out Vector4 result)
		{
			result.W = MathHelper.Hermite(value1.W, tangent1.W, value2.W, tangent2.W, amount);
			result.X = MathHelper.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount);
			result.Y = MathHelper.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount);
			result.Z = MathHelper.Hermite(value1.Z, tangent1.Z, value2.Z, tangent2.Z, amount);
		}

		// Token: 0x06003846 RID: 14406 RVA: 0x0007276C File Offset: 0x0007096C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 Lerp(Vector4 value1, Vector4 value2, float amount)
		{
			return new Vector4(MathHelper.Lerp(value1.X, value2.X, amount), MathHelper.Lerp(value1.Y, value2.Y, amount), MathHelper.Lerp(value1.Z, value2.Z, amount), MathHelper.Lerp(value1.W, value2.W, amount));
		}

		// Token: 0x06003847 RID: 14407 RVA: 0x000727CC File Offset: 0x000709CC
		public static void Lerp(ref Vector4 value1, ref Vector4 value2, float amount, out Vector4 result)
		{
			result.X = MathHelper.Lerp(value1.X, value2.X, amount);
			result.Y = MathHelper.Lerp(value1.Y, value2.Y, amount);
			result.Z = MathHelper.Lerp(value1.Z, value2.Z, amount);
			result.W = MathHelper.Lerp(value1.W, value2.W, amount);
		}

		// Token: 0x06003848 RID: 14408 RVA: 0x0007283C File Offset: 0x00070A3C
		public static Vector4 Max(Vector4 value1, Vector4 value2)
		{
			return new Vector4(MathHelper.Max(value1.X, value2.X), MathHelper.Max(value1.Y, value2.Y), MathHelper.Max(value1.Z, value2.Z), MathHelper.Max(value1.W, value2.W));
		}

		// Token: 0x06003849 RID: 14409 RVA: 0x00072898 File Offset: 0x00070A98
		public static void Max(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
		{
			result.X = MathHelper.Max(value1.X, value2.X);
			result.Y = MathHelper.Max(value1.Y, value2.Y);
			result.Z = MathHelper.Max(value1.Z, value2.Z);
			result.W = MathHelper.Max(value1.W, value2.W);
		}

		// Token: 0x0600384A RID: 14410 RVA: 0x00072904 File Offset: 0x00070B04
		public static Vector4 Min(Vector4 value1, Vector4 value2)
		{
			return new Vector4(MathHelper.Min(value1.X, value2.X), MathHelper.Min(value1.Y, value2.Y), MathHelper.Min(value1.Z, value2.Z), MathHelper.Min(value1.W, value2.W));
		}

		// Token: 0x0600384B RID: 14411 RVA: 0x00072960 File Offset: 0x00070B60
		public static void Min(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
		{
			result.X = MathHelper.Min(value1.X, value2.X);
			result.Y = MathHelper.Min(value1.Y, value2.Y);
			result.Z = MathHelper.Min(value1.Z, value2.Z);
			result.W = MathHelper.Min(value1.W, value2.W);
		}

		// Token: 0x0600384C RID: 14412 RVA: 0x000729CC File Offset: 0x00070BCC
		public static Vector4 Multiply(Vector4 value1, Vector4 value2)
		{
			value1.W *= value2.W;
			value1.X *= value2.X;
			value1.Y *= value2.Y;
			value1.Z *= value2.Z;
			return value1;
		}

		// Token: 0x0600384D RID: 14413 RVA: 0x00072A24 File Offset: 0x00070C24
		public static Vector4 Multiply(Vector4 value1, float scaleFactor)
		{
			value1.W *= scaleFactor;
			value1.X *= scaleFactor;
			value1.Y *= scaleFactor;
			value1.Z *= scaleFactor;
			return value1;
		}

		// Token: 0x0600384E RID: 14414 RVA: 0x00072A67 File Offset: 0x00070C67
		public static void Multiply(ref Vector4 value1, float scaleFactor, out Vector4 result)
		{
			result.W = value1.W * scaleFactor;
			result.X = value1.X * scaleFactor;
			result.Y = value1.Y * scaleFactor;
			result.Z = value1.Z * scaleFactor;
		}

		// Token: 0x0600384F RID: 14415 RVA: 0x00072AA4 File Offset: 0x00070CA4
		public static void Multiply(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
		{
			result.W = value1.W * value2.W;
			result.X = value1.X * value2.X;
			result.Y = value1.Y * value2.Y;
			result.Z = value1.Z * value2.Z;
		}

		// Token: 0x06003850 RID: 14416 RVA: 0x00072B00 File Offset: 0x00070D00
		public static Vector4 Negate(Vector4 value)
		{
			value = new Vector4(-value.X, -value.Y, -value.Z, -value.W);
			return value;
		}

		// Token: 0x06003851 RID: 14417 RVA: 0x00072B36 File Offset: 0x00070D36
		public static void Negate(ref Vector4 value, out Vector4 result)
		{
			result.X = -value.X;
			result.Y = -value.Y;
			result.Z = -value.Z;
			result.W = -value.W;
		}

		// Token: 0x06003852 RID: 14418 RVA: 0x00072B70 File Offset: 0x00070D70
		public static Vector4 Normalize(Vector4 vector)
		{
			Vector4.Normalize(ref vector, out vector);
			return vector;
		}

		// Token: 0x06003853 RID: 14419 RVA: 0x00072B90 File Offset: 0x00070D90
		public static void Normalize(ref Vector4 vector, out Vector4 result)
		{
			float num;
			Vector4.DistanceSquared(ref vector, ref Vector4.zero, out num);
			num = 1f / (float)Math.Sqrt((double)num);
			result.W = vector.W * num;
			result.X = vector.X * num;
			result.Y = vector.Y * num;
			result.Z = vector.Z * num;
		}

		// Token: 0x06003854 RID: 14420 RVA: 0x00072BF4 File Offset: 0x00070DF4
		public static Vector4 SmoothStep(Vector4 value1, Vector4 value2, float amount)
		{
			return new Vector4(MathHelper.SmoothStep(value1.X, value2.X, amount), MathHelper.SmoothStep(value1.Y, value2.Y, amount), MathHelper.SmoothStep(value1.Z, value2.Z, amount), MathHelper.SmoothStep(value1.W, value2.W, amount));
		}

		// Token: 0x06003855 RID: 14421 RVA: 0x00072C54 File Offset: 0x00070E54
		public static void SmoothStep(ref Vector4 value1, ref Vector4 value2, float amount, out Vector4 result)
		{
			result.X = MathHelper.SmoothStep(value1.X, value2.X, amount);
			result.Y = MathHelper.SmoothStep(value1.Y, value2.Y, amount);
			result.Z = MathHelper.SmoothStep(value1.Z, value2.Z, amount);
			result.W = MathHelper.SmoothStep(value1.W, value2.W, amount);
		}

		// Token: 0x06003856 RID: 14422 RVA: 0x00072CC4 File Offset: 0x00070EC4
		public static Vector4 Subtract(Vector4 value1, Vector4 value2)
		{
			value1.W -= value2.W;
			value1.X -= value2.X;
			value1.Y -= value2.Y;
			value1.Z -= value2.Z;
			return value1;
		}

		// Token: 0x06003857 RID: 14423 RVA: 0x00072D1C File Offset: 0x00070F1C
		public static void Subtract(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
		{
			result.W = value1.W - value2.W;
			result.X = value1.X - value2.X;
			result.Y = value1.Y - value2.Y;
			result.Z = value1.Z - value2.Z;
		}

		// Token: 0x06003858 RID: 14424 RVA: 0x00072D78 File Offset: 0x00070F78
		public static Vector4 Transform(Vector2 position, Matrix matrix)
		{
			Vector4 result;
			Vector4.Transform(ref position, ref matrix, out result);
			return result;
		}

		// Token: 0x06003859 RID: 14425 RVA: 0x00072D98 File Offset: 0x00070F98
		public static Vector4 Transform(Vector3 position, Matrix matrix)
		{
			Vector4 result;
			Vector4.Transform(ref position, ref matrix, out result);
			return result;
		}

		// Token: 0x0600385A RID: 14426 RVA: 0x00072DB8 File Offset: 0x00070FB8
		public static Vector4 Transform(Vector4 vector, Matrix matrix)
		{
			Vector4.Transform(ref vector, ref matrix, out vector);
			return vector;
		}

		// Token: 0x0600385B RID: 14427 RVA: 0x00072DD8 File Offset: 0x00070FD8
		public static void Transform(ref Vector2 position, ref Matrix matrix, out Vector4 result)
		{
			result = new Vector4(position.X * matrix.M11 + position.Y * matrix.M21 + matrix.M41, position.X * matrix.M12 + position.Y * matrix.M22 + matrix.M42, position.X * matrix.M13 + position.Y * matrix.M23 + matrix.M43, position.X * matrix.M14 + position.Y * matrix.M24 + matrix.M44);
		}

		// Token: 0x0600385C RID: 14428 RVA: 0x00072E7C File Offset: 0x0007107C
		public static void Transform(ref Vector3 position, ref Matrix matrix, out Vector4 result)
		{
			float x = position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41;
			float y = position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42;
			float z = position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43;
			float w = position.X * matrix.M14 + position.Y * matrix.M24 + position.Z * matrix.M34 + matrix.M44;
			result.X = x;
			result.Y = y;
			result.Z = z;
			result.W = w;
		}

		// Token: 0x0600385D RID: 14429 RVA: 0x00072F6C File Offset: 0x0007116C
		public static void Transform(ref Vector4 vector, ref Matrix matrix, out Vector4 result)
		{
			float x = vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31 + vector.W * matrix.M41;
			float y = vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32 + vector.W * matrix.M42;
			float z = vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33 + vector.W * matrix.M43;
			float w = vector.X * matrix.M14 + vector.Y * matrix.M24 + vector.Z * matrix.M34 + vector.W * matrix.M44;
			result.X = x;
			result.Y = y;
			result.Z = z;
			result.W = w;
		}

		// Token: 0x0600385E RID: 14430 RVA: 0x00073078 File Offset: 0x00071278
		public static void Transform(Vector4[] sourceArray, ref Matrix matrix, Vector4[] destinationArray)
		{
			bool flag = sourceArray == null;
			if (flag)
			{
				throw new ArgumentNullException("sourceArray");
			}
			bool flag2 = destinationArray == null;
			if (flag2)
			{
				throw new ArgumentNullException("destinationArray");
			}
			bool flag3 = destinationArray.Length < sourceArray.Length;
			if (flag3)
			{
				throw new ArgumentException("destinationArray is too small to contain the result.");
			}
			for (int i = 0; i < sourceArray.Length; i++)
			{
				Vector4.Transform(ref sourceArray[i], ref matrix, out destinationArray[i]);
			}
		}

		// Token: 0x0600385F RID: 14431 RVA: 0x000730F4 File Offset: 0x000712F4
		public static void Transform(Vector4[] sourceArray, int sourceIndex, ref Matrix matrix, Vector4[] destinationArray, int destinationIndex, int length)
		{
			bool flag = sourceArray == null;
			if (flag)
			{
				throw new ArgumentNullException("sourceArray");
			}
			bool flag2 = destinationArray == null;
			if (flag2)
			{
				throw new ArgumentNullException("destinationArray");
			}
			bool flag3 = destinationIndex + length > destinationArray.Length;
			if (flag3)
			{
				throw new ArgumentException("destinationArray is too small to contain the result.");
			}
			bool flag4 = sourceIndex + length > sourceArray.Length;
			if (flag4)
			{
				throw new ArgumentException("The combination of sourceIndex and length was greater than sourceArray.Length.");
			}
			for (int i = 0; i < length; i++)
			{
				Vector4.Transform(ref sourceArray[i + sourceIndex], ref matrix, out destinationArray[i + destinationIndex]);
			}
		}

		// Token: 0x06003860 RID: 14432 RVA: 0x00073194 File Offset: 0x00071394
		public static Vector4 Transform(Vector2 value, Quaternion rotation)
		{
			Vector4 result;
			Vector4.Transform(ref value, ref rotation, out result);
			return result;
		}

		// Token: 0x06003861 RID: 14433 RVA: 0x000731B4 File Offset: 0x000713B4
		public static Vector4 Transform(Vector3 value, Quaternion rotation)
		{
			Vector4 result;
			Vector4.Transform(ref value, ref rotation, out result);
			return result;
		}

		// Token: 0x06003862 RID: 14434 RVA: 0x000731D4 File Offset: 0x000713D4
		public static Vector4 Transform(Vector4 value, Quaternion rotation)
		{
			Vector4 result;
			Vector4.Transform(ref value, ref rotation, out result);
			return result;
		}

		// Token: 0x06003863 RID: 14435 RVA: 0x000731F4 File Offset: 0x000713F4
		public static void Transform(ref Vector2 value, ref Quaternion rotation, out Vector4 result)
		{
			double num = (double)(rotation.X + rotation.X);
			double num2 = (double)(rotation.Y + rotation.Y);
			double num3 = (double)(rotation.Z + rotation.Z);
			double num4 = (double)rotation.W * num;
			double num5 = (double)rotation.W * num2;
			double num6 = (double)rotation.W * num3;
			double num7 = (double)rotation.X * num;
			double num8 = (double)rotation.X * num2;
			double num9 = (double)rotation.X * num3;
			double num10 = (double)rotation.Y * num2;
			double num11 = (double)rotation.Y * num3;
			double num12 = (double)rotation.Z * num3;
			result.X = (float)((double)value.X * (1.0 - num10 - num12) + (double)value.Y * (num8 - num6));
			result.Y = (float)((double)value.X * (num8 + num6) + (double)value.Y * (1.0 - num7 - num12));
			result.Z = (float)((double)value.X * (num9 - num5) + (double)value.Y * (num11 + num4));
			result.W = 1f;
		}

		// Token: 0x06003864 RID: 14436 RVA: 0x00073318 File Offset: 0x00071518
		public static void Transform(ref Vector3 value, ref Quaternion rotation, out Vector4 result)
		{
			double num = (double)(rotation.X + rotation.X);
			double num2 = (double)(rotation.Y + rotation.Y);
			double num3 = (double)(rotation.Z + rotation.Z);
			double num4 = (double)rotation.W * num;
			double num5 = (double)rotation.W * num2;
			double num6 = (double)rotation.W * num3;
			double num7 = (double)rotation.X * num;
			double num8 = (double)rotation.X * num2;
			double num9 = (double)rotation.X * num3;
			double num10 = (double)rotation.Y * num2;
			double num11 = (double)rotation.Y * num3;
			double num12 = (double)rotation.Z * num3;
			result.X = (float)((double)value.X * (1.0 - num10 - num12) + (double)value.Y * (num8 - num6) + (double)value.Z * (num9 + num5));
			result.Y = (float)((double)value.X * (num8 + num6) + (double)value.Y * (1.0 - num7 - num12) + (double)value.Z * (num11 - num4));
			result.Z = (float)((double)value.X * (num9 - num5) + (double)value.Y * (num11 + num4) + (double)value.Z * (1.0 - num7 - num10));
			result.W = 1f;
		}

		// Token: 0x06003865 RID: 14437 RVA: 0x0007346C File Offset: 0x0007166C
		public static void Transform(ref Vector4 value, ref Quaternion rotation, out Vector4 result)
		{
			double num = (double)(rotation.X + rotation.X);
			double num2 = (double)(rotation.Y + rotation.Y);
			double num3 = (double)(rotation.Z + rotation.Z);
			double num4 = (double)rotation.W * num;
			double num5 = (double)rotation.W * num2;
			double num6 = (double)rotation.W * num3;
			double num7 = (double)rotation.X * num;
			double num8 = (double)rotation.X * num2;
			double num9 = (double)rotation.X * num3;
			double num10 = (double)rotation.Y * num2;
			double num11 = (double)rotation.Y * num3;
			double num12 = (double)rotation.Z * num3;
			result.X = (float)((double)value.X * (1.0 - num10 - num12) + (double)value.Y * (num8 - num6) + (double)value.Z * (num9 + num5));
			result.Y = (float)((double)value.X * (num8 + num6) + (double)value.Y * (1.0 - num7 - num12) + (double)value.Z * (num11 - num4));
			result.Z = (float)((double)value.X * (num9 - num5) + (double)value.Y * (num11 + num4) + (double)value.Z * (1.0 - num7 - num10));
			result.W = value.W;
		}

		// Token: 0x06003866 RID: 14438 RVA: 0x000735C4 File Offset: 0x000717C4
		public static void Transform(Vector4[] sourceArray, ref Quaternion rotation, Vector4[] destinationArray)
		{
			bool flag = sourceArray == null;
			if (flag)
			{
				throw new ArgumentException("sourceArray");
			}
			bool flag2 = destinationArray == null;
			if (flag2)
			{
				throw new ArgumentException("destinationArray");
			}
			bool flag3 = destinationArray.Length < sourceArray.Length;
			if (flag3)
			{
				throw new ArgumentException("destinationArray is too small to contain the result.");
			}
			for (int i = 0; i < sourceArray.Length; i++)
			{
				Vector4.Transform(ref sourceArray[i], ref rotation, out destinationArray[i]);
			}
		}

		// Token: 0x06003867 RID: 14439 RVA: 0x00073640 File Offset: 0x00071840
		public static void Transform(Vector4[] sourceArray, int sourceIndex, ref Quaternion rotation, Vector4[] destinationArray, int destinationIndex, int length)
		{
			bool flag = sourceArray == null;
			if (flag)
			{
				throw new ArgumentException("sourceArray");
			}
			bool flag2 = destinationArray == null;
			if (flag2)
			{
				throw new ArgumentException("destinationArray");
			}
			bool flag3 = destinationIndex + length > destinationArray.Length;
			if (flag3)
			{
				throw new ArgumentException("destinationArray is too small to contain the result.");
			}
			bool flag4 = sourceIndex + length > sourceArray.Length;
			if (flag4)
			{
				throw new ArgumentException("The combination of sourceIndex and length was greater than sourceArray.Length.");
			}
			for (int i = 0; i < length; i++)
			{
				Vector4.Transform(ref sourceArray[i + sourceIndex], ref rotation, out destinationArray[i + destinationIndex]);
			}
		}

		// Token: 0x06003868 RID: 14440 RVA: 0x000736E0 File Offset: 0x000718E0
		public static Vector4 operator -(Vector4 value)
		{
			return new Vector4(-value.X, -value.Y, -value.Z, -value.W);
		}

		// Token: 0x06003869 RID: 14441 RVA: 0x00073714 File Offset: 0x00071914
		public static bool operator ==(Vector4 value1, Vector4 value2)
		{
			return value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z && value1.W == value2.W;
		}

		// Token: 0x0600386A RID: 14442 RVA: 0x00073764 File Offset: 0x00071964
		public static bool operator !=(Vector4 value1, Vector4 value2)
		{
			return !(value1 == value2);
		}

		// Token: 0x0600386B RID: 14443 RVA: 0x00073780 File Offset: 0x00071980
		public static Vector4 operator +(Vector4 value1, Vector4 value2)
		{
			value1.W += value2.W;
			value1.X += value2.X;
			value1.Y += value2.Y;
			value1.Z += value2.Z;
			return value1;
		}

		// Token: 0x0600386C RID: 14444 RVA: 0x000737D8 File Offset: 0x000719D8
		public static Vector4 operator -(Vector4 value1, Vector4 value2)
		{
			value1.W -= value2.W;
			value1.X -= value2.X;
			value1.Y -= value2.Y;
			value1.Z -= value2.Z;
			return value1;
		}

		// Token: 0x0600386D RID: 14445 RVA: 0x00073830 File Offset: 0x00071A30
		public static Vector4 operator *(Vector4 value1, Vector4 value2)
		{
			value1.W *= value2.W;
			value1.X *= value2.X;
			value1.Y *= value2.Y;
			value1.Z *= value2.Z;
			return value1;
		}

		// Token: 0x0600386E RID: 14446 RVA: 0x00073888 File Offset: 0x00071A88
		public static Vector4 operator *(Vector4 value1, float scaleFactor)
		{
			value1.W *= scaleFactor;
			value1.X *= scaleFactor;
			value1.Y *= scaleFactor;
			value1.Z *= scaleFactor;
			return value1;
		}

		// Token: 0x0600386F RID: 14447 RVA: 0x000738CC File Offset: 0x00071ACC
		public static Vector4 operator *(float scaleFactor, Vector4 value1)
		{
			value1.W *= scaleFactor;
			value1.X *= scaleFactor;
			value1.Y *= scaleFactor;
			value1.Z *= scaleFactor;
			return value1;
		}

		// Token: 0x06003870 RID: 14448 RVA: 0x00073910 File Offset: 0x00071B10
		public static Vector4 operator /(Vector4 value1, Vector4 value2)
		{
			value1.W /= value2.W;
			value1.X /= value2.X;
			value1.Y /= value2.Y;
			value1.Z /= value2.Z;
			return value1;
		}

		// Token: 0x06003871 RID: 14449 RVA: 0x00073968 File Offset: 0x00071B68
		public static Vector4 operator /(Vector4 value1, float divider)
		{
			float num = 1f / divider;
			value1.W *= num;
			value1.X *= num;
			value1.Y *= num;
			value1.Z *= num;
			return value1;
		}

		// Token: 0x06003872 RID: 14450 RVA: 0x000739B3 File Offset: 0x00071BB3
		public Vector4(Vector3 vector)
		{
			this.X = vector.X;
			this.Y = vector.Y;
			this.Z = vector.Z;
			this.W = 0f;
		}

		// Token: 0x06003873 RID: 14451 RVA: 0x000739E5 File Offset: 0x00071BE5
		public Vector4(Vector2 vectorA, Vector2 vectorB)
		{
			this.X = vectorA.X;
			this.Y = vectorA.Y;
			this.Z = vectorB.X;
			this.W = vectorB.Y;
		}

		// Token: 0x06003874 RID: 14452 RVA: 0x00073A18 File Offset: 0x00071C18
		public bool IsInsideFrustum()
		{
			return Math.Abs(this.X) <= Math.Abs(this.W) && Math.Abs(this.Y) <= Math.Abs(this.W) && Math.Abs(this.Z) <= Math.Abs(this.W);
		}

		// Token: 0x06003875 RID: 14453 RVA: 0x00073A78 File Offset: 0x00071C78
		public Vector4 PerspectiveTransform()
		{
			float num = 1f / this.W;
			this.X *= num;
			this.Y *= num;
			this.Z *= num;
			this.W = 1f;
			return this;
		}

		// Token: 0x06003876 RID: 14454 RVA: 0x00073AD4 File Offset: 0x00071CD4
		public float Get(int index)
		{
			float result;
			switch (index)
			{
			case 0:
				result = this.X;
				break;
			case 1:
				result = this.Y;
				break;
			case 2:
				result = this.Z;
				break;
			case 3:
				result = this.W;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return result;
		}

		// Token: 0x0400185F RID: 6239
		[CoherentProperty("x")]
		public float X;

		// Token: 0x04001860 RID: 6240
		[CoherentProperty("y")]
		public float Y;

		// Token: 0x04001861 RID: 6241
		[CoherentProperty("z")]
		public float Z;

		// Token: 0x04001862 RID: 6242
		[CoherentProperty("w")]
		public float W;

		// Token: 0x04001863 RID: 6243
		private static Vector4 zero = default(Vector4);

		// Token: 0x04001864 RID: 6244
		private static readonly Vector4 unit = new Vector4(1f, 1f, 1f, 1f);

		// Token: 0x04001865 RID: 6245
		private static readonly Vector4 unitX = new Vector4(1f, 0f, 0f, 0f);

		// Token: 0x04001866 RID: 6246
		private static readonly Vector4 unitY = new Vector4(0f, 1f, 0f, 0f);

		// Token: 0x04001867 RID: 6247
		private static readonly Vector4 unitZ = new Vector4(0f, 0f, 1f, 0f);

		// Token: 0x04001868 RID: 6248
		private static readonly Vector4 unitW = new Vector4(0f, 0f, 0f, 1f);
	}
}
