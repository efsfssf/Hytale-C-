using System;
using System.Diagnostics;
using System.Text;
using Coherent.UI.Binding;
using HytaleClient.Protocol;
using Newtonsoft.Json;

namespace HytaleClient.Math
{
	// Token: 0x020007F8 RID: 2040
	[CoherentType]
	[DebuggerDisplay("{DebugDisplayString,nq}")]
	[Serializable]
	public struct Vector3 : IEquatable<Vector3>
	{
		// Token: 0x17000FEC RID: 4076
		// (get) Token: 0x060037AD RID: 14253 RVA: 0x0006F8B8 File Offset: 0x0006DAB8
		public static Vector3 Zero
		{
			get
			{
				return Vector3.zero;
			}
		}

		// Token: 0x17000FED RID: 4077
		// (get) Token: 0x060037AE RID: 14254 RVA: 0x0006F8D0 File Offset: 0x0006DAD0
		public static Vector3 One
		{
			get
			{
				return Vector3.one;
			}
		}

		// Token: 0x17000FEE RID: 4078
		// (get) Token: 0x060037AF RID: 14255 RVA: 0x0006F8E8 File Offset: 0x0006DAE8
		public static Vector3 UnitX
		{
			get
			{
				return Vector3.unitX;
			}
		}

		// Token: 0x17000FEF RID: 4079
		// (get) Token: 0x060037B0 RID: 14256 RVA: 0x0006F900 File Offset: 0x0006DB00
		public static Vector3 UnitY
		{
			get
			{
				return Vector3.unitY;
			}
		}

		// Token: 0x17000FF0 RID: 4080
		// (get) Token: 0x060037B1 RID: 14257 RVA: 0x0006F918 File Offset: 0x0006DB18
		public static Vector3 UnitZ
		{
			get
			{
				return Vector3.unitZ;
			}
		}

		// Token: 0x17000FF1 RID: 4081
		// (get) Token: 0x060037B2 RID: 14258 RVA: 0x0006F930 File Offset: 0x0006DB30
		public static Vector3 Up
		{
			get
			{
				return Vector3.up;
			}
		}

		// Token: 0x17000FF2 RID: 4082
		// (get) Token: 0x060037B3 RID: 14259 RVA: 0x0006F948 File Offset: 0x0006DB48
		public static Vector3 Down
		{
			get
			{
				return Vector3.down;
			}
		}

		// Token: 0x17000FF3 RID: 4083
		// (get) Token: 0x060037B4 RID: 14260 RVA: 0x0006F960 File Offset: 0x0006DB60
		public static Vector3 Right
		{
			get
			{
				return Vector3.right;
			}
		}

		// Token: 0x17000FF4 RID: 4084
		// (get) Token: 0x060037B5 RID: 14261 RVA: 0x0006F978 File Offset: 0x0006DB78
		public static Vector3 Left
		{
			get
			{
				return Vector3.left;
			}
		}

		// Token: 0x17000FF5 RID: 4085
		// (get) Token: 0x060037B6 RID: 14262 RVA: 0x0006F990 File Offset: 0x0006DB90
		public static Vector3 Forward
		{
			get
			{
				return Vector3.forward;
			}
		}

		// Token: 0x17000FF6 RID: 4086
		// (get) Token: 0x060037B7 RID: 14263 RVA: 0x0006F9A8 File Offset: 0x0006DBA8
		public static Vector3 Backward
		{
			get
			{
				return Vector3.backward;
			}
		}

