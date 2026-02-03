using System;
using System.Runtime.InteropServices;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Map
{
	// Token: 0x02000A91 RID: 2705
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 32)]
	internal struct ChunkVertex
	{
		// Token: 0x040031E5 RID: 12773
		public static readonly int Size = Marshal.SizeOf(typeof(ChunkVertex));

		// Token: 0x040031E6 RID: 12774
		public ShortVector3 PositionPacked;

		// Token: 0x040031E7 RID: 12775
		public ushort DoubleSidedAndBlockId;

		// Token: 0x040031E8 RID: 12776
		public UShortVector2 TextureCoordinates;

		// Token: 0x040031E9 RID: 12777
		public UShortVector2 MaskTextureCoordinates;

		// Token: 0x040031EA RID: 12778
		public uint NormalAndNodeIndex;

		// Token: 0x040031EB RID: 12779
		public uint TintColorAndEffectAndShadingMode;

		// Token: 0x040031EC RID: 12780
		public uint GlowColorAndSunlight;

		// Token: 0x040031ED RID: 12781
		public uint UseBillboard;
	}
}
