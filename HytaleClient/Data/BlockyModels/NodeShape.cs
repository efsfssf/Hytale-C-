using System;
using System.Collections.Generic;
using HytaleClient.Math;

namespace HytaleClient.Data.BlockyModels
{
	// Token: 0x02000B6D RID: 2925
	public struct NodeShape
	{
		// Token: 0x04003833 RID: 14387
		public bool Visible;

		// Token: 0x04003834 RID: 14388
		public bool DoubleSided;

		// Token: 0x04003835 RID: 14389
		public string ShadingMode;

		// Token: 0x04003836 RID: 14390
		public string Type;

		// Token: 0x04003837 RID: 14391
		public Vector3 Offset;

		// Token: 0x04003838 RID: 14392
		public Vector3 Stretch;

		// Token: 0x04003839 RID: 14393
		public NodeShapeSettings Settings;

		// Token: 0x0400383A RID: 14394
		public IDictionary<string, FaceLayout> TextureLayout;
	}
}
