using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace HytaleClient.Math
{
	// Token: 0x020007E0 RID: 2016
	[DebuggerDisplay("{DebugDisplayString,nq}")]
	public class BoundingFrustum : IEquatable<BoundingFrustum>
	{
		// Token: 0x17000FAD RID: 4013
		// (get) Token: 0x06003562 RID: 13666 RVA: 0x00061470 File Offset: 0x0005F670
		// (set) Token: 0x06003563 RID: 13667 RVA: 0x00061488 File Offset: 0x0005F688
		public Matrix Matrix
		{
			get
			{
				return this.matrix;
			}
			set
			{
				this.matrix = value;
				this.CreatePlanes();
				this.CreateCorners();
			}
		}

		// Token: 0x17000FAE RID: 4014
		// (get) Token: 0x06003564 RID: 13668 RVA: 0x000614A0 File Offset: 0x0005F6A0
		public Plane Near
		{
			get
			{
				return this.planes[0];
			}
		}

		// Token: 0x17000FAF RID: 4015
		// (get) Token: 0x06003565 RID: 13669 RVA: 0x000614C0 File Offset: 0x0005F6C0
		public Plane Far
		{
			get
			{
				return this.planes[1];
			}
		}

		// Token: 0x17000FB0 RID: 4016
		// (get) Token: 0x06003566 RID: 13670 RVA: 0x000614E0 File Offset: 0x0005F6E0
		public Plane Left
		{
			get
			{
				return this.planes[2];
			}
		}

		// Token: 0x17000FB1 RID: 4017
		// (get) Token: 0x06003567 RID: 13671 RVA: 0x00061500 File Offset: 0x0005F700
		public Plane Right
		{
			get
			{
				return this.planes[3];
			}
		}

		// Token: 0x17000FB2 RID: 4018
		// (get) Token: 0x06003568 RID: 13672 RVA: 0x00061520 File Offset: 0x0005F720
		public Plane Top
		{
			get
			{
				return this.planes[4];
			}
		}

		// Token: 0x17000FB3 RID: 4019
		// (get) Token: 0x06003569 RID: 13673 RVA: 0x00061540 File Offset: 0x0005F740
		public Plane Bottom
		{
			get
			{
				return this.planes[5];
			}
		}

		// Token: 0x17000FB4 RID: 4020
		// (get) Token: 0x0600356A RID: 13674 RVA: 0x00061560 File Offset: 0x0005F760
		internal string DebugDisplayString
		{
			get
			{
				return string.Concat(new string[]
				{
					"Near( ",
					this.planes[0].DebugDisplayString,
					" ) \r\n",
					"Far( ",
					this.planes[1].DebugDisplayString,
					" ) \r\n",
					"Left( ",
					this.planes[2].DebugDisplayString,
					" ) \r\n",
					"Right( ",
					this.planes[3].DebugDisplayString,
					" ) \r\n",
					"Top( ",
					this.planes[4].DebugDisplayString,
					" ) \r\n",
					"Bottom( ",
					this.planes[5].DebugDisplayString,
					" ) "
				});
			}
		}

		// Token: 0x0600356B RID: 13675 RVA: 0x0006165F File Offset: 0x0005F85F
		public BoundingFrustum(Matrix value)
		{
			this.matrix = value;
			this.CreatePlanes();
			this.CreateCorners();
		}

		// Token: 0x0600356C RID: 13676 RVA: 0x00061698 File Offset: 0x0005F898
		public ContainmentType Contains(BoundingFrustum frustum)
		{
			bool flag = this == frustum;
			ContainmentType result;
			if (flag)
			{
				result = ContainmentType.Contains;
			}
			else
			{
				bool flag2 = false;
				for (int i = 0; i < 6; i++)
				{
					PlaneIntersectionType planeIntersectionType;
					frustum.Intersects(ref this.planes[i], out planeIntersectionType);
					bool flag3 = planeIntersectionType == PlaneIntersectionType.Front;
					if (flag3)
					{
						return ContainmentType.Disjoint;
					}
					bool flag4 = planeIntersectionType == PlaneIntersectionType.Intersecting;
					if (flag4)
					{
						flag2 = true;
					}
				}
				result = (flag2 ? ContainmentType.Intersects : ContainmentType.Contains);
			}
			return result;
		}

		// Token: 0x0600356D RID: 13677 RVA: 0x0006170C File Offset: 0x0005F90C
		public ContainmentType Contains(BoundingBox box)
		{
			ContainmentType result = ContainmentType.Disjoint;
			this.Contains(ref box, out result);
			return result;
		}

		// Token: 0x0600356E RID: 13678 RVA: 0x0006172C File Offset: 0x0005F92C
		public void Contains(ref BoundingBox box, out ContainmentType result)
		{
			bool flag = false;
			for (int i = 0; i < 6; i++)
			{
				PlaneIntersectionType planeIntersectionType = PlaneIntersectionType.Front;
				box.Intersects(ref this.planes[i], out planeIntersectionType);
				PlaneIntersectionType planeIntersectionType2 = planeIntersectionType;
				PlaneIntersectionType planeIntersectionType3 = planeIntersectionType2;
				if (planeIntersectionType3 == PlaneIntersectionType.Front)
				{
					result = ContainmentType.Disjoint;
					return;
				}
				if (planeIntersectionType3 == PlaneIntersectionType.Intersecting)
				{
					flag = true;
				}
			}
			result = (flag ? ContainmentType.Intersects : ContainmentType.Contains);
		}

		// Token: 0x0600356F RID: 13679 RVA: 0x0006178C File Offset: 0x0005F98C
		public ContainmentType Contains(BoundingSphere sphere)
		{
			ContainmentType result = ContainmentType.Disjoint;
			this.Contains(ref sphere, out result);
			return result;
		}

		// Token: 0x06003570 RID: 13680 RVA: 0x000617AC File Offset: 0x0005F9AC
		public void Contains(ref BoundingSphere sphere, out ContainmentType result)
		{
			bool flag = false;
			for (int i = 0; i < 6; i++)
			{
				PlaneIntersectionType planeIntersectionType = PlaneIntersectionType.Front;
				sphere.Intersects(ref this.planes[i], out planeIntersectionType);
				PlaneIntersectionType planeIntersectionType2 = planeIntersectionType;
				PlaneIntersectionType planeIntersectionType3 = planeIntersectionType2;
				if (planeIntersectionType3 == PlaneIntersectionType.Front)
				{
					result = ContainmentType.Disjoint;
					return;
				}
				if (planeIntersectionType3 == PlaneIntersectionType.Intersecting)
				{
					flag = true;
				}
			}
			result = (flag ? ContainmentType.Intersects : ContainmentType.Contains);
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x0006180C File Offset: 0x0005FA0C
		public ContainmentType Contains(Vector3 point)
		{
			ContainmentType result = ContainmentType.Disjoint;
			this.Contains(ref point, out result);
			return result;
		}

		// Token: 0x06003572 RID: 13682 RVA: 0x0006182C File Offset: 0x0005FA2C
		public void Contains(ref Vector3 point, out ContainmentType result)
		{
			bool flag = false;
			for (int i = 0; i < 6; i++)
			{
				float num = point.X * this.planes[i].Normal.X + point.Y * this.planes[i].Normal.Y + point.Z * this.planes[i].Normal.Z + this.planes[i].D;
				bool flag2 = num > 0f;
				if (flag2)
				{
					result = ContainmentType.Disjoint;
					return;
				}
				bool flag3 = num == 0f;
				if (flag3)
				{
					flag = true;
					break;
				}
			}
			result = (flag ? ContainmentType.Intersects : ContainmentType.Contains);
		}

		// Token: 0x06003573 RID: 13683 RVA: 0x000618F0 File Offset: 0x0005FAF0
		public Vector3[] GetCorners()
		{
			return (Vector3[])this.corners.Clone();
		}

		// Token: 0x06003574 RID: 13684 RVA: 0x00061914 File Offset: 0x0005FB14
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
				throw new ArgumentOutOfRangeException("corners");
			}
			this.corners.CopyTo(corners, 0);
		}

		// Token: 0x06003575 RID: 13685 RVA: 0x0006195C File Offset: 0x0005FB5C
		public void GetFarCorners(Vector3[] farCorners)
		{
			bool flag = farCorners == null;
			if (flag)
			{
				throw new ArgumentNullException("farCorners");
			}
			bool flag2 = farCorners.Length < 4;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException("farCorners");
			}
			farCorners[0] = this.corners[4];
			farCorners[1] = this.corners[5];
			farCorners[2] = this.corners[6];
			farCorners[3] = this.corners[7];
		}

		// Token: 0x06003576 RID: 13686 RVA: 0x000619E0 File Offset: 0x0005FBE0
		public bool Intersects(BoundingFrustum frustum)
		{
			return this.Contains(frustum) > ContainmentType.Disjoint;
		}

		// Token: 0x06003577 RID: 13687 RVA: 0x000619FC File Offset: 0x0005FBFC
		public bool Intersects(BoundingBox box)
		{
			bool result = false;
			this.Intersects(ref box, out result);
			return result;
		}

		// Token: 0x06003578 RID: 13688 RVA: 0x00061A1C File Offset: 0x0005FC1C
		public void Intersects(ref BoundingBox box, out bool result)
		{
			ContainmentType containmentType = ContainmentType.Disjoint;
			this.Contains(ref box, out containmentType);
			result = (containmentType > ContainmentType.Disjoint);
		}

		// Token: 0x06003579 RID: 13689 RVA: 0x00061A3C File Offset: 0x0005FC3C
		public bool Intersects(BoundingSphere sphere)
		{
			bool result = false;
			this.Intersects(ref sphere, out result);
			return result;
		}

		// Token: 0x0600357A RID: 13690 RVA: 0x00061A5C File Offset: 0x0005FC5C
		public void Intersects(ref BoundingSphere sphere, out bool result)
		{
			ContainmentType containmentType = ContainmentType.Disjoint;
			this.Contains(ref sphere, out containmentType);
			result = (containmentType > ContainmentType.Disjoint);
		}

		// Token: 0x0600357B RID: 13691 RVA: 0x00061A7C File Offset: 0x0005FC7C
		public PlaneIntersectionType Intersects(Plane plane)
		{
			PlaneIntersectionType result;
			this.Intersects(ref plane, out result);
			return result;
		}

		// Token: 0x0600357C RID: 13692 RVA: 0x00061A9C File Offset: 0x0005FC9C
		public void Intersects(ref Plane plane, out PlaneIntersectionType result)
		{
			result = plane.Intersects(ref this.corners[0]);
			for (int i = 1; i < this.corners.Length; i++)
			{
				bool flag = plane.Intersects(ref this.corners[i]) != result;
				if (flag)
				{
					result = PlaneIntersectionType.Intersecting;
				}
			}
		}

		// Token: 0x0600357D RID: 13693 RVA: 0x00061AFC File Offset: 0x0005FCFC
		public float? Intersects(Ray ray)
		{
			float? result;
			this.Intersects(ref ray, out result);
			return result;
		}

		// Token: 0x0600357E RID: 13694 RVA: 0x00061B1C File Offset: 0x0005FD1C
		public void Intersects(ref Ray ray, out float? result)
		{
			ContainmentType containmentType;
			this.Contains(ref ray.Position, out containmentType);
			bool flag = containmentType == ContainmentType.Disjoint;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = containmentType == ContainmentType.Contains;
				if (flag2)
				{
					result = new float?(0f);
				}
				else
				{
					bool flag3 = containmentType != ContainmentType.Intersects;
					if (flag3)
					{
						throw new ArgumentOutOfRangeException("ctype");
					}
					throw new NotImplementedException();
				}
			}
		}

		// Token: 0x0600357F RID: 13695 RVA: 0x00061B84 File Offset: 0x0005FD84
		private void CreateCorners()
		{
			BoundingFrustum.IntersectionPoint(ref this.planes[0], ref this.planes[2], ref this.planes[4], out this.corners[0]);
			BoundingFrustum.IntersectionPoint(ref this.planes[0], ref this.planes[3], ref this.planes[4], out this.corners[1]);
			BoundingFrustum.IntersectionPoint(ref this.planes[0], ref this.planes[3], ref this.planes[5], out this.corners[2]);
			BoundingFrustum.IntersectionPoint(ref this.planes[0], ref this.planes[2], ref this.planes[5], out this.corners[3]);
			BoundingFrustum.IntersectionPoint(ref this.planes[1], ref this.planes[2], ref this.planes[4], out this.corners[4]);
			BoundingFrustum.IntersectionPoint(ref this.planes[1], ref this.planes[3], ref this.planes[4], out this.corners[5]);
			BoundingFrustum.IntersectionPoint(ref this.planes[1], ref this.planes[3], ref this.planes[5], out this.corners[6]);
			BoundingFrustum.IntersectionPoint(ref this.planes[1], ref this.planes[2], ref this.planes[5], out this.corners[7]);
		}

		// Token: 0x06003580 RID: 13696 RVA: 0x00061D44 File Offset: 0x0005FF44
		private void CreatePlanes()
		{
			this.planes[0] = new Plane(-this.matrix.M13, -this.matrix.M23, -this.matrix.M33, -this.matrix.M43);
			this.planes[1] = new Plane(this.matrix.M13 - this.matrix.M14, this.matrix.M23 - this.matrix.M24, this.matrix.M33 - this.matrix.M34, this.matrix.M43 - this.matrix.M44);
			this.planes[2] = new Plane(-this.matrix.M14 - this.matrix.M11, -this.matrix.M24 - this.matrix.M21, -this.matrix.M34 - this.matrix.M31, -this.matrix.M44 - this.matrix.M41);
			this.planes[3] = new Plane(this.matrix.M11 - this.matrix.M14, this.matrix.M21 - this.matrix.M24, this.matrix.M31 - this.matrix.M34, this.matrix.M41 - this.matrix.M44);
			this.planes[4] = new Plane(this.matrix.M12 - this.matrix.M14, this.matrix.M22 - this.matrix.M24, this.matrix.M32 - this.matrix.M34, this.matrix.M42 - this.matrix.M44);
			this.planes[5] = new Plane(-this.matrix.M14 - this.matrix.M12, -this.matrix.M24 - this.matrix.M22, -this.matrix.M34 - this.matrix.M32, -this.matrix.M44 - this.matrix.M42);
			this.NormalizePlane(ref this.planes[0]);
			this.NormalizePlane(ref this.planes[1]);
			this.NormalizePlane(ref this.planes[2]);
			this.NormalizePlane(ref this.planes[3]);
			this.NormalizePlane(ref this.planes[4]);
			this.NormalizePlane(ref this.planes[5]);
		}

		// Token: 0x06003581 RID: 13697 RVA: 0x00062030 File Offset: 0x00060230
		private void NormalizePlane(ref Plane p)
		{
			float num = 1f / p.Normal.Length();
			p.Normal.X = p.Normal.X * num;
			p.Normal.Y = p.Normal.Y * num;
			p.Normal.Z = p.Normal.Z * num;
			p.D *= num;
		}

		// Token: 0x06003582 RID: 13698 RVA: 0x0006208C File Offset: 0x0006028C
		private static void IntersectionPoint(ref Plane a, ref Plane b, ref Plane c, out Vector3 result)
		{
			Vector3 vector;
			Vector3.Cross(ref b.Normal, ref c.Normal, out vector);
			float num;
			Vector3.Dot(ref a.Normal, ref vector, out num);
			num *= -1f;
			Vector3.Cross(ref b.Normal, ref c.Normal, out vector);
			Vector3 vector2;
			Vector3.Multiply(ref vector, a.D, out vector2);
			Vector3.Cross(ref c.Normal, ref a.Normal, out vector);
			Vector3 vector3;
			Vector3.Multiply(ref vector, b.D, out vector3);
			Vector3.Cross(ref a.Normal, ref b.Normal, out vector);
			Vector3 vector4;
			Vector3.Multiply(ref vector, c.D, out vector4);
			result.X = (vector2.X + vector3.X + vector4.X) / num;
			result.Y = (vector2.Y + vector3.Y + vector4.Y) / num;
			result.Z = (vector2.Z + vector3.Z + vector4.Z) / num;
		}

		// Token: 0x06003583 RID: 13699 RVA: 0x0006218C File Offset: 0x0006038C
		public static bool operator ==(BoundingFrustum a, BoundingFrustum b)
		{
			bool flag = object.Equals(a, null);
			bool result;
			if (flag)
			{
				result = object.Equals(b, null);
			}
			else
			{
				bool flag2 = object.Equals(b, null);
				if (flag2)
				{
					result = object.Equals(a, null);
				}
				else
				{
					result = (a.matrix == b.matrix);
				}
			}
			return result;
		}

		// Token: 0x06003584 RID: 13700 RVA: 0x000621DC File Offset: 0x000603DC
		public static bool operator !=(BoundingFrustum a, BoundingFrustum b)
		{
			return !(a == b);
		}

		// Token: 0x06003585 RID: 13701 RVA: 0x000621F8 File Offset: 0x000603F8
		public bool Equals(BoundingFrustum other)
		{
			return this == other;
		}

		// Token: 0x06003586 RID: 13702 RVA: 0x00062214 File Offset: 0x00060414
		public override bool Equals(object obj)
		{
			return obj is BoundingFrustum && this.Equals((BoundingFrustum)obj);
		}

		// Token: 0x06003587 RID: 13703 RVA: 0x00062240 File Offset: 0x00060440
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			stringBuilder.Append("{Near:");
			stringBuilder.Append(this.planes[0].ToString());
			stringBuilder.Append(" Far:");
			stringBuilder.Append(this.planes[1].ToString());
			stringBuilder.Append(" Left:");
			stringBuilder.Append(this.planes[2].ToString());
			stringBuilder.Append(" Right:");
			stringBuilder.Append(this.planes[3].ToString());
			stringBuilder.Append(" Top:");
			stringBuilder.Append(this.planes[4].ToString());
			stringBuilder.Append(" Bottom:");
			stringBuilder.Append(this.planes[5].ToString());
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		// Token: 0x06003588 RID: 13704 RVA: 0x0006236C File Offset: 0x0006056C
		public override int GetHashCode()
		{
			return this.matrix.GetHashCode();
		}

		// Token: 0x06003589 RID: 13705 RVA: 0x00062390 File Offset: 0x00060590
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ref Plane GetPlane(BoundingFrustum.PlaneId id)
		{
			return ref this.planes[(int)id];
		}

		// Token: 0x0600358A RID: 13706 RVA: 0x000623B0 File Offset: 0x000605B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BoundingFrustum.PlaneId GetNeighbourId(BoundingFrustum.PlaneId id, uint neighbourIndex)
		{
			Debug.Assert(neighbourIndex < 4U, "There are only 4 neighbour !");
			int num = (int)(id + (int)((BoundingFrustum.PlaneId.Far + (int)((id + 1) % BoundingFrustum.PlaneId.Left)) % (BoundingFrustum.PlaneId)6));
			return (BoundingFrustum.PlaneId)(((long)num + (long)((ulong)neighbourIndex)) % 6L);
		}

		// Token: 0x0600358B RID: 13707 RVA: 0x000623E6 File Offset: 0x000605E6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GetSharedCorners(BoundingFrustum.PlaneId planeA, BoundingFrustum.PlaneId planeB, out BoundingFrustum.CornerId cornerA, out BoundingFrustum.CornerId cornerB)
		{
			cornerA = BoundingFrustum._sharedCorners[(int)(planeA * (BoundingFrustum.PlaneId)6 * BoundingFrustum.PlaneId.Left + (int)(planeB * BoundingFrustum.PlaneId.Left))];
			cornerB = BoundingFrustum._sharedCorners[(int)(planeA * (BoundingFrustum.PlaneId)6 * BoundingFrustum.PlaneId.Left + (int)(planeB * BoundingFrustum.PlaneId.Left) + 1)];
		}

		// Token: 0x0600358C RID: 13708 RVA: 0x00062410 File Offset: 0x00060610
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BuildPlanesFromCorners(Vector3[] corners)
		{
			corners.CopyTo(this.corners, 0);
			this.planes[0] = new Plane(corners[0], corners[1], corners[3]);
			this.planes[1] = new Plane(corners[7], corners[6], corners[4]);
			this.planes[2] = new Plane(corners[3], corners[7], corners[0]);
			this.planes[3] = new Plane(corners[6], corners[2], corners[5]);
			this.planes[4] = new Plane(corners[5], corners[1], corners[4]);
			this.planes[5] = new Plane(corners[3], corners[2], corners[7]);
		}

		// Token: 0x0600358D RID: 13709 RVA: 0x00062510 File Offset: 0x00060710
		static BoundingFrustum()
		{
			BoundingFrustum._sharedCorners[28] = BoundingFrustum.CornerId.FarLeftBottom;
			BoundingFrustum._sharedCorners[29] = BoundingFrustum.CornerId.FarLeftBottom;
			BoundingFrustum._sharedCorners[30] = BoundingFrustum.CornerId.FarLeftBottom;
			BoundingFrustum._sharedCorners[31] = BoundingFrustum.CornerId.FarLeftBottom;
			BoundingFrustum._sharedCorners[32] = BoundingFrustum.CornerId.NearLeftTop;
			BoundingFrustum._sharedCorners[33] = BoundingFrustum.CornerId.FarLeftTop;
			BoundingFrustum._sharedCorners[34] = BoundingFrustum.CornerId.FarLeftBottom;
			BoundingFrustum._sharedCorners[35] = BoundingFrustum.CornerId.NearLeftBottom;
			BoundingFrustum._sharedCorners[24] = BoundingFrustum.CornerId.NearLeftBottom;
			BoundingFrustum._sharedCorners[25] = BoundingFrustum.CornerId.NearLeftTop;
			BoundingFrustum._sharedCorners[26] = BoundingFrustum.CornerId.FarLeftTop;
			BoundingFrustum._sharedCorners[27] = BoundingFrustum.CornerId.FarLeftBottom;
			BoundingFrustum._sharedCorners[40] = BoundingFrustum.CornerId.FarRightBottom;
			BoundingFrustum._sharedCorners[41] = BoundingFrustum.CornerId.FarRightBottom;
			BoundingFrustum._sharedCorners[42] = BoundingFrustum.CornerId.FarRightBottom;
			BoundingFrustum._sharedCorners[43] = BoundingFrustum.CornerId.FarRightBottom;
			BoundingFrustum._sharedCorners[44] = BoundingFrustum.CornerId.FarRightTop;
			BoundingFrustum._sharedCorners[45] = BoundingFrustum.CornerId.NearRightTop;
			BoundingFrustum._sharedCorners[46] = BoundingFrustum.CornerId.NearRightBottom;
			BoundingFrustum._sharedCorners[47] = BoundingFrustum.CornerId.FarRightBottom;
			BoundingFrustum._sharedCorners[36] = BoundingFrustum.CornerId.NearRightTop;
			BoundingFrustum._sharedCorners[37] = BoundingFrustum.CornerId.NearRightBottom;
			BoundingFrustum._sharedCorners[38] = BoundingFrustum.CornerId.FarRightBottom;
			BoundingFrustum._sharedCorners[39] = BoundingFrustum.CornerId.FarRightTop;
			BoundingFrustum._sharedCorners[52] = BoundingFrustum.CornerId.FarLeftTop;
			BoundingFrustum._sharedCorners[53] = BoundingFrustum.CornerId.NearLeftTop;
			BoundingFrustum._sharedCorners[54] = BoundingFrustum.CornerId.NearRightTop;
			BoundingFrustum._sharedCorners[55] = BoundingFrustum.CornerId.FarRightTop;
			BoundingFrustum._sharedCorners[56] = BoundingFrustum.CornerId.NearLeftTop;
			BoundingFrustum._sharedCorners[57] = BoundingFrustum.CornerId.NearLeftTop;
			BoundingFrustum._sharedCorners[58] = BoundingFrustum.CornerId.NearLeftTop;
			BoundingFrustum._sharedCorners[59] = BoundingFrustum.CornerId.NearLeftTop;
			BoundingFrustum._sharedCorners[48] = BoundingFrustum.CornerId.NearLeftTop;
			BoundingFrustum._sharedCorners[49] = BoundingFrustum.CornerId.NearRightTop;
			BoundingFrustum._sharedCorners[50] = BoundingFrustum.CornerId.FarRightTop;
			BoundingFrustum._sharedCorners[51] = BoundingFrustum.CornerId.FarLeftTop;
			BoundingFrustum._sharedCorners[64] = BoundingFrustum.CornerId.NearLeftBottom;
			BoundingFrustum._sharedCorners[65] = BoundingFrustum.CornerId.FarLeftBottom;
			BoundingFrustum._sharedCorners[66] = BoundingFrustum.CornerId.FarRightBottom;
			BoundingFrustum._sharedCorners[67] = BoundingFrustum.CornerId.NearRightBottom;
			BoundingFrustum._sharedCorners[68] = BoundingFrustum.CornerId.NearLeftBottom;
			BoundingFrustum._sharedCorners[69] = BoundingFrustum.CornerId.NearLeftBottom;
			BoundingFrustum._sharedCorners[70] = BoundingFrustum.CornerId.NearLeftBottom;
			BoundingFrustum._sharedCorners[71] = BoundingFrustum.CornerId.NearLeftBottom;
			BoundingFrustum._sharedCorners[60] = BoundingFrustum.CornerId.NearRightBottom;
			BoundingFrustum._sharedCorners[61] = BoundingFrustum.CornerId.NearLeftBottom;
			BoundingFrustum._sharedCorners[62] = BoundingFrustum.CornerId.FarLeftBottom;
			BoundingFrustum._sharedCorners[63] = BoundingFrustum.CornerId.FarRightBottom;
			BoundingFrustum._sharedCorners[4] = BoundingFrustum.CornerId.NearLeftTop;
			BoundingFrustum._sharedCorners[5] = BoundingFrustum.CornerId.NearLeftBottom;
			BoundingFrustum._sharedCorners[6] = BoundingFrustum.CornerId.NearRightBottom;
			BoundingFrustum._sharedCorners[7] = BoundingFrustum.CornerId.NearRightTop;
			BoundingFrustum._sharedCorners[8] = BoundingFrustum.CornerId.NearRightTop;
			BoundingFrustum._sharedCorners[9] = BoundingFrustum.CornerId.NearLeftTop;
			BoundingFrustum._sharedCorners[10] = BoundingFrustum.CornerId.NearLeftBottom;
			BoundingFrustum._sharedCorners[11] = BoundingFrustum.CornerId.NearRightBottom;
			BoundingFrustum._sharedCorners[0] = BoundingFrustum.CornerId.NearLeftBottom;
			BoundingFrustum._sharedCorners[1] = BoundingFrustum.CornerId.NearRightBottom;
			BoundingFrustum._sharedCorners[2] = BoundingFrustum.CornerId.NearLeftBottom;
			BoundingFrustum._sharedCorners[3] = BoundingFrustum.CornerId.NearRightBottom;
			BoundingFrustum._sharedCorners[16] = BoundingFrustum.CornerId.FarLeftBottom;
			BoundingFrustum._sharedCorners[17] = BoundingFrustum.CornerId.FarLeftTop;
			BoundingFrustum._sharedCorners[18] = BoundingFrustum.CornerId.FarRightTop;
			BoundingFrustum._sharedCorners[19] = BoundingFrustum.CornerId.FarRightBottom;
			BoundingFrustum._sharedCorners[20] = BoundingFrustum.CornerId.FarLeftTop;
			BoundingFrustum._sharedCorners[21] = BoundingFrustum.CornerId.FarRightTop;
			BoundingFrustum._sharedCorners[22] = BoundingFrustum.CornerId.FarRightBottom;
			BoundingFrustum._sharedCorners[23] = BoundingFrustum.CornerId.FarLeftBottom;
			BoundingFrustum._sharedCorners[14] = BoundingFrustum.CornerId.FarLeftBottom;
			BoundingFrustum._sharedCorners[15] = BoundingFrustum.CornerId.FarRightBottom;
			BoundingFrustum._sharedCorners[14] = BoundingFrustum.CornerId.FarLeftBottom;
			BoundingFrustum._sharedCorners[15] = BoundingFrustum.CornerId.FarRightBottom;
		}

		// Token: 0x040017E1 RID: 6113
		public const int CornerCount = 8;

		// Token: 0x040017E2 RID: 6114
		private Matrix matrix;

		// Token: 0x040017E3 RID: 6115
		private readonly Vector3[] corners = new Vector3[8];

		// Token: 0x040017E4 RID: 6116
		private readonly Plane[] planes = new Plane[6];

		// Token: 0x040017E5 RID: 6117
		private const int PlaneCount = 6;

		// Token: 0x040017E6 RID: 6118
		private static BoundingFrustum.CornerId[] _sharedCorners = new BoundingFrustum.CornerId[72];

		// Token: 0x02000CC1 RID: 3265
		public enum PlaneId
		{
			// Token: 0x04003FB6 RID: 16310
			Near,
			// Token: 0x04003FB7 RID: 16311
			Far,
			// Token: 0x04003FB8 RID: 16312
			Left,
			// Token: 0x04003FB9 RID: 16313
			Right,
			// Token: 0x04003FBA RID: 16314
			Top,
			// Token: 0x04003FBB RID: 16315
			Bottom
		}

		// Token: 0x02000CC2 RID: 3266
		public enum CornerId
		{
			// Token: 0x04003FBD RID: 16317
			NearLeftTop,
			// Token: 0x04003FBE RID: 16318
			NearRightTop,
			// Token: 0x04003FBF RID: 16319
			NearRightBottom,
			// Token: 0x04003FC0 RID: 16320
			NearLeftBottom,
			// Token: 0x04003FC1 RID: 16321
			FarLeftTop,
			// Token: 0x04003FC2 RID: 16322
			FarRightTop,
			// Token: 0x04003FC3 RID: 16323
			FarRightBottom,
			// Token: 0x04003FC4 RID: 16324
			FarLeftBottom
		}
	}
}
