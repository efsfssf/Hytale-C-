using System;
using System.Runtime.InteropServices;
using HytaleClient.Math;

namespace HytaleClient.Graphics.BlockyModels
{
	// Token: 0x02000AC3 RID: 2755
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 32)]
	internal struct ModelVertex
	{
		// Token: 0x04003461 RID: 13409
		public static readonly int Size = Marshal.SizeOf(typeof(ModelVertex));

		// Token: 0x04003462 RID: 13410
		public uint NodeIndex;

		// Token: 0x04003463 RID: 13411
		public uint AtlasIndexAndShadingModeAndGradienId;

		// Token: 0x04003464 RID: 13412
		public Vector3 Position;

		// Token: 0x04003465 RID: 13413
		public Vector2 TextureCoordinates;
	}
}
