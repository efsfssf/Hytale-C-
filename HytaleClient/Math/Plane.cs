using System;
using System.Diagnostics;

namespace HytaleClient.Math
{
	// Token: 0x020007EF RID: 2031
	[DebuggerDisplay("{DebugDisplayString,nq}")]
	[Serializable]
	public struct Plane : IEquatable<Plane>
	{
		// Token: 0x17000FD7 RID: 4055
		// (get) Token: 0x060036C1 RID: 14017 RVA: 0x0006B61C File Offset: 0x0006981C
		internal string DebugDisplayString
		{
			get
			{
				return this.Normal.DebugDisplayString + " " + this.D.ToString();
			}
		}

		// Token: 0x060036C2 RID: 14018 RVA: 0x0006B64E File Offset: 0x0006984E
		public Plane(Vector4 value)
		{
			this = new Plane(new Vector3(value.X, value.Y, value.Z), value.W);
		}

		// Token: 0x060036C3 RID: 14019 RVA: 0x0006B675 File Offset: 0x00069875
		public Plane(Vector3 normal, float d)
		{
			this.Normal = normal;
			this.D = d;
		}

		// Token: 0x060036C4 RID: 14020 RVA: 0x0006B688 File Offset: 0x00069888
		public Plane(Vector3 a, Vector3 b, Vector3 c)
		{
			Vector3 vector = b - a;
			Vector3 vector2 = c - a;
			Vector3 value = Vector3.Cross(vector, vector2);
			this.Normal = Vector3.Normalize(value);
			this.D = -Vector3.Dot(this.Normal, a);
		}

		// Token: 0x060036C5 RID: 14021 RVA: 0x0006B6CD File Offset: 0x000698CD
		public Plane(float a, float b, float c, float d)
		{
			this = new Plane(new Vector3(a, b, c), d);
		}

		// Token: 0x060036C6 RID: 14022 RVA: 0x0006B6E4 File Offset: 0x000698E4
		public float Dot(Vector4 value)
		{
			return this.Normal.X * value.X + this.Normal.Y * value.Y + this.Normal.Z * value.Z + this.D * value.W;
		}

		// Token: 0x060036C7 RID: 14023 RVA: 0x0006B73C File Offset: 0x0006993C
		public void Dot(ref Vector4 value, out float result)
		{
			result = this.Normal.X * value.X + this.Normal.Y * value.Y + this.Normal.Z * value.Z + this.D * value.W;
		}

		// Token: 0x060036C8 RID: 14024 RVA: 0x0006B794 File Offset: 0x00069994
		public float DotCoordinate(Vector3 value)
		{
			return this.Normal.X * value.X + this.Normal.Y * value.Y + this.Normal.Z * value.Z + this.D;
		}

		// Token: 0x060036C9 RID: 14025 RVA: 0x0006B7E8 File Offset: 0x000699E8
		public void DotCoordinate(ref Vector3 value, out float result)
		{
			result = this.Normal.X * value.X + this.Normal.Y * value.Y + this.Normal.Z * value.Z + this.D;
		}

		// Token: 0x060036CA RID: 14026 RVA: 0x0006B838 File Offset: 0x00069A38
		public float DotNormal(Vector3 value)
		{
			return this.Normal.X * value.X + this.Normal.Y * value.Y + this.Normal.Z * value.Z;
		}

		// Token: 0x060036CB RID: 14027 RVA: 0x0006B882 File Offset: 0x00069A82
		public void DotNormal(ref Vector3 value, out float result)
		{
			result = this.Normal.X * value.X + this.Normal.Y * value.Y + this.Normal.Z * value.Z;
		}

		// Token: 0x060036CC RID: 14028 RVA: 0x0006B8C0 File Offset: 0x00069AC0
		public void Normalize()
		{
			Vector3 normal = this.Normal;
			this.Normal = Vector3.Normalize(this.Normal);
			float num = (float)Math.Sqrt((double)(this.Normal.X * this.Normal.X + this.Normal.Y * this.Normal.Y + this.Normal.Z * this.Normal.Z)) / (float)Math.Sqrt((double)(normal.X * normal.X + normal.Y * normal.Y + normal.Z * normal.Z));
			this.D *= num;
		}

		// Token: 0x060036CD RID: 14029 RVA: 0x0006B974 File Offset: 0x00069B74
		public PlaneIntersectionType Intersects(BoundingBox box)
		{
			return box.Intersects(this);
		}

		// Token: 0x060036CE RID: 14030 RVA: 0x0006B993 File Offset: 0x00069B93
		public void Intersects(ref BoundingBox box, out PlaneIntersectionType result)
		{
			box.Intersects(ref this, out result);
		}

		// Token: 0x060036CF RID: 14031 RVA: 0x0006B9A0 File Offset: 0x00069BA0
		public PlaneIntersectionType Intersects(BoundingSphere sphere)
		{
			return sphere.Intersects(this);
		}

