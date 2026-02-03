using System;
using System.Runtime.InteropServices;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Sky
{
	// Token: 0x02000A50 RID: 2640
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 32)]
	internal struct SkyAndCloudsVertex
	{
		// Token: 0x04002EEB RID: 12011
		public static readonly int Size = Marshal.SizeOf(typeof(SkyAndCloudsVertex));

		// Token: 0x04002EEC RID: 12012
		public Vector3 Position;

		// Token: 0x04002EED RID: 12013
		public Vector2 TextureCoordinates;
	}
}
