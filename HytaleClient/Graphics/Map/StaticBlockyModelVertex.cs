using System;
using System.Runtime.InteropServices;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Map
{
	// Token: 0x02000A94 RID: 2708
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct StaticBlockyModelVertex
	{
		// Token: 0x04003215 RID: 12821
		public byte NodeIndex;

		// Token: 0x04003216 RID: 12822
		public Vector3 Position;

		// Token: 0x04003217 RID: 12823
		public Vector3 Normal;

		// Token: 0x04003218 RID: 12824
		public ShadingMode ShadingMode;

		// Token: 0x04003219 RID: 12825
		public uint DoubleSided;

		// Token: 0x0400321A RID: 12826
		public Vector2 TextureCoordinates;

		// Token: 0x0400321B RID: 12827
		public uint TintColorAndEffect;

		// Token: 0x0400321C RID: 12828
		public uint GlowColorAndSunlight;
	}
}
