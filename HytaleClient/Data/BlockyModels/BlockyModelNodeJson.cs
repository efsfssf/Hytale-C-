using System;
using HytaleClient.Math;

namespace HytaleClient.Data.BlockyModels
{
	// Token: 0x02000B6C RID: 2924
	public struct BlockyModelNodeJson
	{
		// Token: 0x0400382E RID: 14382
		public string Name;

		// Token: 0x0400382F RID: 14383
		public Vector3 Position;

		// Token: 0x04003830 RID: 14384
		public Quaternion Orientation;

		// Token: 0x04003831 RID: 14385
		public NodeShape Shape;

		// Token: 0x04003832 RID: 14386
		public BlockyModelNodeJson[] Children;
	}
}
