using System;
using System.Diagnostics;

namespace HytaleClient.Math
{
	// Token: 0x020007F2 RID: 2034
	[DebuggerDisplay("{DebugDisplayString,nq}")]
	[Serializable]
	public struct Quaternion : IEquatable<Quaternion>
	{
		// Token: 0x17000FDA RID: 4058
		// (get) Token: 0x060036ED RID: 14061 RVA: 0x0006BF38 File Offset: 0x0006A138
		public static Quaternion Identity
		{
			get
			{
				return Quaternion.identity;
			}
		}

		// Token: 0x17000FDB RID: 4059
		// (get) Token: 0x060036EE RID: 14062 RVA: 0x0006BF50 File Offset: 0x0006A150
		internal string DebugDisplayString
		{
			get
			{
				bool flag = this == Quaternion.Identity;
				string result;
				if (flag)
				{
					result = "Identity";
				}
				else
				{
					result = string.Concat(new string[]
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
				return result;
			}
		}

		// Token: 0x060036EF RID: 14063 RVA: 0x0006BFDA File Offset: 0x0006A1DA
		public Quaternion(float x, float y, float z, float w)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.W = w;
		}

		// Token: 0x060036F0 RID: 14064 RVA: 0x0006BFFA File Offset: 0x0006A1FA
		public Quaternion(Vector3 vectorPart, float scalarPart)
		{
			this.X = vectorPart.X;
			this.Y = vectorPart.Y;
			this.Z = vectorPart.Z;
			this.W = scalarPart;
		}

		// Token: 0x060036F1 RID: 14065 RVA: 0x0006C028 File Offset: 0x0006A228
		public void Conjugate()
		{
			this.X = -this.X;
			this.Y = -this.Y;
			this.Z = -this.Z;
		}

		// Token: 0x060036F2 RID: 14066 RVA: 0x0006C054 File Offset: 0x0006A254
		public override bool Equals(object obj)
		{
			return obj is Quaternion && this.Equals((Quaternion)obj);
		}

		// Token: 0x060036F3 RID: 14067 RVA: 0x0006C080 File Offset: 0x0006A280
		public bool Equals(Quaternion other)
		{
			return this.X == other.X && this.Y == other.Y && this.Z == other.Z && this.W == other.W;
		}

		// Token: 0x060036F4 RID: 14068 RVA: 0x0006C0D0 File Offset: 0x0006A2D0
		public override int GetHashCode()
		{
			return this.X.GetHashCode() + this.Y.GetHashCode() + this.Z.GetHashCode() + this.W.GetHashCode();
		}

		// Token: 0x060036F5 RID: 14069 RVA: 0x0006C114 File Offset: 0x0006A314
		public float Length()
		{
			float num = this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W;
			return (float)Math.Sqrt((double)num);
		}

		// Token: 0x060036F6 RID: 14070 RVA: 0x0006C168 File Offset: 0x0006A368
		public float LengthSquared()
		{
			return this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W;
		}

		// Token: 0x060036F7 RID: 14071 RVA: 0x0006C1B4 File Offset: 0x0006A3B4
		public void Normalize()
		{
			float num = 1f / (float)Math.Sqrt((double)(this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W));
			this.X *= num;
			this.Y *= num;
			this.Z *= num;
			this.W *= num;
		}

		// Token: 0x060036F8 RID: 14072 RVA: 0x0006C240 File Offset: 0x0006A440
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

		// Token: 0x060036F9 RID: 14073 RVA: 0x0006C2C0 File Offset: 0x0006A4C0
		public static Quaternion Add(Quaternion quaternion1, Quaternion quaternion2)
		{
			Quaternion result;
			Quaternion.Add(ref quaternion1, ref quaternion2, out result);
			return result;
		}

		// Token: 0x060036FA RID: 14074 RVA: 0x0006C2E0 File Offset: 0x0006A4E0
		public static void Add(ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
		{
			result.X = quaternion1.X + quaternion2.X;
			result.Y = quaternion1.Y + quaternion2.Y;
			result.Z = quaternion1.Z + quaternion2.Z;
			result.W = quaternion1.W + quaternion2.W;
		}

		// Token: 0x060036FB RID: 14075 RVA: 0x0006C33C File Offset: 0x0006A53C
		public static Quaternion Concatenate(Quaternion value1, Quaternion value2)
		{
			Quaternion result;
			Quaternion.Concatenate(ref value1, ref value2, out result);
			return result;
		}

		// Token: 0x060036FC RID: 14076 RVA: 0x0006C35C File Offset: 0x0006A55C
		public static void Concatenate(ref Quaternion value1, ref Quaternion value2, out Quaternion result)
		{
			float x = value1.X;
			float y = value1.Y;
			float z = value1.Z;
			float w = value1.W;
			float x2 = value2.X;
			float y2 = value2.Y;
			float z2 = value2.Z;
			float w2 = value2.W;
			result.X = x2 * w + x * w2 + (y2 * z - z2 * y);
			result.Y = y2 * w + y * w2 + (z2 * x - x2 * z);
			result.Z = z2 * w + z * w2 + (x2 * y - y2 * x);
			result.W = w2 * w - (x2 * x + y2 * y + z2 * z);
		}

		// Token: 0x060036FD RID: 14077 RVA: 0x0006C40C File Offset: 0x0006A60C
		public static Quaternion Conjugate(Quaternion value)
		{
			return new Quaternion(-value.X, -value.Y, -value.Z, value.W);
		}

		// Token: 0x060036FE RID: 14078 RVA: 0x0006C43E File Offset: 0x0006A63E
		public static void Conjugate(ref Quaternion value, out Quaternion result)
		{
			result.X = -value.X;
			result.Y = -value.Y;
			result.Z = -value.Z;
			result.W = value.W;
		}

		// Token: 0x060036FF RID: 14079 RVA: 0x0006C474 File Offset: 0x0006A674
		public static Quaternion CreateFromAxisAngle(Vector3 axis, float angle)
		{
			Quaternion result;
			Quaternion.CreateFromAxisAngle(ref axis, angle, out result);
			return result;
		}

		// Token: 0x06003700 RID: 14080 RVA: 0x0006C494 File Offset: 0x0006A694
		public static void CreateFromAxisAngle(ref Vector3 axis, float angle, out Quaternion result)
		{
			float num = angle * 0.5f;
			float num2 = (float)Math.Sin((double)num);
			float w = (float)Math.Cos((double)num);
			result.X = axis.X * num2;
			result.Y = axis.Y * num2;
			result.Z = axis.Z * num2;
			result.W = w;
		}

		// Token: 0x06003701 RID: 14081 RVA: 0x0006C4F0 File Offset: 0x0006A6F0
		public static Quaternion CreateFromRotationMatrix(Matrix matrix)
		{
			Quaternion result;
			Quaternion.CreateFromRotationMatrix(ref matrix, out result);
			return result;
		}

		// Token: 0x06003702 RID: 14082 RVA: 0x0006C510 File Offset: 0x0006A710
		public static void CreateFromRotationMatrix(ref Matrix matrix, out Quaternion result)
		{
			float num = matrix.M11 + matrix.M22 + matrix.M33;
			bool flag = num > 0f;
			if (flag)
			{
				float num2 = (float)Math.Sqrt((double)(num + 1f));
				result.W = num2 * 0.5f;
				num2 = 0.5f / num2;
				result.X = (matrix.M23 - matrix.M32) * num2;
				result.Y = (matrix.M31 - matrix.M13) * num2;
				result.Z = (matrix.M12 - matrix.M21) * num2;
			}
			else
			{
				bool flag2 = matrix.M11 >= matrix.M22 && matrix.M11 >= matrix.M33;
				if (flag2)
				{
					float num2 = (float)Math.Sqrt((double)(1f + matrix.M11 - matrix.M22 - matrix.M33));
					float num3 = 0.5f / num2;
					result.X = 0.5f * num2;
					result.Y = (matrix.M12 + matrix.M21) * num3;
					result.Z = (matrix.M13 + matrix.M31) * num3;
					result.W = (matrix.M23 - matrix.M32) * num3;
				}
				else
				{
					bool flag3 = matrix.M22 > matrix.M33;
					if (flag3)
					{
						float num2 = (float)Math.Sqrt((double)(1f + matrix.M22 - matrix.M11 - matrix.M33));
						float num3 = 0.5f / num2;
						result.X = (matrix.M21 + matrix.M12) * num3;
						result.Y = 0.5f * num2;
						result.Z = (matrix.M32 + matrix.M23) * num3;
						result.W = (matrix.M31 - matrix.M13) * num3;
					}
					else
					{
						float num2 = (float)Math.Sqrt((double)(1f + matrix.M33 - matrix.M11 - matrix.M22));
						float num3 = 0.5f / num2;
						result.X = (matrix.M31 + matrix.M13) * num3;
						result.Y = (matrix.M32 + matrix.M23) * num3;
						result.Z = 0.5f * num2;
						result.W = (matrix.M12 - matrix.M21) * num3;
					}
				}
			}
		}

		// Token: 0x06003703 RID: 14083 RVA: 0x0006C754 File Offset: 0x0006A954
		public static Quaternion CreateFromYawPitchRoll(float yaw, float pitch, float roll)
		{
			Quaternion result;
			Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll, out result);
			return result;
		}

		// Token: 0x06003704 RID: 14084 RVA: 0x0006C774 File Offset: 0x0006A974
		public static void CreateFromYawPitchRoll(float yaw, float pitch, float roll, out Quaternion result)
		{
			float num = roll * 0.5f;
			float num2 = (float)Math.Sin((double)num);
			float num3 = (float)Math.Cos((double)num);
			float num4 = pitch * 0.5f;
			float num5 = (float)Math.Sin((double)num4);
			float num6 = (float)Math.Cos((double)num4);
			float num7 = yaw * 0.5f;
			float num8 = (float)Math.Sin((double)num7);
			float num9 = (float)Math.Cos((double)num7);
			result.X = num9 * num5 * num3 + num8 * num6 * num2;
			result.Y = num8 * num6 * num3 - num9 * num5 * num2;
			result.Z = num9 * num6 * num2 - num8 * num5 * num3;
			result.W = num9 * num6 * num3 + num8 * num5 * num2;
		}

		// Token: 0x06003705 RID: 14085 RVA: 0x0006C82C File Offset: 0x0006AA2C
		public static Quaternion Divide(Quaternion quaternion1, Quaternion quaternion2)
		{
			Quaternion result;
			Quaternion.Divide(ref quaternion1, ref quaternion2, out result);
			return result;
		}

		// Token: 0x06003706 RID: 14086 RVA: 0x0006C84C File Offset: 0x0006AA4C
		public static void Divide(ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
		{
			float x = quaternion1.X;
			float y = quaternion1.Y;
			float z = quaternion1.Z;
			float w = quaternion1.W;
			float num = quaternion2.X * quaternion2.X + quaternion2.Y * quaternion2.Y + quaternion2.Z * quaternion2.Z + quaternion2.W * quaternion2.W;
			float num2 = 1f / num;
			float num3 = -quaternion2.X * num2;
			float num4 = -quaternion2.Y * num2;
			float num5 = -quaternion2.Z * num2;
			float num6 = quaternion2.W * num2;
			float num7 = y * num5 - z * num4;
			float num8 = z * num3 - x * num5;
			float num9 = x * num4 - y * num3;
			float num10 = x * num3 + y * num4 + z * num5;
			result.X = x * num6 + num3 * w + num7;
			result.Y = y * num6 + num4 * w + num8;
			result.Z = z * num6 + num5 * w + num9;
			result.W = w * num6 - num10;
		}

		// Token: 0x06003707 RID: 14087 RVA: 0x0006C95C File Offset: 0x0006AB5C
		public static float Dot(Quaternion quaternion1, Quaternion quaternion2)
		{
			return quaternion1.X * quaternion2.X + quaternion1.Y * quaternion2.Y + quaternion1.Z * quaternion2.Z + quaternion1.W * quaternion2.W;
		}

		// Token: 0x06003708 RID: 14088 RVA: 0x0006C9A5 File Offset: 0x0006ABA5
		public static void Dot(ref Quaternion quaternion1, ref Quaternion quaternion2, out float result)
		{
			result = quaternion1.X * quaternion2.X + quaternion1.Y * quaternion2.Y + quaternion1.Z * quaternion2.Z + quaternion1.W * quaternion2.W;
		}

		// Token: 0x06003709 RID: 14089 RVA: 0x0006C9E4 File Offset: 0x0006ABE4
		public static Quaternion Inverse(Quaternion quaternion)
		{
			Quaternion result;
			Quaternion.Inverse(ref quaternion, out result);
			return result;
		}

		// Token: 0x0600370A RID: 14090 RVA: 0x0006CA04 File Offset: 0x0006AC04
		public static void Inverse(ref Quaternion quaternion, out Quaternion result)
		{
			float num = quaternion.X * quaternion.X + quaternion.Y * quaternion.Y + quaternion.Z * quaternion.Z + quaternion.W * quaternion.W;
			float num2 = 1f / num;
			result.X = -quaternion.X * num2;
			result.Y = -quaternion.Y * num2;
			result.Z = -quaternion.Z * num2;
			result.W = quaternion.W * num2;
		}

		// Token: 0x0600370B RID: 14091 RVA: 0x0006CA90 File Offset: 0x0006AC90
		public static Quaternion Lerp(Quaternion quaternion1, Quaternion quaternion2, float amount)
		{
			Quaternion result;
			Quaternion.Lerp(ref quaternion1, ref quaternion2, amount, out result);
			return result;
		}

		// Token: 0x0600370C RID: 14092 RVA: 0x0006CAB0 File Offset: 0x0006ACB0
		public static void Lerp(ref Quaternion quaternion1, ref Quaternion quaternion2, float amount, out Quaternion result)
		{
			float num = 1f - amount;
			float num2 = quaternion1.X * quaternion2.X + quaternion1.Y * quaternion2.Y + quaternion1.Z * quaternion2.Z + quaternion1.W * quaternion2.W;
			bool flag = num2 >= 0f;
			if (flag)
			{
				result.X = num * quaternion1.X + amount * quaternion2.X;
				result.Y = num * quaternion1.Y + amount * quaternion2.Y;
				result.Z = num * quaternion1.Z + amount * quaternion2.Z;
				result.W = num * quaternion1.W + amount * quaternion2.W;
			}
			else
			{
				result.X = num * quaternion1.X - amount * quaternion2.X;
				result.Y = num * quaternion1.Y - amount * quaternion2.Y;
				result.Z = num * quaternion1.Z - amount * quaternion2.Z;
				result.W = num * quaternion1.W - amount * quaternion2.W;
			}
			float num3 = result.X * result.X + result.Y * result.Y + result.Z * result.Z + result.W * result.W;
			float num4 = 1f / (float)Math.Sqrt((double)num3);
			result.X *= num4;
			result.Y *= num4;
			result.Z *= num4;
			result.W *= num4;
		}

		// Token: 0x0600370D RID: 14093 RVA: 0x0006CC48 File Offset: 0x0006AE48
		public static Quaternion Slerp(Quaternion quaternion1, Quaternion quaternion2, float amount)
		{
			Quaternion result;
			Quaternion.Slerp(ref quaternion1, ref quaternion2, amount, out result);
			return result;
		}

		// Token: 0x0600370E RID: 14094 RVA: 0x0006CC68 File Offset: 0x0006AE68
		public static void Slerp(ref Quaternion quaternion1, ref Quaternion quaternion2, float amount, out Quaternion result)
		{
			float num = quaternion1.X * quaternion2.X + quaternion1.Y * quaternion2.Y + quaternion1.Z * quaternion2.Z + quaternion1.W * quaternion2.W;
			float num2 = 1f;
			bool flag = num < 0f;
			if (flag)
			{
				num2 = -1f;
				num = -num;
			}
			bool flag2 = num > 0.999999f;
			float num3;
			float num4;
			if (flag2)
			{
				num3 = 1f - amount;
				num4 = amount * num2;
			}
			else
			{
				float num5 = (float)Math.Acos((double)num);
				float num6 = (float)(1.0 / Math.Sin((double)num5));
				num3 = (float)Math.Sin((double)((1f - amount) * num5)) * num6;
				num4 = num2 * ((float)Math.Sin((double)(amount * num5)) * num6);
			}
			result.X = num3 * quaternion1.X + num4 * quaternion2.X;
			result.Y = num3 * quaternion1.Y + num4 * quaternion2.Y;
			result.Z = num3 * quaternion1.Z + num4 * quaternion2.Z;
			result.W = num3 * quaternion1.W + num4 * quaternion2.W;
		}

		// Token: 0x0600370F RID: 14095 RVA: 0x0006CD94 File Offset: 0x0006AF94
		public static Quaternion Subtract(Quaternion quaternion1, Quaternion quaternion2)
		{
			Quaternion result;
			Quaternion.Subtract(ref quaternion1, ref quaternion2, out result);
			return result;
		}

		// Token: 0x06003710 RID: 14096 RVA: 0x0006CDB4 File Offset: 0x0006AFB4
		public static void Subtract(ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
		{
			result.X = quaternion1.X - quaternion2.X;
			result.Y = quaternion1.Y - quaternion2.Y;
			result.Z = quaternion1.Z - quaternion2.Z;
			result.W = quaternion1.W - quaternion2.W;
		}

		// Token: 0x06003711 RID: 14097 RVA: 0x0006CE10 File Offset: 0x0006B010
		public static Quaternion Multiply(Quaternion quaternion1, Quaternion quaternion2)
		{
			Quaternion result;
			Quaternion.Multiply(ref quaternion1, ref quaternion2, out result);
			return result;
		}

		// Token: 0x06003712 RID: 14098 RVA: 0x0006CE30 File Offset: 0x0006B030
		public static Quaternion Multiply(Quaternion quaternion1, float scaleFactor)
		{
			Quaternion result;
			Quaternion.Multiply(ref quaternion1, scaleFactor, out result);
			return result;
		}

		// Token: 0x06003713 RID: 14099 RVA: 0x0006CE50 File Offset: 0x0006B050
		public static void Multiply(ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
		{
			float x = quaternion1.X;
			float y = quaternion1.Y;
			float z = quaternion1.Z;
			float w = quaternion1.W;
			float x2 = quaternion2.X;
			float y2 = quaternion2.Y;
			float z2 = quaternion2.Z;
			float w2 = quaternion2.W;
			float num = y * z2 - z * y2;
			float num2 = z * x2 - x * z2;
			float num3 = x * y2 - y * x2;
			float num4 = x * x2 + y * y2 + z * z2;
			result.X = x * w2 + x2 * w + num;
			result.Y = y * w2 + y2 * w + num2;
			result.Z = z * w2 + z2 * w + num3;
			result.W = w * w2 - num4;
		}

		// Token: 0x06003714 RID: 14100 RVA: 0x0006CF0E File Offset: 0x0006B10E
		public static void Multiply(ref Quaternion quaternion1, float scaleFactor, out Quaternion result)
		{
			result.X = quaternion1.X * scaleFactor;
			result.Y = quaternion1.Y * scaleFactor;
			result.Z = quaternion1.Z * scaleFactor;
			result.W = quaternion1.W * scaleFactor;
		}

		// Token: 0x06003715 RID: 14101 RVA: 0x0006CF4C File Offset: 0x0006B14C
		public static Quaternion Negate(Quaternion quaternion)
		{
			return new Quaternion(-quaternion.X, -quaternion.Y, -quaternion.Z, -quaternion.W);
		}

		// Token: 0x06003716 RID: 14102 RVA: 0x0006CF7F File Offset: 0x0006B17F
		public static void Negate(ref Quaternion quaternion, out Quaternion result)
		{
			result.X = -quaternion.X;
			result.Y = -quaternion.Y;
			result.Z = -quaternion.Z;
			result.W = -quaternion.W;
		}

		// Token: 0x06003717 RID: 14103 RVA: 0x0006CFB8 File Offset: 0x0006B1B8
		public static Quaternion Normalize(Quaternion quaternion)
		{
			Quaternion result;
			Quaternion.Normalize(ref quaternion, out result);
			return result;
		}

		// Token: 0x06003718 RID: 14104 RVA: 0x0006CFD8 File Offset: 0x0006B1D8
		public static void Normalize(ref Quaternion quaternion, out Quaternion result)
		{
			float num = 1f / (float)Math.Sqrt((double)(quaternion.X * quaternion.X + quaternion.Y * quaternion.Y + quaternion.Z * quaternion.Z + quaternion.W * quaternion.W));
			result.X = quaternion.X * num;
			result.Y = quaternion.Y * num;
			result.Z = quaternion.Z * num;
			result.W = quaternion.W * num;
		}

		// Token: 0x06003719 RID: 14105 RVA: 0x0006D064 File Offset: 0x0006B264
		public static Quaternion operator +(Quaternion quaternion1, Quaternion quaternion2)
		{
			Quaternion result;
			Quaternion.Add(ref quaternion1, ref quaternion2, out result);
			return result;
		}

		// Token: 0x0600371A RID: 14106 RVA: 0x0006D084 File Offset: 0x0006B284
		public static Quaternion operator /(Quaternion quaternion1, Quaternion quaternion2)
		{
			Quaternion result;
			Quaternion.Divide(ref quaternion1, ref quaternion2, out result);
			return result;
		}

		// Token: 0x0600371B RID: 14107 RVA: 0x0006D0A4 File Offset: 0x0006B2A4
		public static bool operator ==(Quaternion quaternion1, Quaternion quaternion2)
		{
			return quaternion1.Equals(quaternion2);
		}

		// Token: 0x0600371C RID: 14108 RVA: 0x0006D0C0 File Offset: 0x0006B2C0
		public static bool operator !=(Quaternion quaternion1, Quaternion quaternion2)
		{
			return !quaternion1.Equals(quaternion2);
		}

		// Token: 0x0600371D RID: 14109 RVA: 0x0006D0E0 File Offset: 0x0006B2E0
		public static Quaternion operator *(Quaternion quaternion1, Quaternion quaternion2)
		{
			Quaternion result;
			Quaternion.Multiply(ref quaternion1, ref quaternion2, out result);
			return result;
		}

		// Token: 0x0600371E RID: 14110 RVA: 0x0006D100 File Offset: 0x0006B300
		public static Quaternion operator *(Quaternion quaternion1, float scaleFactor)
		{
			Quaternion result;
			Quaternion.Multiply(ref quaternion1, scaleFactor, out result);
			return result;
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x0006D120 File Offset: 0x0006B320
		public static Quaternion operator -(Quaternion quaternion1, Quaternion quaternion2)
		{
			Quaternion result;
			Quaternion.Subtract(ref quaternion1, ref quaternion2, out result);
			return result;
		}

		// Token: 0x06003720 RID: 14112 RVA: 0x0006D140 File Offset: 0x0006B340
		public static Quaternion operator -(Quaternion quaternion)
		{
			Quaternion result;
			Quaternion.Negate(ref quaternion, out result);
			return result;
		}

		// Token: 0x06003721 RID: 14113 RVA: 0x0006D160 File Offset: 0x0006B360
		public static Quaternion CreateFromVectors(Vector3 source, Vector3 destination)
		{
			Quaternion result;
			Quaternion.CreateFromVectors(ref source, ref destination, out result);
			return result;
		}

		// Token: 0x06003722 RID: 14114 RVA: 0x0006D180 File Offset: 0x0006B380
		public static void CreateFromVectors(ref Vector3 source, ref Vector3 destination, out Quaternion result)
		{
			source.Normalize();
			destination.Normalize();
			float num = Vector3.Dot(source, destination);
			bool flag = num >= 1f;
			if (flag)
			{
				result.X = 0f;
				result.Y = 0f;
				result.Z = 0f;
				result.W = 1f;
			}
			else
			{
				bool flag2 = num <= -1f;
				if (flag2)
				{
					Vector3 vector = Vector3.Cross(Vector3.UnitX, source);
					bool flag3 = vector.LengthSquared() == 0f;
					if (flag3)
					{
						vector = Vector3.Cross(Vector3.UnitY, source);
					}
					vector.Normalize();
					Quaternion.CreateFromAxisAngle(ref vector, 3.1415927f, out result);
				}
				else
				{
					float num2 = (float)Math.Sqrt((double)((1f + num) * 2f));
					Vector3 vector2 = Vector3.Cross(source, destination) / num2;
					result.X = vector2.X;
					result.Y = vector2.Y;
					result.Z = vector2.Z;
					result.W = 0.5f * num2;
					result.Normalize();
				}
			}
		}

		// Token: 0x06003723 RID: 14115 RVA: 0x0006D2B8 File Offset: 0x0006B4B8
		public static Quaternion CreateFromNormalizedVectors(Vector3 source, Vector3 destination)
		{
			Quaternion result;
			Quaternion.CreateFromNormalizedVectors(ref source, ref destination, out result);
			return result;
		}

		// Token: 0x06003724 RID: 14116 RVA: 0x0006D2D8 File Offset: 0x0006B4D8
		public static void CreateFromNormalizedVectors(ref Vector3 source, ref Vector3 destination, out Quaternion result)
		{
			float num = Vector3.Dot(source, destination);
			bool flag = num >= 1f;
			if (flag)
			{
				result.X = 0f;
				result.Y = 0f;
				result.Z = 0f;
				result.W = 1f;
			}
			else
			{
				bool flag2 = num <= -1f;
				if (flag2)
				{
					Vector3 vector = Vector3.Cross(Vector3.UnitX, source);
					bool flag3 = vector.LengthSquared() == 0f;
					if (flag3)
					{
						vector = Vector3.Cross(Vector3.UnitY, source);
					}
					vector.Normalize();
					Quaternion.CreateFromAxisAngle(ref vector, 3.1415927f, out result);
				}
				else
				{
					float num2 = (float)Math.Sqrt((double)((1f + num) * 2f));
					Vector3 vector2 = Vector3.Cross(source, destination) / num2;
					result.X = vector2.X;
					result.Y = vector2.Y;
					result.Z = vector2.Z;
					result.W = 0.5f * num2;
					result.Normalize();
				}
			}
		}

		// Token: 0x06003725 RID: 14117 RVA: 0x0006D400 File Offset: 0x0006B600
		public static void CreateFromYaw(float yaw, out Quaternion result)
		{
			float num = yaw * 0.5f;
			float y = (float)Math.Sin((double)num);
			float w = (float)Math.Cos((double)num);
			result.X = 0f;
			result.Y = y;
			result.Z = 0f;
			result.W = w;
		}

		// Token: 0x06003726 RID: 14118 RVA: 0x0006D44C File Offset: 0x0006B64C
		public static void CreateFromPitch(float pitch, out Quaternion result)
		{
			float num = pitch * 0.5f;
			float x = (float)Math.Sin((double)num);
			float w = (float)Math.Cos((double)num);
			result.X = x;
			result.Y = 0f;
			result.Z = 0f;
			result.W = w;
		}

		// Token: 0x0400183D RID: 6205
		public float X;

		// Token: 0x0400183E RID: 6206
		public float Y;

		// Token: 0x0400183F RID: 6207
		public float Z;

		// Token: 0x04001840 RID: 6208
		public float W;

		// Token: 0x04001841 RID: 6209
		private static readonly Quaternion identity = new Quaternion(0f, 0f, 0f, 1f);
	}
}
