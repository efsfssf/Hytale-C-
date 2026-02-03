using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HytaleClient.Math
{
	// Token: 0x020007DF RID: 2015
	[DebuggerDisplay("{DebugDisplayString,nq}")]
	[Serializable]
	public struct BoundingBox : IEquatable<BoundingBox>
	{
		// Token: 0x17000FAC RID: 4012
		// (get) Token: 0x06003538 RID: 13624 RVA: 0x0005FAB0 File Offset: 0x0005DCB0
		internal string DebugDisplayString
		{
			get
			{
				return string.Concat(new string[]
				{
					"Min( ",
					this.Min.DebugDisplayString,
					" ) \r\n",
					"Max( ",
					this.Max.DebugDisplayString,
					" )"
				});
			}
		}

		// Token: 0x06003539 RID: 13625 RVA: 0x0005FB09 File Offset: 0x0005DD09
		public BoundingBox(Vector3 min, Vector3 max)
		{
			this.Min = min;
			this.Max = max;
		}

		// Token: 0x0600353A RID: 13626 RVA: 0x0005FB1A File Offset: 0x0005DD1A
		public void Contains(ref BoundingBox box, out ContainmentType result)
		{
			result = this.Contains(box);
		}

		// Token: 0x0600353B RID: 13627 RVA: 0x0005FB2B File Offset: 0x0005DD2B
		public void Contains(ref BoundingSphere sphere, out ContainmentType result)
		{
			result = this.Contains(sphere);
		}

		// Token: 0x0600353C RID: 13628 RVA: 0x0005FB3C File Offset: 0x0005DD3C
		public ContainmentType Contains(Vector3 point)
		{
			ContainmentType result;
			this.Contains(ref point, out result);
			return result;
		}

		// Token: 0x0600353D RID: 13629 RVA: 0x0005FB5C File Offset: 0x0005DD5C
		public ContainmentType Contains(BoundingBox box)
		{
			bool flag = box.Max.X < this.Min.X || box.Min.X > this.Max.X || box.Max.Y < this.Min.Y || box.Min.Y > this.Max.Y || box.Max.Z < this.Min.Z || box.Min.Z > this.Max.Z;
			ContainmentType result;
			if (flag)
			{
				result = ContainmentType.Disjoint;
			}
			else
			{
				bool flag2 = box.Min.X >= this.Min.X && box.Max.X <= this.Max.X && box.Min.Y >= this.Min.Y && box.Max.Y <= this.Max.Y && box.Min.Z >= this.Min.Z && box.Max.Z <= this.Max.Z;
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

		// Token: 0x0600353E RID: 13630 RVA: 0x0005FCB0 File Offset: 0x0005DEB0
		public ContainmentType Contains(BoundingFrustum frustum)
		{
			Vector3[] corners = frustum.GetCorners();
			int i;
			for (i = 0; i < corners.Length; i++)
			{
				ContainmentType containmentType;
				this.Contains(ref corners[i], out containmentType);
				bool flag = containmentType == ContainmentType.Disjoint;
				if (flag)
				{
					break;
				}
			}
			bool flag2 = i == corners.Length;
			ContainmentType result;
			if (flag2)
			{
				result = ContainmentType.Contains;
			}
			else
			{
				bool flag3 = i != 0;
				if (flag3)
				{
					result = ContainmentType.Intersects;
				}
				else
				{
					for (i++; i < corners.Length; i++)
					{
						ContainmentType containmentType;
						this.Contains(ref corners[i], out containmentType);
						bool flag4 = containmentType != ContainmentType.Contains;
						if (flag4)
						{
							return ContainmentType.Intersects;
						}
					}
					result = ContainmentType.Contains;
				}
			}
			return result;
		}

		// Token: 0x0600353F RID: 13631 RVA: 0x0005FD5C File Offset: 0x0005DF5C
		public ContainmentType Contains(BoundingSphere sphere)
		{
			bool flag = sphere.Center.X - this.Min.X >= sphere.Radius && sphere.Center.Y - this.Min.Y >= sphere.Radius && sphere.Center.Z - this.Min.Z >= sphere.Radius && this.Max.X - sphere.Center.X >= sphere.Radius && this.Max.Y - sphere.Center.Y >= sphere.Radius && this.Max.Z - sphere.Center.Z >= sphere.Radius;
			ContainmentType result;
			if (flag)
			{
				result = ContainmentType.Contains;
			}
			else
			{
				double num = 0.0;
				double num2 = (double)(sphere.Center.X - this.Min.X);
				bool flag2 = num2 < 0.0;
				if (flag2)
				{
					bool flag3 = num2 < (double)(-(double)sphere.Radius);
					if (flag3)
					{
						return ContainmentType.Disjoint;
					}
					num += num2 * num2;
				}
				else
				{
					num2 = (double)(sphere.Center.X - this.Max.X);
					bool flag4 = num2 > 0.0;
					if (flag4)
					{
						bool flag5 = num2 > (double)sphere.Radius;
						if (flag5)
						{
							return ContainmentType.Disjoint;
						}
						num += num2 * num2;
					}
				}
				num2 = (double)(sphere.Center.Y - this.Min.Y);
				bool flag6 = num2 < 0.0;
				if (flag6)
				{
					bool flag7 = num2 < (double)(-(double)sphere.Radius);
					if (flag7)
					{
						return ContainmentType.Disjoint;
					}
					num += num2 * num2;
				}
				else
				{
					num2 = (double)(sphere.Center.Y - this.Max.Y);
					bool flag8 = num2 > 0.0;
					if (flag8)
					{
						bool flag9 = num2 > (double)sphere.Radius;
						if (flag9)
						{
							return ContainmentType.Disjoint;
						}
						num += num2 * num2;
					}
				}
				num2 = (double)(sphere.Center.Z - this.Min.Z);
				bool flag10 = num2 < 0.0;
				if (flag10)
				{
					bool flag11 = num2 < (double)(-(double)sphere.Radius);
					if (flag11)
					{
						return ContainmentType.Disjoint;
					}
					num += num2 * num2;
				}
				else
				{
					num2 = (double)(sphere.Center.Z - this.Max.Z);
					bool flag12 = num2 > 0.0;
					if (flag12)
					{
						bool flag13 = num2 > (double)sphere.Radius;
						if (flag13)
						{
							return ContainmentType.Disjoint;
						}
						num += num2 * num2;
					}
				}
				bool flag14 = num <= (double)(sphere.Radius * sphere.Radius);
				if (flag14)
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

		// Token: 0x06003540 RID: 13632 RVA: 0x00060038 File Offset: 0x0005E238
		public void Contains(ref Vector3 point, out ContainmentType result)
		{
			bool flag = point.X < this.Min.X || point.X > this.Max.X || point.Y < this.Min.Y || point.Y > this.Max.Y || point.Z < this.Min.Z || point.Z > this.Max.Z;
			if (flag)
			{
				result = ContainmentType.Disjoint;
			}
			else
			{
				result = ContainmentType.Contains;
			}
		}

		// Token: 0x06003541 RID: 13633 RVA: 0x000600CC File Offset: 0x0005E2CC
		public Vector3[] GetCorners()
		{
			return new Vector3[]
			{
				new Vector3(this.Min.X, this.Max.Y, this.Max.Z),
				new Vector3(this.Max.X, this.Max.Y, this.Max.Z),
				new Vector3(this.Max.X, this.Min.Y, this.Max.Z),
				new Vector3(this.Min.X, this.Min.Y, this.Max.Z),
				new Vector3(this.Min.X, this.Max.Y, this.Min.Z),
				new Vector3(this.Max.X, this.Max.Y, this.Min.Z),
				new Vector3(this.Max.X, this.Min.Y, this.Min.Z),
				new Vector3(this.Min.X, this.Min.Y, this.Min.Z)
			};
		}

		// Token: 0x06003542 RID: 13634 RVA: 0x0006024C File Offset: 0x0005E44C
		public void GetCorners(Vector3[] corners)
		{
			bool flag = corners == null;
			if (flag)
			{
				throw new ArgumentNullException("corners");
			}
			bool flag2 = corners.Length < 8;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException("corners", "Not Enought Corners");
			}
			corners[0].X = this.Min.X;
			corners[0].Y = this.Max.Y;
			corners[0].Z = this.Max.Z;
			corners[1].X = this.Max.X;
			corners[1].Y = this.Max.Y;
			corners[1].Z = this.Max.Z;
			corners[2].X = this.Max.X;
			corners[2].Y = this.Min.Y;
			corners[2].Z = this.Max.Z;
			corners[3].X = this.Min.X;
			corners[3].Y = this.Min.Y;
			corners[3].Z = this.Max.Z;
			corners[4].X = this.Min.X;
			corners[4].Y = this.Max.Y;
			corners[4].Z = this.Min.Z;
			corners[5].X = this.Max.X;
			corners[5].Y = this.Max.Y;
			corners[5].Z = this.Min.Z;
			corners[6].X = this.Max.X;
			corners[6].Y = this.Min.Y;
			corners[6].Z = this.Min.Z;
			corners[7].X = this.Min.X;
			corners[7].Y = this.Min.Y;
			corners[7].Z = this.Min.Z;
		}

		// Token: 0x06003543 RID: 13635 RVA: 0x000604B4 File Offset: 0x0005E6B4
		public float? Intersects(Ray ray)
		{
			return ray.Intersects(this);
		}

		// Token: 0x06003544 RID: 13636 RVA: 0x000604D3 File Offset: 0x0005E6D3
		public void Intersects(ref Ray ray, out float? result)
		{
			result = this.Intersects(ray);
		}

		// Token: 0x06003545 RID: 13637 RVA: 0x000604E8 File Offset: 0x0005E6E8
		public bool Intersects(BoundingFrustum frustum)
		{
			return frustum.Intersects(this);
		}

		// Token: 0x06003546 RID: 13638 RVA: 0x00060506 File Offset: 0x0005E706
		public void Intersects(ref BoundingSphere sphere, out bool result)
		{
			result = this.Intersects(sphere);
		}

		// Token: 0x06003547 RID: 13639 RVA: 0x00060518 File Offset: 0x0005E718
		public bool Intersects(BoundingBox box)
		{
			bool result;
			this.Intersects(ref box, out result);
			return result;
		}

		// Token: 0x06003548 RID: 13640 RVA: 0x00060538 File Offset: 0x0005E738
		public PlaneIntersectionType Intersects(Plane plane)
		{
			PlaneIntersectionType result;
			this.Intersects(ref plane, out result);
			return result;
		}

		// Token: 0x06003549 RID: 13641 RVA: 0x00060558 File Offset: 0x0005E758
		public void Intersects(ref BoundingBox box, out bool result)
		{
			bool flag = this.Max.X >= box.Min.X && this.Min.X <= box.Max.X;
			if (flag)
			{
				bool flag2 = this.Max.Y < box.Min.Y || this.Min.Y > box.Max.Y;
				if (flag2)
				{
					result = false;
				}
				else
				{
					result = (this.Max.Z >= box.Min.Z && this.Min.Z <= box.Max.Z);
				}
			}
			else
			{
				result = false;
			}
		}

		// Token: 0x0600354A RID: 13642 RVA: 0x00060620 File Offset: 0x0005E820
		public bool Intersects(BoundingSphere sphere)
		{
			bool flag = sphere.Center.X - this.Min.X > sphere.Radius && sphere.Center.Y - this.Min.Y > sphere.Radius && sphere.Center.Z - this.Min.Z > sphere.Radius && this.Max.X - sphere.Center.X > sphere.Radius && this.Max.Y - sphere.Center.Y > sphere.Radius && this.Max.Z - sphere.Center.Z > sphere.Radius;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				double num = 0.0;
				bool flag2 = sphere.Center.X - this.Min.X <= sphere.Radius;
				if (flag2)
				{
					num += (double)((sphere.Center.X - this.Min.X) * (sphere.Center.X - this.Min.X));
				}
				else
				{
					bool flag3 = this.Max.X - sphere.Center.X <= sphere.Radius;
					if (flag3)
					{
						num += (double)((sphere.Center.X - this.Max.X) * (sphere.Center.X - this.Max.X));
					}
				}
				bool flag4 = sphere.Center.Y - this.Min.Y <= sphere.Radius;
				if (flag4)
				{
					num += (double)((sphere.Center.Y - this.Min.Y) * (sphere.Center.Y - this.Min.Y));
				}
				else
				{
					bool flag5 = this.Max.Y - sphere.Center.Y <= sphere.Radius;
					if (flag5)
					{
						num += (double)((sphere.Center.Y - this.Max.Y) * (sphere.Center.Y - this.Max.Y));
					}
				}
				bool flag6 = sphere.Center.Z - this.Min.Z <= sphere.Radius;
				if (flag6)
				{
					num += (double)((sphere.Center.Z - this.Min.Z) * (sphere.Center.Z - this.Min.Z));
				}
				else
				{
					bool flag7 = this.Max.Z - sphere.Center.Z <= sphere.Radius;
					if (flag7)
					{
						num += (double)((sphere.Center.Z - this.Max.Z) * (sphere.Center.Z - this.Max.Z));
					}
				}
				bool flag8 = num <= (double)(sphere.Radius * sphere.Radius);
				result = flag8;
			}
			return result;
		}

		// Token: 0x0600354B RID: 13643 RVA: 0x0006095C File Offset: 0x0005EB5C
		public void Intersects(ref Plane plane, out PlaneIntersectionType result)
		{
			bool flag = plane.Normal.X >= 0f;
			Vector3 vector;
			Vector3 vector2;
			if (flag)
			{
				vector.X = this.Max.X;
				vector2.X = this.Min.X;
			}
			else
			{
				vector.X = this.Min.X;
				vector2.X = this.Max.X;
			}
			bool flag2 = plane.Normal.Y >= 0f;
			if (flag2)
			{
				vector.Y = this.Max.Y;
				vector2.Y = this.Min.Y;
			}
			else
			{
				vector.Y = this.Min.Y;
				vector2.Y = this.Max.Y;
			}
			bool flag3 = plane.Normal.Z >= 0f;
			if (flag3)
			{
				vector.Z = this.Max.Z;
				vector2.Z = this.Min.Z;
			}
			else
			{
				vector.Z = this.Min.Z;
				vector2.Z = this.Max.Z;
			}
			float num = plane.Normal.X * vector2.X + plane.Normal.Y * vector2.Y + plane.Normal.Z * vector2.Z + plane.D;
			bool flag4 = num > 0f;
			if (flag4)
			{
				result = PlaneIntersectionType.Front;
			}
			else
			{
				num = plane.Normal.X * vector.X + plane.Normal.Y * vector.Y + plane.Normal.Z * vector.Z + plane.D;
				bool flag5 = num < 0f;
				if (flag5)
				{
					result = PlaneIntersectionType.Back;
				}
				else
				{
					result = PlaneIntersectionType.Intersecting;
				}
			}
		}

		// Token: 0x0600354C RID: 13644 RVA: 0x00060B50 File Offset: 0x0005ED50
		public bool Equals(BoundingBox other)
		{
			return this.Min == other.Min && this.Max == other.Max;
		}

		// Token: 0x0600354D RID: 13645 RVA: 0x00060B8C File Offset: 0x0005ED8C
		public static BoundingBox CreateFromPoints(IEnumerable<Vector3> points)
		{
			bool flag = points == null;
			if (flag)
			{
				throw new ArgumentNullException("points");
			}
			bool flag2 = true;
			Vector3 maxVector = BoundingBox.MaxVector3;
			Vector3 minVector = BoundingBox.MinVector3;
			foreach (Vector3 vector in points)
			{
				maxVector.X = ((maxVector.X < vector.X) ? maxVector.X : vector.X);
				maxVector.Y = ((maxVector.Y < vector.Y) ? maxVector.Y : vector.Y);
				maxVector.Z = ((maxVector.Z < vector.Z) ? maxVector.Z : vector.Z);
				minVector.X = ((minVector.X > vector.X) ? minVector.X : vector.X);
				minVector.Y = ((minVector.Y > vector.Y) ? minVector.Y : vector.Y);
				minVector.Z = ((minVector.Z > vector.Z) ? minVector.Z : vector.Z);
				flag2 = false;
			}
			bool flag3 = flag2;
			if (flag3)
			{
				throw new ArgumentException("Collection is empty", "points");
			}
			return new BoundingBox(maxVector, minVector);
		}

		// Token: 0x0600354E RID: 13646 RVA: 0x00060D08 File Offset: 0x0005EF08
		public static BoundingBox CreateFromSphere(BoundingSphere sphere)
		{
			BoundingBox result;
			BoundingBox.CreateFromSphere(ref sphere, out result);
			return result;
		}

		// Token: 0x0600354F RID: 13647 RVA: 0x00060D28 File Offset: 0x0005EF28
		public static void CreateFromSphere(ref BoundingSphere sphere, out BoundingBox result)
		{
			Vector3 value = new Vector3(sphere.Radius);
			result.Min = sphere.Center - value;
			result.Max = sphere.Center + value;
		}

		// Token: 0x06003550 RID: 13648 RVA: 0x00060D68 File Offset: 0x0005EF68
		public static BoundingBox CreateMerged(BoundingBox original, BoundingBox additional)
		{
			BoundingBox result;
			BoundingBox.CreateMerged(ref original, ref additional, out result);
			return result;
		}

		// Token: 0x06003551 RID: 13649 RVA: 0x00060D88 File Offset: 0x0005EF88
		public static void CreateMerged(ref BoundingBox original, ref BoundingBox additional, out BoundingBox result)
		{
			result.Min.X = Math.Min(original.Min.X, additional.Min.X);
			result.Min.Y = Math.Min(original.Min.Y, additional.Min.Y);
			result.Min.Z = Math.Min(original.Min.Z, additional.Min.Z);
			result.Max.X = Math.Max(original.Max.X, additional.Max.X);
			result.Max.Y = Math.Max(original.Max.Y, additional.Max.Y);
			result.Max.Z = Math.Max(original.Max.Z, additional.Max.Z);
		}

		// Token: 0x06003552 RID: 13650 RVA: 0x00060E7C File Offset: 0x0005F07C
		public override bool Equals(object obj)
		{
			return obj is BoundingBox && this.Equals((BoundingBox)obj);
		}

		// Token: 0x06003553 RID: 13651 RVA: 0x00060EA8 File Offset: 0x0005F0A8
		public override int GetHashCode()
		{
			return this.Min.GetHashCode() + this.Max.GetHashCode();
		}

		// Token: 0x06003554 RID: 13652 RVA: 0x00060EE0 File Offset: 0x0005F0E0
		public static bool operator ==(BoundingBox a, BoundingBox b)
		{
			return a.Equals(b);
		}

		// Token: 0x06003555 RID: 13653 RVA: 0x00060EFC File Offset: 0x0005F0FC
		public static bool operator !=(BoundingBox a, BoundingBox b)
		{
			return !a.Equals(b);
		}

		// Token: 0x06003556 RID: 13654 RVA: 0x00060F1C File Offset: 0x0005F11C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{{Min:",
				this.Min.ToString(),
				" Max:",
				this.Max.ToString(),
				"}}"
			});
		}

		// Token: 0x06003557 RID: 13655 RVA: 0x00060F79 File Offset: 0x0005F179
		public void Grow(Vector3 amount)
		{
			this.Min -= amount;
			this.Max += amount;
		}

		// Token: 0x06003558 RID: 13656 RVA: 0x00060FA0 File Offset: 0x0005F1A0
		public void Translate(Vector3 offset)
		{
			this.Min.X = this.Min.X + offset.X;
			this.Min.Y = this.Min.Y + offset.Y;
			this.Min.Z = this.Min.Z + offset.Z;
			this.Max.X = this.Max.X + offset.X;
			this.Max.Y = this.Max.Y + offset.Y;
			this.Max.Z = this.Max.Z + offset.Z;
		}

		// Token: 0x06003559 RID: 13657 RVA: 0x0006102C File Offset: 0x0005F22C
		public BoundingBox MinkowskiSum(BoundingBox bb)
		{
			return new BoundingBox(this.Min - bb.Max, this.Max - bb.Min);
		}

		// Token: 0x0600355A RID: 13658 RVA: 0x00061068 File Offset: 0x0005F268
		public Vector3 GetSize()
		{
			return new Vector3(this.Max.X - this.Min.X, this.Max.Y - this.Min.Y, this.Max.Z - this.Min.Z);
		}

		// Token: 0x0600355B RID: 13659 RVA: 0x000610C4 File Offset: 0x0005F2C4
		public Vector3 GetCenter()
		{
			return new Vector3(this.Max.X + this.Min.X, this.Max.Y + this.Min.Y, this.Max.Z + this.Min.Z) * 0.5f;
		}

		// Token: 0x0600355C RID: 13660 RVA: 0x0006112C File Offset: 0x0005F32C
		public bool IntersectsExclusive(BoundingBox box)
		{
			bool flag = this.Max.X > box.Min.X && this.Min.X < box.Max.X;
			bool result;
			if (flag)
			{
				bool flag2 = this.Max.Y <= box.Min.Y || this.Min.Y >= box.Max.Y;
				result = (!flag2 && this.Max.Z > box.Min.Z && this.Min.Z < box.Max.Z);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600355D RID: 13661 RVA: 0x000611EC File Offset: 0x0005F3EC
		public bool IntersectsExclusive(BoundingBox box, float offsetX, float offsetY, float offsetZ)
		{
			float num = box.Min.X + offsetX;
			float num2 = box.Max.X + offsetX;
			bool flag = this.Max.X > num && this.Min.X < num2;
			bool result;
			if (flag)
			{
				float num3 = box.Min.Y + offsetY;
				float num4 = box.Max.Y + offsetY;
				bool flag2 = this.Max.Y <= num3 || this.Min.Y >= num4;
				if (flag2)
				{
					result = false;
				}
				else
				{
					float num5 = box.Min.Z + offsetZ;
					float num6 = box.Max.Z + offsetZ;
					result = (this.Max.Z > num5 && this.Min.Z < num6);
				}
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600355E RID: 13662 RVA: 0x000612D8 File Offset: 0x0005F4D8
		public float GetVolume()
		{
			Vector3 size = this.GetSize();
			return size.X * size.Y * size.Z;
		}

		// Token: 0x0600355F RID: 13663 RVA: 0x00061308 File Offset: 0x0005F508
		public bool ForEachBlock<T>(Vector3 v, float epsilon, T t, Func<int, int, int, T, bool> consumer)
		{
			return this.ForEachBlock<T>(v.X, v.Y, v.Z, epsilon, t, consumer);
		}

		// Token: 0x06003560 RID: 13664 RVA: 0x00061338 File Offset: 0x0005F538
		public bool ForEachBlock<T>(float x, float y, float z, float epsilon, T t, Func<int, int, int, T, bool> consumer)
		{
			int num = (int)Math.Floor((double)(x + this.Min.X - epsilon));
			int num2 = (int)Math.Floor((double)(y + this.Min.Y - epsilon));
			int num3 = (int)Math.Floor((double)(z + this.Min.Z - epsilon));
			int num4 = (int)Math.Floor((double)(x + this.Max.X + epsilon));
			int num5 = (int)Math.Floor((double)(y + this.Max.Y + epsilon));
			int num6 = (int)Math.Floor((double)(z + this.Max.Z + epsilon));
			for (int i = num; i <= num4; i++)
			{
				for (int j = num2; j <= num5; j++)
				{
					for (int k = num3; k <= num6; k++)
					{
						bool flag = !consumer(i, j, k, t);
						if (flag)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x040017DC RID: 6108
		public Vector3 Min;

		// Token: 0x040017DD RID: 6109
		public Vector3 Max;

		// Token: 0x040017DE RID: 6110
		public const int CornerCount = 8;

		// Token: 0x040017DF RID: 6111
		private static readonly Vector3 MaxVector3 = new Vector3(float.MaxValue);

		// Token: 0x040017E0 RID: 6112
		private static readonly Vector3 MinVector3 = new Vector3(float.MinValue);
	}
}
