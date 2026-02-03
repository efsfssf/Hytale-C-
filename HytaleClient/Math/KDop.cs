using System;
using System.Runtime.CompilerServices;

namespace HytaleClient.Math
{
	// Token: 0x020007E6 RID: 2022
	public struct KDop
	{
		// Token: 0x060035F3 RID: 13811 RVA: 0x0006492E File Offset: 0x00062B2E
		public KDop(int planesCount = 13)
		{
			this.PlaneCount = planesCount;
			this.Planes = new Plane[this.PlaneCount];
		}

		// Token: 0x060035F4 RID: 13812 RVA: 0x0006494C File Offset: 0x00062B4C
		public void BuildFrom(BoundingFrustum frustum, Vector3 direction)
		{
			int num = 0;
			Vector3[] array = new Vector3[8];
			frustum.GetCorners(array);
			for (int i = 0; i < 6; i++)
			{
				ref Plane plane = ref frustum.GetPlane((BoundingFrustum.PlaneId)i);
				bool flag = Vector3.Dot(plane.Normal, direction) > 0f;
				if (flag)
				{
					this.Planes[num] = plane;
					num++;
				}
			}
			for (int j = 0; j < 6; j++)
			{
				ref Plane plane2 = ref frustum.GetPlane((BoundingFrustum.PlaneId)j);
				bool flag2 = Vector3.Dot(plane2.Normal, direction) < 0f;
				if (!flag2)
				{
					for (uint num2 = 0U; num2 < 4U; num2 += 1U)
					{
						BoundingFrustum.PlaneId neighbourId = BoundingFrustum.GetNeighbourId((BoundingFrustum.PlaneId)j, num2);
						ref Plane plane3 = ref frustum.GetPlane(neighbourId);
						bool flag3 = Vector3.Dot(plane3.Normal, direction) < 0f;
						if (flag3)
						{
							BoundingFrustum.CornerId cornerId;
							BoundingFrustum.CornerId cornerId2;
							BoundingFrustum.GetSharedCorners((BoundingFrustum.PlaneId)j, neighbourId, out cornerId, out cornerId2);
							Plane plane4 = new Plane(array[(int)cornerId], array[(int)cornerId2], array[(int)cornerId] + direction);
							this.Planes[num] = plane4;
							num++;
						}
					}
				}
			}
			this.PlaneCount = num;
		}

		// Token: 0x060035F5 RID: 13813 RVA: 0x00064A98 File Offset: 0x00062C98
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Intersects(BoundingSphere volume)
		{
			ContainmentType containmentType;
			this.Contains(volume, out containmentType);
			return containmentType > ContainmentType.Disjoint;
		}

		// Token: 0x060035F6 RID: 13814 RVA: 0x00064AB8 File Offset: 0x00062CB8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Intersects(BoundingBox volume)
		{
			ContainmentType containmentType;
			this.Contains(volume, out containmentType);
			return containmentType > ContainmentType.Disjoint;
		}

		// Token: 0x060035F7 RID: 13815 RVA: 0x00064AD8 File Offset: 0x00062CD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Contains(BoundingSphere volume, out ContainmentType result)
		{
			bool flag = false;
			for (int i = 0; i < this.PlaneCount; i++)
			{
				PlaneIntersectionType planeIntersectionType = PlaneIntersectionType.Front;
				volume.Intersects(ref this.Planes[i], out planeIntersectionType);
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

		// Token: 0x060035F8 RID: 13816 RVA: 0x00064B3C File Offset: 0x00062D3C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Contains(BoundingBox volume, out ContainmentType result)
		{
			bool flag = false;
			for (int i = 0; i < this.PlaneCount; i++)
			{
				PlaneIntersectionType planeIntersectionType = PlaneIntersectionType.Front;
				volume.Intersects(ref this.Planes[i], out planeIntersectionType);
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

		// Token: 0x040017FF RID: 6143
		public int PlaneCount;

		// Token: 0x04001800 RID: 6144
		public Plane[] Planes;
	}
}
