using System;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Trails
{
	// Token: 0x02000AAF RID: 2735
	internal class TrailSettings
	{
		// Token: 0x04003318 RID: 13080
		public string Id;

		// Token: 0x04003319 RID: 13081
		public string Texture;

		// Token: 0x0400331A RID: 13082
		public Rectangle ImageLocation;

		// Token: 0x0400331B RID: 13083
		public int LifeSpan;

		// Token: 0x0400331C RID: 13084
		public float Roll;

		// Token: 0x0400331D RID: 13085
		public Edge Start;

		// Token: 0x0400331E RID: 13086
		public Edge End;

		// Token: 0x0400331F RID: 13087
		public float LightInfluence;

		// Token: 0x04003320 RID: 13088
		public FXSystem.RenderMode RenderMode;

		// Token: 0x04003321 RID: 13089
		public bool Smooth;

		// Token: 0x04003322 RID: 13090
		public Point FrameSize;

		// Token: 0x04003323 RID: 13091
		public Point FrameRange;

		// Token: 0x04003324 RID: 13092
		public int FrameLifeSpan;

		// Token: 0x04003325 RID: 13093
		public Vector3 IntersectionHighlightColor;

		// Token: 0x04003326 RID: 13094
		public float IntersectionHighlightThreshold;
	}
}
