using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HytaleClient.Math
{
	// Token: 0x020007E1 RID: 2017
	[DebuggerDisplay("{DebugDisplayString,nq}")]
	[Serializable]
	public struct BoundingSphere : IEquatable<BoundingSphere>
	{
		// Token: 0x17000FB5 RID: 4021
		// (get) Token: 0x0600358E RID: 13710 RVA: 0x000627AC File Offset: 0x000609AC
		internal string DebugDisplayString
		{
			get
			{
				return string.Concat(new string[]
				{
					"Center( ",
					this.Center.DebugDisplayString,
					" ) \r\n",
					"Radius( ",
					this.Radius.ToString(),
					" ) "
				});
			}
		}

		// Token: 0x0600358F RID: 13711 RVA: 0x00062805 File Offset: 0x00060A05
		public BoundingSphere(Vector3 center, float radius)
		{
			this.Center = center;
			this.Radius = radius;
		}

		// Token: 0x06003590 RID: 13712 RVA: 0x00062818 File Offset: 0x00060A18
		public BoundingSphere Transform(Matrix matrix)
		{
			return new BoundingSphere
			{
				Center = Vector3.Transform(this.Center, matrix),
				Radius = this.Radius * (float)Math.Sqrt((double)Math.Max(matrix.M11 * matrix.M11 + matrix.M12 * matrix.M12 + matrix.M13 * matrix.M13, Math.Max(matrix.M21 * matrix.M21 + matrix.M22 * matrix.M22 + matrix.M23 * matrix.M23, matrix.M31 * matrix.M31 + matrix.M32 * matrix.M32 + matrix.M33 * matrix.M33)))
			};
		}

		// Token: 0x06003591 RID: 13713 RVA: 0x000628E0 File Offset: 0x00060AE0
		public void Transform(ref Matrix matrix, out BoundingSphere result)
		{
			result.Center = Vector3.Transform(this.Center, matrix);
			result.Radius = this.Radius * (float)Math.Sqrt((double)Math.Max(matrix.M11 * matrix.M11 + matrix.M12 * matrix.M12 + matrix.M13 * matrix.M13, Math.Max(matrix.M21 * matrix.M21 + matrix.M22 * matrix.M22 + matrix.M23 * matrix.M23, matrix.M31 * matrix.M31 + matrix.M32 * matrix.M32 + matrix.M33 * matrix.M33)));
		}

		// Token: 0x06003592 RID: 13714 RVA: 0x0006299E File Offset: 0x00060B9E
		public void Contains(ref BoundingBox box, out ContainmentType result)
		{
			result = this.Contains(box);
		}

		// Token: 0x06003593 RID: 13715 RVA: 0x000629AF File Offset: 0x00060BAF
		public void Contains(ref BoundingSphere sphere, out ContainmentType result)
		{
			result = this.Contains(sphere);
		}

		// Token: 0x06003594 RID: 13716 RVA: 0x000629C0 File Offset: 0x00060BC0
		public void Contains(ref Vector3 point, out ContainmentType result)
		{
			result = this.Contains(point);
		}

		// Token: 0x06003595 RID: 13717 RVA: 0x000629D4 File Offset: 0x00060BD4
		public ContainmentType Contains(BoundingBox box)
		{
			bool flag = true;
			foreach (Vector3 point in box.GetCorners())
			{
				bool flag2 = this.Contains(point) == ContainmentType.Disjoint;
				if (flag2)
				{
					flag = false;
					break;
				}
			}
			bool flag3 = flag;
			ContainmentType result;
			if (flag3)
			{
				result = ContainmentType.Contains;
			}
			else
			{
				double num = 0.0;
				bool flag4 = this.Center.X < box.Min.X;
				if (flag4)
				{
					num += (double)((this.Center.X - box.Min.X) * (this.Center.X - box.Min.X));
				}
				else
				{
					bool flag5 = this.Center.X > box.Max.X;
					if (flag5)
					{
						num += (double)((this.Center.X - box.Max.X) * (this.Center.X - box.Max.X));
					}
				}
				bool flag6 = this.Center.Y < box.Min.Y;
				if (flag6)
				{
					num += (double)((this.Center.Y - box.Min.Y) * (this.Center.Y - box.Min.Y));
				}
				else
				{
					bool flag7 = this.Center.Y > box.Max.Y;
					if (flag7)
					{
						num += (double)((this.Center.Y - box.Max.Y) * (this.Center.Y - box.Max.Y));
					}
				}
				bool flag8 = this.Center.Z < box.Min.Z;
				if (flag8)
				{
					num += (double)((this.Center.Z - box.Min.Z) * (this.Center.Z - box.Min.Z));
				}
				else
				{
					bool flag9 = this.Center.Z > box.Max.Z;
					if (flag9)
					{
						num += (double)((this.Center.Z - box.Max.Z) * (this.Center.Z - box.Max.Z));
					}
				}
				bool flag10 = num <= (double)(this.Radius * this.Radius);
				if (flag10)
				{
					result = ContainmentType.Intersects;
				}
				else
				{
					result = ContainmentType.Disjoint;
				}
			}
			return result;
		}

		// Token: 0x06003596 RID: 13718 RVA: 0x00062C58 File Offset: 0x00060E58
		public ContainmentType Contains(BoundingFrustum frustum)
		{
			bool flag = true;
			Vector3[] corners = frustum.GetCorners();
			foreach (Vector3 point in corners)
			{
				bool flag2 = this.Contains(point) == ContainmentType.Disjoint;
				if (flag2)
				{
					flag = false;
					break;
				}
			}
			bool flag3 = flag;
			ContainmentType result;
			if (flag3)
			{
				result = ContainmentType.Contains;
			}
			else
			{
				double num = 0.0;
				bool flag4 = num <= (double)(this.Radius * this.Radius);
				if (flag4)
				{
					result = ContainmentType.Intersects;
				}
				else
				{
					result = ContainmentType.Disjoint;
				}
			}
			return result;
		}

		// Token: 0x06003597 RID: 13719 RVA: 0x00062CE4 File Offset: 0x00060EE4
		public ContainmentType Contains(BoundingSphere sphere)
		{
			float num;
			Vector3.DistanceSquared(ref sphere.Center, ref this.Center, out num);
			bool flag = num > (sphere.Radius + this.Radius) * (sphere.Radius + this.Radius);
			ContainmentType result;
			if (flag)
			{
				result = ContainmentType.Disjoint;
			}
			else
			{
				bool flag2 = num <= this.Radius * sphere.Radius * (this.Radius - sphere.Radius);
				if (flag2)
				{
					result = ContainmentType.Contains;
				}
				else
				{
					result = ContainmentType.Intersects;
				}
			}
			return result;
		}

		// Token: 0x06003598 RID: 13720 RVA: 0x00062D60 File Offset: 0x00060F60
		public ContainmentType Contains(Vector3 point)
		{
			float num = this.Radius * this.Radius;
			float num2;
			Vector3.DistanceSquared(ref point, ref this.Center, out num2);
			bool flag = num2 > num;
			ContainmentType result;
			if (flag)
			{
				result = ContainmentType.Disjoint;
			}
			else
			{
				bool flag2 = num2 < num;
				if (flag2)
				{
					result = ContainmentType.Contains;
				}
				else
				{
					result = ContainmentType.Intersects;
				}
			}
			return result;
		}

		// Token: 0x06003599 RID: 13721 RVA: 0x00062DB0 File Offset: 0x00060FB0
		public bool Equals(BoundingSphere other)
		{
			return this.Center == other.Center && this.Radius == other.Radius;
		}

		// Token: 0x0600359A RID: 13722 RVA: 0x00062DE8 File Offset: 0x00060FE8
		public static BoundingSphere CreateFromBoundingBox(BoundingBox box)
		{
			BoundingSphere result;
			BoundingSphere.CreateFromBoundingBox(ref box, out result);
			return result;
		}

		// Token: 0x0600359B RID: 13723 RVA: 0x00062E08 File Offset: 0x00061008
		public static void CreateFromBoundingBox(ref BoundingBox box, out BoundingSphere result)
		{
			Vector3 vector = new Vector3((box.Min.X + box.Max.X) * 0.5f, (box.Min.Y + box.Max.Y) * 0.5f, (box.Min.Z + box.Max.Z) * 0.5f);
			float radius = Vector3.Distance(vector, box.Max);
			result = new BoundingSphere(vector, radius);
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x00062E90 File Offset: 0x00061090
		public static BoundingSphere CreateFromFrustum(BoundingFrustum frustum)
		{
			return BoundingSphere.CreateFromPoints(frustum.GetCorners());
		}

		// Token: 0x0600359D RID: 13725 RVA: 0x00062EB0 File Offset: 0x000610B0
		public static BoundingSphere CreateFromPoints(IEnumerable<Vector3> points)
		{
			bool flag = points == null;
			if (flag)
			{
				throw new ArgumentNullException("points");
			}
			Vector3 vector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			Vector3 vector2 = -vector;
			Vector3 vector3 = vector;
			Vector3 vector4 = -vector;
			Vector3 vector5 = vector;
			Vector3 vector6 = -vector;
			int num = 0;
			foreach (Vector3 vector7 in points)
			{
				num++;
				bool flag2 = vector7.X < vector.X;
				if (flag2)
				{
					vector = vector7;
				}
				bool flag3 = vector7.X > vector2.X;
				if (flag3)
				{
					vector2 = vector7;
				}
				bool flag4 = vector7.Y < vector3.Y;
				if (flag4)
				{
					vector3 = vector7;
				}
				bool flag5 = vector7.Y > vector4.Y;
				if (flag5)
				{
					vector4 = vector7;
				}
				bool flag6 = vector7.Z < vector5.Z;
				if (flag6)
				{
					vector5 = vector7;
				}
				bool flag7 = vector7.Z > vector6.Z;
				if (flag7)
				{
					vector6 = vector7;
				}
			}
			bool flag8 = num == 0;
			if (flag8)
			{
				throw new ArgumentException("You should have at least one point in points.");
			}
			float num2 = Vector3.DistanceSquared(vector2, vector);
			float num3 = Vector3.DistanceSquared(vector4, vector3);
			float num4 = Vector3.DistanceSquared(vector6, vector5);
			Vector3 value = vector;
			Vector3 vector8 = vector2;
			bool flag9 = num3 > num2 && num3 > num4;
			if (flag9)
			{
				vector8 = vector4;
				value = vector3;
			}
			bool flag10 = num4 > num2 && num4 > num3;
			if (flag10)
			{
				vector8 = vector6;
				value = vector5;
			}
			Vector3 vector9 = (value + vector8) * 0.5f;
			float num5 = Vector3.Distance(vector8, vector9);
			float num6 = num5 * num5;
			foreach (Vector3 vector10 in points)
			{
				Vector3 value2 = vector10 - vector9;
				float num7 = value2.LengthSquared();
				bool flag11 = num7 > num6;
				if (flag11)
				{
					float divider = (float)Math.Sqrt((double)num7);
					Vector3 value3 = value2 / divider;
					Vector3 value4 = vector9 - num5 * value3;
					vector9 = (value4 + vector10) * 0.5f;
					num5 = Vector3.Distance(vector10, vector9);
					num6 = num5 * num5;
				}
			}
			return new BoundingSphere(vector9, num5);
		}

		// Token: 0x0600359E RID: 13726 RVA: 0x0006314C File Offset: 0x0006134C
		public static BoundingSphere CreateMerged(BoundingSphere original, BoundingSphere additional)
		{
			BoundingSphere result;
			BoundingSphere.CreateMerged(ref original, ref additional, out result);
			return result;
		}

		// Token: 0x0600359F RID: 13727 RVA: 0x0006316C File Offset: 0x0006136C
		public static void CreateMerged(ref BoundingSphere original, ref BoundingSphere additional, out BoundingSphere result)
		{
			Vector3 vector = Vector3.Subtract(additional.Center, original.Center);
			float num = vector.Length();
			bool flag = num <= original.Radius + additional.Radius;
			if (flag)
			{
				bool flag2 = num <= original.Radius - additional.Radius;
				if (flag2)
				{
					result = original;
					return;
				}
				bool flag3 = num <= additional.Radius - original.Radius;
				if (flag3)
				{
					result = additional;
					return;
				}
			}
			float num2 = Math.Max(original.Radius - num, additional.Radius);
			float num3 = Math.Max(original.Radius + num, additional.Radius);
			vector += (num2 - num3) / (2f * vector.Length()) * vector;
			result = default(BoundingSphere);
			result.Center = original.Center + vector;
			result.Radius = (num2 + num3) * 0.5f;
		}

		// Token: 0x060035A0 RID: 13728 RVA: 0x00063270 File Offset: 0x00061470
		public bool Intersects(BoundingBox box)
		{
			return box.Intersects(this);
		}

		// Token: 0x060035A1 RID: 13729 RVA: 0x0006328F File Offset: 0x0006148F
		public void Intersects(ref BoundingBox box, out bool result)
		{
			box.Intersects(ref this, out result);
		}

		// Token: 0x060035A2 RID: 13730 RVA: 0x0006329C File Offset: 0x0006149C
		public bool Intersects(BoundingFrustum frustum)
		{
			return frustum.Intersects(this);
		}

		// Token: 0x060035A3 RID: 13731 RVA: 0x000632BC File Offset: 0x000614BC
		public bool Intersects(BoundingSphere sphere)
		{
			bool result;
			this.Intersects(ref sphere, out result);
			return result;
		}

		// Token: 0x060035A4 RID: 13732 RVA: 0x000632DC File Offset: 0x000614DC
		public void Intersects(ref BoundingSphere sphere, out bool result)
		{
			float num;
			Vector3.DistanceSquared(ref sphere.Center, ref this.Center, out num);
			result = (num <= (sphere.Radius + this.Radius) * (sphere.Radius + this.Radius));
		}

		// Token: 0x060035A5 RID: 13733 RVA: 0x00063324 File Offset: 0x00061524
		public float? Intersects(Ray ray)
		{
			return ray.Intersects(this);
		}

		// Token: 0x060035A6 RID: 13734 RVA: 0x00063343 File Offset: 0x00061543
		public void Intersects(ref Ray ray, out float? result)
		{
			ray.Intersects(ref this, out result);
		}

		// Token: 0x060035A7 RID: 13735 RVA: 0x00063350 File Offset: 0x00061550
		public PlaneIntersectionType Intersects(Plane plane)
		{
			PlaneIntersectionType result = PlaneIntersectionType.Front;
			this.Intersects(ref plane, out result);
			return result;
		}

		// Token: 0x060035A8 RID: 13736 RVA: 0x00063370 File Offset: 0x00061570
		public void Intersects(ref Plane plane, out PlaneIntersectionType result)
		{
			float num = 0f;
			Vector3.Dot(ref plane.Normal, ref this.Center, out num);
			num += plane.D;
			bool flag = num > this.Radius;
			if (flag)
			{
				result = PlaneIntersectionType.Front;
			}
			else
			{
				bool flag2 = num < -this.Radius;
				if (flag2)
				{
					result = PlaneIntersectionType.Back;
				}
				else
				{
					result = PlaneIntersectionType.Intersecting;
				}
			}
		}

		// Token: 0x060035A9 RID: 13737 RVA: 0x000633D0 File Offset: 0x000615D0
		public override bool Equals(object obj)
		{
			return obj is BoundingSphere && this.Equals((BoundingSphere)obj);
		}

		// Token: 0x060035AA RID: 13738 RVA: 0x000633FC File Offset: 0x000615FC
		public override int GetHashCode()
		{
			return this.Center.GetHashCode() + this.Radius.GetHashCode();
		}

		// Token: 0x060035AB RID: 13739 RVA: 0x0006342C File Offset: 0x0006162C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{Center:",
				this.Center.ToString(),
				" Radius:",
				this.Radius.ToString(),
				"}"
			});
		}

		// Token: 0x060035AC RID: 13740 RVA: 0x00063484 File Offset: 0x00061684
		public static bool operator ==(BoundingSphere a, BoundingSphere b)
		{
			return a.Equals(b);
		}

		// Token: 0x060035AD RID: 13741 RVA: 0x000634A0 File Offset: 0x000616A0
		public static bool operator !=(BoundingSphere a, BoundingSphere b)
		{
			return !a.Equals(b);
		}

		// Token: 0x040017E7 RID: 6119
		public Vector3 Center;

		// Token: 0x040017E8 RID: 6120
		public float Radius;
	}
}
