using System;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Trails
{
	// Token: 0x02000AA9 RID: 2729
	internal struct SegmentBuffers : IFXDataStorage
	{
		// Token: 0x060055CA RID: 21962 RVA: 0x0019869A File Offset: 0x0019689A
		public void Initialize(int maxSegments)
		{
			this.Count = maxSegments;
			this.TrailPosition = new Vector3[maxSegments];
			this.Rotation = new Quaternion[maxSegments];
			this.Length = new float[maxSegments];
			this.Life = new int[maxSegments];
		}

		// Token: 0x060055CB RID: 21963 RVA: 0x001986D4 File Offset: 0x001968D4
		public void Release()
		{
		}

		// Token: 0x040032C5 RID: 12997
		public int Count;

		// Token: 0x040032C6 RID: 12998
		public Vector3[] TrailPosition;

		// Token: 0x040032C7 RID: 12999
		public Quaternion[] Rotation;

		// Token: 0x040032C8 RID: 13000
		public float[] Length;

		// Token: 0x040032C9 RID: 13001
		public int[] Life;
	}
}
