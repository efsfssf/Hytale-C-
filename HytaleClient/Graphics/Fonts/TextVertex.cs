using System;
using System.Runtime.InteropServices;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Fonts
{
	// Token: 0x02000AC1 RID: 2753
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 32)]
	internal struct TextVertex
	{
		// Token: 0x04003453 RID: 13395
		public static readonly int Size = Marshal.SizeOf(typeof(TextVertex));

		// Token: 0x04003454 RID: 13396
		public Vector3 Position;

		// Token: 0x04003455 RID: 13397
		public Vector2 TextureCoordinates;

		// Token: 0x04003456 RID: 13398
		public uint FillColor;

		// Token: 0x04003457 RID: 13399
		public uint OutlineColor;
	}
}
