using System;

namespace HytaleClient.Graphics.Map
{
	// Token: 0x02000A90 RID: 2704
	internal class ChunkGeometryData
	{
		// Token: 0x040031DF RID: 12767
		public int VerticesCount;

		// Token: 0x040031E0 RID: 12768
		public int IndicesCount;

		// Token: 0x040031E1 RID: 12769
		public ChunkVertex[] Vertices;

		// Token: 0x040031E2 RID: 12770
		public uint[] Indices;

		// Token: 0x040031E3 RID: 12771
		public uint VerticesOffset;

		// Token: 0x040031E4 RID: 12772
		public int IndicesOffset;
	}
}
