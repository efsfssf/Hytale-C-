using System;
using HytaleClient.Math;

namespace HytaleClient.Data.ClientInteraction.Selector
{
	// Token: 0x02000B19 RID: 2841
	internal class HitDetectionBuffer
	{
		// Token: 0x04003732 RID: 14130
		public Vector4 HitPosition;

		// Token: 0x04003733 RID: 14131
		public Quad4 TransformedQuad;

		// Token: 0x04003734 RID: 14132
		public Vector4 TransformedPoint;

		// Token: 0x04003735 RID: 14133
		public Triangle4 VisibleTriangle;

		// Token: 0x04003736 RID: 14134
		public bool ContainsFully;
	}
}
