using System;
using System.Runtime.InteropServices;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Batcher2D
{
	// Token: 0x02000AC5 RID: 2757
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 64)]
	internal struct Batcher2DVertex
	{
		// Token: 0x04003475 RID: 13429
		public static readonly int Size = Marshal.SizeOf(typeof(Batcher2DVertex));

		// Token: 0x04003476 RID: 13430
		public Vector3 Position;

		// Token: 0x04003477 RID: 13431
		public UShortVector2 TextureCoordinates;

		// Token: 0x04003478 RID: 13432
		public UShortVector4 Scissor;

		// Token: 0x04003479 RID: 13433
		public Vector4 MaskTextureArea;

		// Token: 0x0400347A RID: 13434
		public UShortVector4 MaskBounds;

		// Token: 0x0400347B RID: 13435
		public UInt32Color FillColor;

		// Token: 0x0400347C RID: 13436
		public UInt32Color OutlineColor;

		// Token: 0x0400347D RID: 13437
		public byte FillThreshold;

		// Token: 0x0400347E RID: 13438
		public byte FillBlurAmount;

		// Token: 0x0400347F RID: 13439
		public byte OutlineThreshold;

		// Token: 0x04003480 RID: 13440
		public byte OutlineBlurAmount;

		// Token: 0x04003481 RID: 13441
		public uint FontId;
	}
}