		// Token: 0x17000FF7 RID: 4087
		// (get) Token: 0x060037B8 RID: 14264 RVA: 0x0006F9C0 File Offset: 0x0006DBC0
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
					this.Z.ToString()
				});
			}
		}

		// Token: 0x060037B9 RID: 14265 RVA: 0x0006FA17 File Offset: 0x0006DC17
		public Vector3(float x, float y, float z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x060037BA RID: 14266 RVA: 0x0006FA2F File Offset: 0x0006DC2F
		public Vector3(float value)
		{
			this.X = value;
			this.Y = value;
			this.Z = value;
		}

		// Token: 0x060037BB RID: 14267 RVA: 0x0006FA47 File Offset: 0x0006DC47
		public Vector3(Vector2 value, float z)
		{
			this.X = value.X;
			this.Y = value.Y;
			this.Z = z;
		}

		// Token: 0x060037BC RID: 14268 RVA: 0x0006FA6C File Offset: 0x0006DC6C
		public override bool Equals(object obj)
		{
			return obj is Vector3 && this.Equals((Vector3)obj);
		}

		// Token: 0x060037BD RID: 14269 RVA: 0x0006FA98 File Offset: 0x0006DC98
		public bool Equals(Vector3 other)
		{
			return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
		}

		// Token: 0x060037BE RID: 14270 RVA: 0x0006FAD8 File Offset: 0x0006DCD8
		public override int GetHashCode()
		{
			return (int)(this.X + this.Y + this.Z);
		}

		// Token: 0x060037BF RID: 14271 RVA: 0x0006FB00 File Offset: 0x0006DD00
		public float Length()
		{
			float num;
			Vector3.DistanceSquared(ref this, ref Vector3.zero, out num);
			return (float)Math.Sqrt((double)num);
		}

		// Token: 0x060037C0 RID: 14272 RVA: 0x0006FB28 File Offset: 0x0006DD28
		public float LengthSquared()
		{
			float result;
			Vector3.DistanceSquared(ref this, ref Vector3.zero, out result);
			return result;
		}

		// Token: 0x060037C1 RID: 14273 RVA: 0x0006FB4C File Offset: 0x0006DD4C
		public Vector3 Abs()
		{
			return new Vector3(Math.Abs(this.X), Math.Abs(this.Y), Math.Abs(this.Z));
		}

		// Token: 0x060037C2 RID: 14274 RVA: 0x0006FB84 File Offset: 0x0006DD84
		public Vector3 Sign(Vector3 other)
		{
			return new Vector3((float)Math.Sign(other.X) * this.X, (float)Math.Sign(other.Y) * this.Y, (float)Math.Sign(other.Z) * this.Z);
		}

		// Token: 0x060037C3 RID: 14275 RVA: 0x0006FBD4 File Offset: 0x0006DDD4
		public void Normalize()
		{
			Vector3.Normalize(ref this, out this);
		}

		// Token: 0x060037C4 RID: 14276 RVA: 0x0006FBE0 File Offset: 0x0006DDE0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(32);
			stringBuilder.Append("{X:");
			stringBuilder.Append(this.X);
			stringBuilder.Append(" Y:");
			stringBuilder.Append(this.Y);
			stringBuilder.Append(" Z:");
			stringBuilder.Append(this.Z);
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		// Token: 0x060037C5 RID: 14277 RVA: 0x0006FC58 File Offset: 0x0006DE58
		public static Vector3 Add(Vector3 value1, Vector3 value2)
		{
			value1.X += value2.X;
			value1.Y += value2.Y;
			value1.Z += value2.Z;
			return value1;
		}

		// Token: 0x060037C6 RID: 14278 RVA: 0x0006FC9E File Offset: 0x0006DE9E
		public static void Add(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
		{
			result.X = value1.X + value2.X;
			result.Y = value1.Y + value2.Y;
			result.Z = value1.Z + value2.Z;
		}

		// Token: 0x060037C7 RID: 14279 RVA: 0x0006FCDC File Offset: 0x0006DEDC
		public static Vector3 Barycentric(Vector3 value1, Vector3 value2, Vector3 value3, float amount1, float amount2)
		{
			return new Vector3(MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2), MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2), MathHelper.Barycentric(value1.Z, value2.Z, value3.Z, amount1, amount2));
		}

		// Token: 0x060037C8 RID: 14280 RVA: 0x0006FD44 File Offset: 0x0006DF44
		public static void Barycentric(ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, float amount1, float amount2, out Vector3 result)
		{
			result.X = MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2);
			result.Y = MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2);
			result.Z = MathHelper.Barycentric(value1.Z, value2.Z, value3.Z, amount1, amount2);
		}

		// Token: 0x060037C9 RID: 14281 RVA: 0x0006FDB8 File Offset: 0x0006DFB8
		public static Vector3 CatmullRom(Vector3 value1, Vector3 value2, Vector3 value3, Vector3 value4, float amount)
		{
			return new Vector3(MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount), MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount), MathHelper.CatmullRom(value1.Z, value2.Z, value3.Z, value4.Z, amount));
		}

		// Token: 0x060037CA RID: 14282 RVA: 0x0006FE2C File Offset: 0x0006E02C
		public static void CatmullRom(ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, ref Vector3 value4, float amount, out Vector3 result)
		{
			result.X = MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount);
			result.Y = MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount);
			result.Z = MathHelper.CatmullRom(value1.Z, value2.Z, value3.Z, value4.Z, amount);
		}

		// Token: 0x060037CB RID: 14283 RVA: 0x0006FEAC File Offset: 0x0006E0AC
		public static Vector3 Clamp(Vector3 value1, Vector3 min, Vector3 max)
		{
			return new Vector3(MathHelper.Clamp(value1.X, min.X, max.X), MathHelper.Clamp(value1.Y, min.Y, max.Y), MathHelper.Clamp(value1.Z, min.Z, max.Z));
		}

		// Token: 0x060037CC RID: 14284 RVA: 0x0006FF08 File Offset: 0x0006E108
		public static void Clamp(ref Vector3 value1, ref Vector3 min, ref Vector3 max, out Vector3 result)
		{
			result.X = MathHelper.Clamp(value1.X, min.X, max.X);
			result.Y = MathHelper.Clamp(value1.Y, min.Y, max.Y);
			result.Z = MathHelper.Clamp(value1.Z, min.Z, max.Z);
		}

		// Token: 0x060037CD RID: 14285 RVA: 0x0006FF70 File Offset: 0x0006E170
		public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
		{
			Vector3.Cross(ref vector1, ref vector2, out vector1);
			return vector1;
		}

		// Token: 0x060037CE RID: 14286 RVA: 0x0006FF90 File Offset: 0x0006E190
		public static void Cross(ref Vector3 vector1, ref Vector3 vector2, out Vector3 result)
		{
			float x = vector1.Y * vector2.Z - vector2.Y * vector1.Z;
			float y = -(vector1.X * vector2.Z - vector2.X * vector1.Z);
			float z = vector1.X * vector2.Y - vector2.X * vector1.Y;
			result.X = x;
			result.Y = y;
			result.Z = z;
		}

		// Token: 0x060037CF RID: 14287 RVA: 0x00070008 File Offset: 0x0006E208
		public static float Distance(Vector3 vector1, Vector3 vector2)
		{
			float num;
			Vector3.DistanceSquared(ref vector1, ref vector2, out num);
			return (float)Math.Sqrt((double)num);
		}

		// Token: 0x060037D0 RID: 14288 RVA: 0x0007002E File Offset: 0x0006E22E
		public static void Distance(ref Vector3 value1, ref Vector3 value2, out float result)
		{
			Vector3.DistanceSquared(ref value1, ref value2, out result);
			result = (float)Math.Sqrt((double)result);
		}

		// Token: 0x060037D1 RID: 14289 RVA: 0x00070048 File Offset: 0x0006E248
		public static float DistanceSquared(Vector3 value1, Vector3 value2)
		{
			return (value1.X - value2.X) * (value1.X - value2.X) + (value1.Y - value2.Y) * (value1.Y - value2.Y) + (value1.Z - value2.Z) * (value1.Z - value2.Z);
		}

		// Token: 0x060037D2 RID: 14290 RVA: 0x000700B0 File Offset: 0x0006E2B0
		public static void DistanceSquared(ref Vector3 value1, ref Vector3 value2, out float result)
		{
			result = (value1.X - value2.X) * (value1.X - value2.X) + (value1.Y - value2.Y) * (value1.Y - value2.Y) + (value1.Z - value2.Z) * (value1.Z - value2.Z);
		}

		// Token: 0x060037D3 RID: 14291 RVA: 0x00070114 File Offset: 0x0006E314
		public static Vector3 Divide(Vector3 value1, Vector3 value2)
		{
			value1.X /= value2.X;
			value1.Y /= value2.Y;
			value1.Z /= value2.Z;
			return value1;
		}

		// Token: 0x060037D4 RID: 14292 RVA: 0x0007015A File Offset: 0x0006E35A
		public static void Divide(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
		{
			result.X = value1.X / value2.X;
			result.Y = value1.Y / value2.Y;
			result.Z = value1.Z / value2.Z;
		}

		// Token: 0x060037D5 RID: 14293 RVA: 0x00070198 File Offset: 0x0006E398
		public static Vector3 Divide(Vector3 value1, float value2)
		{
			float num = 1f / value2;
			value1.X *= num;
			value1.Y *= num;
			value1.Z *= num;
			return value1;
		}

		// Token: 0x060037D6 RID: 14294 RVA: 0x000701D8 File Offset: 0x0006E3D8
		public static void Divide(ref Vector3 value1, float value2, out Vector3 result)
		{
			float num = 1f / value2;
			result.X = value1.X * num;
			result.Y = value1.Y * num;
			result.Z = value1.Z * num;
		}

		// Token: 0x060037D7 RID: 14295 RVA: 0x00070218 File Offset: 0x0006E418
		public static float Dot(Vector3 vector1, Vector3 vector2)
		{
			return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
		}

		// Token: 0x060037D8 RID: 14296 RVA: 0x00070253 File Offset: 0x0006E453
		public static void Dot(ref Vector3 vector1, ref Vector3 vector2, out float result)
		{
			result = vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
		}

		// Token: 0x060037D9 RID: 14297 RVA: 0x00070284 File Offset: 0x0006E484
		public static Vector3 Hermite(Vector3 value1, Vector3 tangent1, Vector3 value2, Vector3 tangent2, float amount)
		{
			Vector3 result = default(Vector3);
			Vector3.Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
			return result;
		}

		// Token: 0x060037DA RID: 14298 RVA: 0x000702B4 File Offset: 0x0006E4B4
		public static void Hermite(ref Vector3 value1, ref Vector3 tangent1, ref Vector3 value2, ref Vector3 tangent2, float amount, out Vector3 result)
		{
			result.X = MathHelper.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount);
			result.Y = MathHelper.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount);
			result.Z = MathHelper.Hermite(value1.Z, tangent1.Z, value2.Z, tangent2.Z, amount);
		}

		// Token: 0x060037DB RID: 14299 RVA: 0x00070334 File Offset: 0x0006E534
		public static Vector3 Lerp(Vector3 value1, Vector3 value2, float amount)
		{
			return new Vector3(MathHelper.Lerp(value1.X, value2.X, amount), MathHelper.Lerp(value1.Y, value2.Y, amount), MathHelper.Lerp(value1.Z, value2.Z, amount));
		}

		// Token: 0x060037DC RID: 14300 RVA: 0x00070384 File Offset: 0x0006E584
		public static void Lerp(ref Vector3 value1, ref Vector3 value2, float amount, out Vector3 result)
		{
			result.X = MathHelper.Lerp(value1.X, value2.X, amount);
			result.Y = MathHelper.Lerp(value1.Y, value2.Y, amount);
			result.Z = MathHelper.Lerp(value1.Z, value2.Z, amount);
		}

		// Token: 0x060037DD RID: 14301 RVA: 0x000703DC File Offset: 0x0006E5DC
		public static Vector3 Max(Vector3 value1, Vector3 value2)
		{
			return new Vector3(MathHelper.Max(value1.X, value2.X), MathHelper.Max(value1.Y, value2.Y), MathHelper.Max(value1.Z, value2.Z));
		}

		// Token: 0x060037DE RID: 14302 RVA: 0x00070428 File Offset: 0x0006E628
		public static void Max(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
		{
			result.X = MathHelper.Max(value1.X, value2.X);
			result.Y = MathHelper.Max(value1.Y, value2.Y);
			result.Z = MathHelper.Max(value1.Z, value2.Z);
		}

		// Token: 0x060037DF RID: 14303 RVA: 0x0007047C File Offset: 0x0006E67C
		public static Vector3 Min(Vector3 value1, Vector3 value2)
		{
			return new Vector3(MathHelper.Min(value1.X, value2.X), MathHelper.Min(value1.Y, value2.Y), MathHelper.Min(value1.Z, value2.Z));
		}

		// Token: 0x060037E0 RID: 14304 RVA: 0x000704C8 File Offset: 0x0006E6C8
		public static void Min(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
		{
			result.X = MathHelper.Min(value1.X, value2.X);
			result.Y = MathHelper.Min(value1.Y, value2.Y);
			result.Z = MathHelper.Min(value1.Z, value2.Z);
		}

		// Token: 0x060037E1 RID: 14305 RVA: 0x0007051C File Offset: 0x0006E71C
		public static Vector3 Multiply(Vector3 value1, Vector3 value2)
		{
			value1.X *= value2.X;
			value1.Y *= value2.Y;
			value1.Z *= value2.Z;
			return value1;
		}

		// Token: 0x060037E2 RID: 14306 RVA: 0x00070564 File Offset: 0x0006E764
		public static Vector3 Multiply(Vector3 value1, float scaleFactor)
		{
			value1.X *= scaleFactor;
			value1.Y *= scaleFactor;
			value1.Z *= scaleFactor;
			return value1;
		}

		// Token: 0x060037E3 RID: 14307 RVA: 0x0007059B File Offset: 0x0006E79B
		public static void Multiply(ref Vector3 value1, float scaleFactor, out Vector3 result)
		{
			result.X = value1.X * scaleFactor;
			result.Y = value1.Y * scaleFactor;
			result.Z = value1.Z * scaleFactor;
		}

		// Token: 0x060037E4 RID: 14308 RVA: 0x000705C8 File Offset: 0x0006E7C8
		public static void Multiply(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
		{
			result.X = value1.X * value2.X;
			result.Y = value1.Y * value2.Y;
			result.Z = value1.Z * value2.Z;
		}

		// Token: 0x060037E5 RID: 14309 RVA: 0x00070604 File Offset: 0x0006E804
		public static Vector3 Negate(Vector3 value)
		{
			value = new Vector3(-value.X, -value.Y, -value.Z);
			return value;
		}

		// Token: 0x060037E6 RID: 14310 RVA: 0x00070633 File Offset: 0x0006E833
		public static void Negate(ref Vector3 value, out Vector3 result)
		{
			result.X = -value.X;
			result.Y = -value.Y;
			result.Z = -value.Z;
		}

		// Token: 0x060037E7 RID: 14311 RVA: 0x00070660 File Offset: 0x0006E860
		public static Vector3 Normalize(Vector3 value)
		{
			Vector3.Normalize(ref value, out value);
			return value;
		}

		// Token: 0x060037E8 RID: 14312 RVA: 0x00070680 File Offset: 0x0006E880
		public static void Normalize(ref Vector3 value, out Vector3 result)
		{
			float num;
			Vector3.Distance(ref value, ref Vector3.zero, out num);
			num = 1f / num;
			result.X = value.X * num;
			result.Y = value.Y * num;
			result.Z = value.Z * num;
		}

		// Token: 0x060037E9 RID: 14313 RVA: 0x000706D0 File Offset: 0x0006E8D0
		public static Vector3 Reflect(Vector3 vector, Vector3 normal)
		{
			float num = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;
			Vector3 result;
			result.X = vector.X - 2f * normal.X * num;
			result.Y = vector.Y - 2f * normal.Y * num;
			result.Z = vector.Z - 2f * normal.Z * num;
			return result;
		}

		// Token: 0x060037EA RID: 14314 RVA: 0x00070764 File Offset: 0x0006E964
		public static void Reflect(ref Vector3 vector, ref Vector3 normal, out Vector3 result)
		{
			float num = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;
			result.X = vector.X - 2f * normal.X * num;
			result.Y = vector.Y - 2f * normal.Y * num;
			result.Z = vector.Z - 2f * normal.Z * num;
		}

		// Token: 0x060037EB RID: 14315 RVA: 0x000707F0 File Offset: 0x0006E9F0
		public static Vector3 SmoothStep(Vector3 value1, Vector3 value2, float amount)
		{
			return new Vector3(MathHelper.SmoothStep(value1.X, value2.X, amount), MathHelper.SmoothStep(value1.Y, value2.Y, amount), MathHelper.SmoothStep(value1.Z, value2.Z, amount));
		}

		// Token: 0x060037EC RID: 14316 RVA: 0x00070840 File Offset: 0x0006EA40
		public static void SmoothStep(ref Vector3 value1, ref Vector3 value2, float amount, out Vector3 result)
		{
			result.X = MathHelper.SmoothStep(value1.X, value2.X, amount);
			result.Y = MathHelper.SmoothStep(value1.Y, value2.Y, amount);
			result.Z = MathHelper.SmoothStep(value1.Z, value2.Z, amount);
		}

		// Token: 0x060037ED RID: 14317 RVA: 0x00070898 File Offset: 0x0006EA98
		public static Vector3 Subtract(Vector3 value1, Vector3 value2)
		{
			value1.X -= value2.X;
			value1.Y -= value2.Y;
			value1.Z -= value2.Z;
			return value1;
		}

		// Token: 0x060037EE RID: 14318 RVA: 0x000708DE File Offset: 0x0006EADE
		public static void Subtract(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
		{
			result.X = value1.X - value2.X;
			result.Y = value1.Y - value2.Y;
			result.Z = value1.Z - value2.Z;
		}

		// Token: 0x060037EF RID: 14319 RVA: 0x0007091C File Offset: 0x0006EB1C
		public static Vector3 Transform(Vector3 position, Matrix matrix)
		{
			Vector3.Transform(ref position, ref matrix, out position);
			return position;
		}

		// Token: 0x060037F0 RID: 14320 RVA: 0x0007093C File Offset: 0x0006EB3C
		public static void Transform(ref Vector3 position, ref Matrix matrix, out Vector3 result)
		{
			float x = position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41;
			float y = position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42;
			float z = position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43;
			result.X = x;
			result.Y = y;
			result.Z = z;
		}

		// Token: 0x060037F1 RID: 14321 RVA: 0x000709F4 File Offset: 0x0006EBF4
		public static void Transform(Vector3[] sourceArray, ref Matrix matrix, Vector3[] destinationArray)
		{
			Debug.Assert(destinationArray.Length >= sourceArray.Length, "The destination array is smaller than the source array.");
			for (int i = 0; i < sourceArray.Length; i++)
			{
				Vector3 vector = sourceArray[i];
				destinationArray[i] = new Vector3(vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31 + matrix.M41, vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32 + matrix.M42, vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33 + matrix.M43);
			}
		}

		// Token: 0x060037F2 RID: 14322 RVA: 0x00070AD8 File Offset: 0x0006ECD8
		public static void Transform(Vector3[] sourceArray, int sourceIndex, ref Matrix matrix, Vector3[] destinationArray, int destinationIndex, int length)
		{
			Debug.Assert(sourceArray.Length - sourceIndex >= length, "The source array is too small for the given sourceIndex and length.");
			Debug.Assert(destinationArray.Length - destinationIndex >= length, "The destination array is too small for the given destinationIndex and length.");
			for (int i = 0; i < length; i++)
			{
				Vector3 vector = sourceArray[sourceIndex + i];
				destinationArray[destinationIndex + i] = new Vector3(vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31 + matrix.M41, vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32 + matrix.M42, vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33 + matrix.M43);
			}
		}

		// Token: 0x060037F3 RID: 14323 RVA: 0x00070BD8 File Offset: 0x0006EDD8
		public static Vector3 Transform(Vector3 value, Quaternion rotation)
		{
			Vector3 result;
			Vector3.Transform(ref value, ref rotation, out result);
			return result;
		}

		// Token: 0x060037F4 RID: 14324 RVA: 0x00070BF8 File Offset: 0x0006EDF8
		public static void Transform(ref Vector3 value, ref Quaternion rotation, out Vector3 result)
		{
			float num = 2f * (rotation.Y * value.Z - rotation.Z * value.Y);
			float num2 = 2f * (rotation.Z * value.X - rotation.X * value.Z);
			float num3 = 2f * (rotation.X * value.Y - rotation.Y * value.X);
			result.X = value.X + num * rotation.W + (rotation.Y * num3 - rotation.Z * num2);
			result.Y = value.Y + num2 * rotation.W + (rotation.Z * num - rotation.X * num3);
			result.Z = value.Z + num3 * rotation.W + (rotation.X * num2 - rotation.Y * num);
		}

		// Token: 0x060037F5 RID: 14325 RVA: 0x00070CE4 File Offset: 0x0006EEE4
		public static void Transform(Vector3[] sourceArray, ref Quaternion rotation, Vector3[] destinationArray)
		{
			Debug.Assert(destinationArray.Length >= sourceArray.Length, "The destination array is smaller than the source array.");
			for (int i = 0; i < sourceArray.Length; i++)
			{
				Vector3 vector = sourceArray[i];
				float num = 2f * (rotation.Y * vector.Z - rotation.Z * vector.Y);
				float num2 = 2f * (rotation.Z * vector.X - rotation.X * vector.Z);
				float num3 = 2f * (rotation.X * vector.Y - rotation.Y * vector.X);
				destinationArray[i] = new Vector3(vector.X + num * rotation.W + (rotation.Y * num3 - rotation.Z * num2), vector.Y + num2 * rotation.W + (rotation.Z * num - rotation.X * num3), vector.Z + num3 * rotation.W + (rotation.X * num2 - rotation.Y * num));
			}
		}

		// Token: 0x060037F6 RID: 14326 RVA: 0x00070E08 File Offset: 0x0006F008
		public static void Transform(Vector3[] sourceArray, int sourceIndex, ref Quaternion rotation, Vector3[] destinationArray, int destinationIndex, int length)
		{
			Debug.Assert(sourceArray.Length - sourceIndex >= length, "The source array is too small for the given sourceIndex and length.");
			Debug.Assert(destinationArray.Length - destinationIndex >= length, "The destination array is too small for the given destinationIndex and length.");
			for (int i = 0; i < length; i++)
			{
				Vector3 vector = sourceArray[sourceIndex + i];
				float num = 2f * (rotation.Y * vector.Z - rotation.Z * vector.Y);
				float num2 = 2f * (rotation.Z * vector.X - rotation.X * vector.Z);
				float num3 = 2f * (rotation.X * vector.Y - rotation.Y * vector.X);
				destinationArray[destinationIndex + i] = new Vector3(vector.X + num * rotation.W + (rotation.Y * num3 - rotation.Z * num2), vector.Y + num2 * rotation.W + (rotation.Z * num - rotation.X * num3), vector.Z + num3 * rotation.W + (rotation.X * num2 - rotation.Y * num));
			}
		}

		// Token: 0x060037F7 RID: 14327 RVA: 0x00070F48 File Offset: 0x0006F148
		public static Vector3 TransformNormal(Vector3 normal, Matrix matrix)
		{
			Vector3.TransformNormal(ref normal, ref matrix, out normal);
			return normal;
		}

		// Token: 0x060037F8 RID: 14328 RVA: 0x00070F68 File Offset: 0x0006F168
		public static void TransformNormal(ref Vector3 normal, ref Matrix matrix, out Vector3 result)
		{
			float x = normal.X * matrix.M11 + normal.Y * matrix.M21 + normal.Z * matrix.M31;
			float y = normal.X * matrix.M12 + normal.Y * matrix.M22 + normal.Z * matrix.M32;
			float z = normal.X * matrix.M13 + normal.Y * matrix.M23 + normal.Z * matrix.M33;
			result.X = x;
			result.Y = y;
			result.Z = z;
		}

		// Token: 0x060037F9 RID: 14329 RVA: 0x0007100C File Offset: 0x0006F20C
		public static void TransformNormal(Vector3[] sourceArray, ref Matrix matrix, Vector3[] destinationArray)
		{
			Debug.Assert(destinationArray.Length >= sourceArray.Length, "The destination array is smaller than the source array.");
			for (int i = 0; i < sourceArray.Length; i++)
			{
				Vector3 vector = sourceArray[i];
				destinationArray[i].X = vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31;
				destinationArray[i].Y = vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32;
				destinationArray[i].Z = vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33;
			}
		}

		// Token: 0x060037FA RID: 14330 RVA: 0x000710F4 File Offset: 0x0006F2F4
		public static void TransformNormal(Vector3[] sourceArray, int sourceIndex, ref Matrix matrix, Vector3[] destinationArray, int destinationIndex, int length)
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
			bool flag3 = sourceIndex + length > sourceArray.Length;
			if (flag3)
			{
				throw new ArgumentException("the combination of sourceIndex and length was greater than sourceArray.Length");
			}
			bool flag4 = destinationIndex + length > destinationArray.Length;
			if (flag4)
			{
				throw new ArgumentException("destinationArray is too small to contain the result");
			}
			for (int i = 0; i < length; i++)
			{
				Vector3 vector = sourceArray[i + sourceIndex];
				destinationArray[i + destinationIndex].X = vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31;
				destinationArray[i + destinationIndex].Y = vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32;
				destinationArray[i + destinationIndex].Z = vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33;
			}
		}

		// Token: 0x060037FB RID: 14331 RVA: 0x0007123C File Offset: 0x0006F43C
		public static bool operator ==(Vector3 value1, Vector3 value2)
		{
			return value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z;
		}

		// Token: 0x060037FC RID: 14332 RVA: 0x0007127C File Offset: 0x0006F47C
		public static bool operator !=(Vector3 value1, Vector3 value2)
		{
			return !(value1 == value2);
		}

		// Token: 0x060037FD RID: 14333 RVA: 0x00071298 File Offset: 0x0006F498
		public static Vector3 operator +(Vector3 value1, Vector3 value2)
		{
			value1.X += value2.X;
			value1.Y += value2.Y;
			value1.Z += value2.Z;
			return value1;
		}

		// Token: 0x060037FE RID: 14334 RVA: 0x000712E0 File Offset: 0x0006F4E0
		public static Vector3 operator -(Vector3 value)
		{
			value = new Vector3(-value.X, -value.Y, -value.Z);
			return value;
		}

		// Token: 0x060037FF RID: 14335 RVA: 0x00071310 File Offset: 0x0006F510
		public static Vector3 operator -(Vector3 value1, Vector3 value2)
		{
			value1.X -= value2.X;
			value1.Y -= value2.Y;
			value1.Z -= value2.Z;
			return value1;
		}

		// Token: 0x06003800 RID: 14336 RVA: 0x00071358 File Offset: 0x0006F558
		public static Vector3 operator *(Vector3 value1, Vector3 value2)
		{
			value1.X *= value2.X;
			value1.Y *= value2.Y;
			value1.Z *= value2.Z;
			return value1;
		}

		// Token: 0x06003801 RID: 14337 RVA: 0x000713A0 File Offset: 0x0006F5A0
		public static Vector3 operator *(Vector3 value, float scaleFactor)
		{
			value.X *= scaleFactor;
			value.Y *= scaleFactor;
			value.Z *= scaleFactor;
			return value;
		}

		// Token: 0x06003802 RID: 14338 RVA: 0x000713D8 File Offset: 0x0006F5D8
		public static Vector3 operator *(float scaleFactor, Vector3 value)
		{
			value.X *= scaleFactor;
			value.Y *= scaleFactor;
			value.Z *= scaleFactor;
			return value;
		}

		// Token: 0x06003803 RID: 14339 RVA: 0x00071410 File Offset: 0x0006F610
		public static Vector3 operator /(Vector3 value1, Vector3 value2)
		{
			value1.X /= value2.X;
			value1.Y /= value2.Y;
			value1.Z /= value2.Z;
			return value1;
		}

		// Token: 0x06003804 RID: 14340 RVA: 0x00071458 File Offset: 0x0006F658
		public static Vector3 operator /(Vector3 value, float divider)
		{
			float num = 1f / divider;
			value.X *= num;
			value.Y *= num;
			value.Z *= num;
			return value;
		}

		// Token: 0x17000FF8 RID: 4088
		// (get) Token: 0x06003805 RID: 14341 RVA: 0x00071498 File Offset: 0x0006F698
		// (set) Token: 0x06003806 RID: 14342 RVA: 0x000714B0 File Offset: 0x0006F6B0
		[JsonIgnore]
		public float Pitch
		{
			get
			{
				return this.X;
			}
			set
			{
				this.X = value;
			}
		}

		// Token: 0x17000FF9 RID: 4089
		// (get) Token: 0x06003807 RID: 14343 RVA: 0x000714BC File Offset: 0x0006F6BC
		// (set) Token: 0x06003808 RID: 14344 RVA: 0x000714D4 File Offset: 0x0006F6D4
		[JsonIgnore]
		public float Yaw
		{
			get
			{
				return this.Y;
			}
			set
			{
				this.Y = value;
			}
		}

		// Token: 0x17000FFA RID: 4090
		// (get) Token: 0x06003809 RID: 14345 RVA: 0x000714E0 File Offset: 0x0006F6E0
		// (set) Token: 0x0600380A RID: 14346 RVA: 0x000714F8 File Offset: 0x0006F6F8
		[JsonIgnore]
		public float Roll
		{
			get
			{
				return this.Z;
			}
			set
			{
				this.Z = value;
			}
		}

		// Token: 0x0600380B RID: 14347 RVA: 0x00071504 File Offset: 0x0006F704
		public Position ToPositionPacket()
		{
			return new Position((double)this.X, (double)this.Y, (double)this.Z);
		}

		// Token: 0x0600380C RID: 14348 RVA: 0x00071530 File Offset: 0x0006F730
		public Vector3f ToProtocolVector3f()
		{
			return new Vector3f(this.X, this.Y, this.Z);
		}

		// Token: 0x0600380D RID: 14349 RVA: 0x0007155C File Offset: 0x0006F75C
		public Direction ToDirectionPacket()
		{
			return new Direction(this.Yaw, this.Pitch, this.Roll);
		}

		// Token: 0x0600380E RID: 14350 RVA: 0x00071588 File Offset: 0x0006F788
		public Vector3 ClipToZero(float epsilon)
		{
			return new Vector3(MathHelper.ClipToZero(this.X, epsilon), MathHelper.ClipToZero(this.Y, epsilon), MathHelper.ClipToZero(this.Z, epsilon));
		}

		// Token: 0x0600380F RID: 14351 RVA: 0x000715C4 File Offset: 0x0006F7C4
		public static Vector3 Spline(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
		{
			Vector3 result = default(Vector3);
			Vector3.Spline(ref t, ref p0, ref p1, ref p2, ref p3, out result);
			return result;
		}

		// Token: 0x06003810 RID: 14352 RVA: 0x000715F4 File Offset: 0x0006F7F4
		public static void Spline(ref float t, ref Vector3 p0, ref Vector3 p1, ref Vector3 p2, ref Vector3 p3, out Vector3 result)
		{
			result.X = MathHelper.Spline(t, p0.X, p1.X, p2.X, p3.X);
			result.Y = MathHelper.Spline(t, p0.Y, p1.Y, p2.Y, p3.Y);
			result.Z = MathHelper.Spline(t, p0.Z, p1.Z, p2.Z, p3.Z);
		}

		// Token: 0x06003811 RID: 14353 RVA: 0x00071678 File Offset: 0x0006F878
		public static void CubicBezierCurve(ref float t, ref Vector3 p0, ref Vector3 p1, ref Vector3 p2, ref Vector3 p3, out Vector3 result)
		{
			result.X = MathHelper.CubicBezierCurve(t, p0.X, p1.X, p2.X, p3.X);
			result.Y = MathHelper.CubicBezierCurve(t, p0.Y, p1.Y, p2.Y, p3.Y);
			result.Z = MathHelper.CubicBezierCurve(t, p0.Z, p1.Z, p2.Z, p3.Z);
		}

		// Token: 0x06003812 RID: 14354 RVA: 0x000716FC File Offset: 0x0006F8FC
		public static Vector3 GetTargetDirection(Vector3 source, Vector3 target)
		{
			Vector3 vector = Vector3.Subtract(target, source);
			vector.Normalize();
			double num = Math.Atan2((double)vector.X, (double)vector.Z) + 3.1415927410125732;
			double num2 = Math.Asin((double)vector.Y);
			return new Vector3((float)num2, (float)num, 0f);
		}

		// Token: 0x06003813 RID: 14355 RVA: 0x00071758 File Offset: 0x0006F958
		public static Vector3 CreateFromYawPitch(float yaw, float pitch)
		{
			float num = (float)Math.Cos((double)pitch);
			return new Vector3(num * -(float)Math.Sin((double)yaw), (float)Math.Sin((double)pitch), num * -(float)Math.Cos((double)yaw));
		}

		// Token: 0x17000FFB RID: 4091
		// (get) Token: 0x06003814 RID: 14356 RVA: 0x00071798 File Offset: 0x0006F998
		public static Vector3 NaN
		{
			get
			{
				return Vector3.nan;
			}
		}

		// Token: 0x06003815 RID: 14357 RVA: 0x000717B0 File Offset: 0x0006F9B0
		public bool IsNaN()
		{
			return float.IsNaN(this.X) && float.IsNaN(this.Y) && float.IsNaN(this.Z);
		}

		// Token: 0x06003816 RID: 14358 RVA: 0x000717EC File Offset: 0x0006F9EC
		public static Vector3 LerpAngle(Vector3 one, Vector3 two, float progress)
		{
			return new Vector3(MathHelper.LerpAngle(one.Pitch, two.Pitch, progress), MathHelper.LerpAngle(one.Yaw, two.Yaw, progress), MathHelper.LerpAngle(one.Roll, two.Roll, progress));
		}

		// Token: 0x06003817 RID: 14359 RVA: 0x00071840 File Offset: 0x0006FA40
		public static Vector3 WrapAngle(Vector3 vector)
		{
			return new Vector3(MathHelper.WrapAngle(vector.Pitch), MathHelper.WrapAngle(vector.Yaw), MathHelper.WrapAngle(vector.Roll));
		}

		// Token: 0x06003818 RID: 14360 RVA: 0x0007187C File Offset: 0x0006FA7C
		public static Vector3 CastToInts(Vector3 vector)
		{
			return new Vector3((float)((int)vector.X), (float)((int)vector.Y), (float)((int)vector.Z));
		}

		// Token: 0x06003819 RID: 14361 RVA: 0x000718AC File Offset: 0x0006FAAC
		public static Vector3 Floor(Vector3 vector)
		{
			return new Vector3((float)Math.Floor((double)vector.X), (float)Math.Floor((double)vector.Y), (float)Math.Floor((double)vector.Z));
		}

		// Token: 0x0600381A RID: 14362 RVA: 0x000718EC File Offset: 0x0006FAEC
		public static void ScreenToWorldRay(Vector2 screenPoint, Vector3 cameraPosition, Matrix invViewProjection, out Vector3 position, out Vector3 direction)
		{
			Vector3 vector = new Vector3(screenPoint.X, -screenPoint.Y, 0f);
			position = Vector3.Unproject(ref vector, ref invViewProjection);
			direction = Vector3.Normalize(position - cameraPosition);
		}

		// Token: 0x0600381B RID: 14363 RVA: 0x0007193C File Offset: 0x0006FB3C
		public static Vector2 WorldToScreenPos(ref Matrix viewProjectionMatrix, float viewWidth, float viewHeight, Vector3 worldPosition)
		{
			Matrix matrix = Matrix.CreateTranslation(worldPosition);
			Matrix.Multiply(ref matrix, ref viewProjectionMatrix, out matrix);
			Vector3 vector = matrix.Translation / matrix.M44;
			return new Vector2((vector.X / 2f + 0.5f) * viewWidth, (vector.Y / 2f + 0.5f) * viewHeight);
		}

		// Token: 0x0600381C RID: 14364 RVA: 0x000719A0 File Offset: 0x0006FBA0
		public static Vector3 Unproject(ref Vector3 screenPoint, ref Matrix invViewProjection)
		{
			Vector3 vector;
			Vector3.Transform(ref screenPoint, ref invViewProjection, out vector);
			float num = screenPoint.X * invViewProjection.M14 + screenPoint.Y * invViewProjection.M24 + screenPoint.Z * invViewProjection.M34 + invViewProjection.M44;
			bool flag = !MathHelper.WithinEpsilon(num, 1f);
			if (flag)
			{
				vector.X /= num;
				vector.Y /= num;
				vector.Z /= num;
			}
			return vector;
		}

		// Token: 0x0600381D RID: 14365 RVA: 0x00071A30 File Offset: 0x0006FC30
		public static Vector3 ProjectOnPlane(Vector3 vector, Vector3 planeNormal)
		{
			float num = Vector3.Dot(planeNormal, planeNormal);
			bool flag = (double)num < double.Epsilon;
			Vector3 result;
			if (flag)
			{
				result = vector;
			}
			else
			{
				float num2 = Vector3.Dot(vector, planeNormal);
				result = new Vector3(vector.X - planeNormal.X * num2 / num, vector.Y - planeNormal.Y * num2 / num, vector.Z - planeNormal.Z * num2 / num);
			}
			return result;
		}

		// Token: 0x0600381E RID: 14366 RVA: 0x00071AA0 File Offset: 0x0006FCA0
		public Vector3 SetLength(float newLen)
		{
			return this * (newLen / this.Length());
		}

		// Token: 0x04001850 RID: 6224
		private static Vector3 zero = new Vector3(0f, 0f, 0f);

		// Token: 0x04001851 RID: 6225
		private static readonly Vector3 one = new Vector3(1f, 1f, 1f);

		// Token: 0x04001852 RID: 6226
		private static readonly Vector3 unitX = new Vector3(1f, 0f, 0f);

		// Token: 0x04001853 RID: 6227
		private static readonly Vector3 unitY = new Vector3(0f, 1f, 0f);

		// Token: 0x04001854 RID: 6228
		private static readonly Vector3 unitZ = new Vector3(0f, 0f, 1f);

		// Token: 0x04001855 RID: 6229
		private static readonly Vector3 up = new Vector3(0f, 1f, 0f);

		// Token: 0x04001856 RID: 6230
		private static readonly Vector3 down = new Vector3(0f, -1f, 0f);

		// Token: 0x04001857 RID: 6231
		private static readonly Vector3 right = new Vector3(1f, 0f, 0f);

		// Token: 0x04001858 RID: 6232
		private static readonly Vector3 left = new Vector3(-1f, 0f, 0f);

		// Token: 0x04001859 RID: 6233
		private static readonly Vector3 forward = new Vector3(0f, 0f, -1f);

		// Token: 0x0400185A RID: 6234
		private static readonly Vector3 backward = new Vector3(0f, 0f, 1f);

		// Token: 0x0400185B RID: 6235
		[CoherentProperty("x")]
		public float X;

		// Token: 0x0400185C RID: 6236
		[CoherentProperty("y")]
		public float Y;

		// Token: 0x0400185D RID: 6237
		[CoherentProperty("z")]
		public float Z;

		// Token: 0x0400185E RID: 6238
		private static readonly Vector3 nan = new Vector3(float.NaN, float.NaN, float.NaN);
	}
}
