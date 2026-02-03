using System;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A77 RID: 2679
	internal class MapChunkNearProgram : MapChunkBaseProgram
	{
		// Token: 0x060054BC RID: 21692 RVA: 0x00186286 File Offset: 0x00184486
		public MapChunkNearProgram(bool alphaTest, bool useDeferred, bool useLOD, string variationName = null) : base(alphaTest, false, true, useDeferred, useLOD, variationName)
		{
		}

		// Token: 0x040030AE RID: 12462
		public Uniform FoliageInteractionPositions;

		// Token: 0x040030AF RID: 12463
		public Uniform FoliageInteractionParams;
	}
}