		// Token: 0x060036D0 RID: 14032 RVA: 0x0006B9BF File Offset: 0x00069BBF
		public void Intersects(ref BoundingSphere sphere, out PlaneIntersectionType result)
		{
			sphere.Intersects(ref this, out result);
		}

		// Token: 0x060036D1 RID: 14033 RVA: 0x0006B9CC File Offset: 0x00069BCC
		public PlaneIntersectionType Intersects(BoundingFrustum frustum)
		{
			return frustum.Intersects(this);
		}

		// Token: 0x060036D2 RID: 14034 RVA: 0x0006B9EC File Offset: 0x00069BEC
		internal PlaneIntersectionType Intersects(ref Vector3 point)
		{
			float num;
			this.DotCoordinate(ref point, out num);
			bool flag = num > 0f;
			PlaneIntersectionType result;
			if (flag)
			{
				result = PlaneIntersectionType.Front;
			}
			else
			{
				bool flag2 = num < 0f;
				if (flag2)
				{
					result = PlaneIntersectionType.Back;
				}
				else
				{
					result = PlaneIntersectionType.Intersecting;
				}
			}
			return result;
		}

		// Token: 0x060036D3 RID: 14035 RVA: 0x0006BA2C File Offset: 0x00069C2C
		public static Plane Normalize(Plane value)
		{
			Plane result;
			Plane.Normalize(ref value, out result);
			return result;
		}

		// Token: 0x060036D4 RID: 14036 RVA: 0x0006BA4C File Offset: 0x00069C4C
		public static void Normalize(ref Plane value, out Plane result)
		{
			result.Normal = Vector3.Normalize(value.Normal);
			float num = (float)Math.Sqrt((double)(result.Normal.X * result.Normal.X + result.Normal.Y * result.Normal.Y + result.Normal.Z * result.Normal.Z)) / (float)Math.Sqrt((double)(value.Normal.X * value.Normal.X + value.Normal.Y * value.Normal.Y + value.Normal.Z * value.Normal.Z));
			result.D = value.D * num;
		}

		// Token: 0x060036D5 RID: 14037 RVA: 0x0006BB18 File Offset: 0x00069D18
		public static Plane Transform(Plane plane, Matrix matrix)
		{
			Plane result;
			Plane.Transform(ref plane, ref matrix, out result);
			return result;
		}

		// Token: 0x060036D6 RID: 14038 RVA: 0x0006BB38 File Offset: 0x00069D38
		public static void Transform(ref Plane plane, ref Matrix matrix, out Plane result)
		{
			Matrix matrix2;
			Matrix.Invert(ref matrix, out matrix2);
			Matrix.Transpose(ref matrix2, out matrix2);
			Vector4 vector = new Vector4(plane.Normal, plane.D);
			Vector4 value;
			Vector4.Transform(ref vector, ref matrix2, out value);
			result = new Plane(value);
		}

		// Token: 0x060036D7 RID: 14039 RVA: 0x0006BB84 File Offset: 0x00069D84
		public static Plane Transform(Plane plane, Quaternion rotation)
		{
			Plane result;
			Plane.Transform(ref plane, ref rotation, out result);
			return result;
		}

		// Token: 0x060036D8 RID: 14040 RVA: 0x0006BBA3 File Offset: 0x00069DA3
		public static void Transform(ref Plane plane, ref Quaternion rotation, out Plane result)
		{
			Vector3.Transform(ref plane.Normal, ref rotation, out result.Normal);
			result.D = plane.D;
		}

		// Token: 0x060036D9 RID: 14041 RVA: 0x0006BBC8 File Offset: 0x00069DC8
		public static bool operator !=(Plane plane1, Plane plane2)
		{
			return !plane1.Equals(plane2);
		}

		// Token: 0x060036DA RID: 14042 RVA: 0x0006BBE8 File Offset: 0x00069DE8
		public static bool operator ==(Plane plane1, Plane plane2)
		{
			return plane1.Equals(plane2);
		}

		// Token: 0x060036DB RID: 14043 RVA: 0x0006BC04 File Offset: 0x00069E04
		public override bool Equals(object obj)
		{
			return obj is Plane && this.Equals((Plane)obj);
		}

		// Token: 0x060036DC RID: 14044 RVA: 0x0006BC30 File Offset: 0x00069E30
		public bool Equals(Plane other)
		{
			return this.Normal == other.Normal && this.D == other.D;
		}

		// Token: 0x060036DD RID: 14045 RVA: 0x0006BC68 File Offset: 0x00069E68
		public override int GetHashCode()
		{
			return this.Normal.GetHashCode() ^ this.D.GetHashCode();
		}

		// Token: 0x060036DE RID: 14046 RVA: 0x0006BC98 File Offset: 0x00069E98
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{Normal:",
				this.Normal.ToString(),
				" D:",
				this.D.ToString(),
				"}"
			});
		}

		// Token: 0x04001834 RID: 6196
		public Vector3 Normal;

		// Token: 0x04001835 RID: 6197
		public float D;
	}
}
