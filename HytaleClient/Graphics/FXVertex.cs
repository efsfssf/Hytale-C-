using System;
using System.Runtime.InteropServices;
using HytaleClient.Math;

namespace HytaleClient.Graphics
{
	// Token: 0x020009A6 RID: 2470
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 64)]
	internal struct FXVertex
	{
		// Token: 0x04002A94 RID: 10900
		public static readonly int Size = Marshal.SizeOf(typeof(FXVertex));

		// Token: 0x04002A95 RID: 10901
		public static readonly int ConfigBitShiftQuadType = 0;

		// Token: 0x04002A96 RID: 10902
		public static readonly int ConfigBitShiftLinearFiltering = 3;

		// Token: 0x04002A97 RID: 10903
		public static readonly int ConfigBitShiftSoftParticles = 4;

		// Token: 0x04002A98 RID: 10904
		public static readonly int ConfigBitShiftInvertUTexture = 5;

		// Token: 0x04002A99 RID: 10905
		public static readonly int ConfigBitShiftInvertVTexture = 6;

		// Token: 0x04002A9A RID: 10906
		public static readonly int ConfigBitShiftBlendMode = 7;

		// Token: 0x04002A9B RID: 10907
		public static readonly int ConfigBitShiftIsFirstPerson = 9;

		// Token: 0x04002A9C RID: 10908
		public static readonly int ConfigBitShiftNextFree = 10;

		// Token: 0x04002A9D RID: 10909
		public static readonly int ConfigBitShiftDrawId = 16;

		// Token: 0x04002A9E RID: 10910
		public static readonly uint ConfigBitMaskQuadType = 7U;

		// Token: 0x04002A9F RID: 10911
		public static readonly uint ConfigBitQuadTypeOriented = 0U;

		// Token: 0x04002AA0 RID: 10912
		public static readonly uint ConfigBitQuadTypeBillboard = 1U;

		// Token: 0x04002AA1 RID: 10913
		public static readonly uint ConfigBitQuadTypeBillboardY = 2U;

		// Token: 0x04002AA2 RID: 10914
		public static readonly uint ConfigBitQuadTypeBillboardVelocity = 3U;

		// Token: 0x04002AA3 RID: 10915
		public static readonly uint ConfigBitQuadTypeVelocity = 4U;

		// Token: 0x04002AA4 RID: 10916
		public static readonly uint ConfigBitMaskBlendMode = 1U;

		// Token: 0x04002AA5 RID: 10917
		public static readonly uint ConfigBitBlendModeLinear = 0U;

		// Token: 0x04002AA6 RID: 10918
		public static readonly uint ConfigBitBlendModeAdd = 1U;

		// Token: 0x04002AA7 RID: 10919
		[FieldOffset(0)]
		public uint Config;

		// Token: 0x04002AA8 RID: 10920
		[FieldOffset(4)]
		public uint TextureInfo;

		// Token: 0x04002AA9 RID: 10921
		[FieldOffset(8)]
		public uint Color;

		// Token: 0x04002AAA RID: 10922
		[FieldOffset(12)]
		public Vector3 Position;

		// Token: 0x04002AAB RID: 10923
		[FieldOffset(24)]
		public Vector2 Scale;

		// Token: 0x04002AAC RID: 10924
		[FieldOffset(32)]
		public Vector3 Velocity;

		// Token: 0x04002AAD RID: 10925
		[FieldOffset(44)]
		public Vector4 Rotation;

		// Token: 0x04002AAE RID: 10926
		[FieldOffset(60)]
		public uint SeedAndLifeRatio;

		// Token: 0x04002AAF RID: 10927
		[FieldOffset(4)]
		public Vector3 TopLeftPosition;

		// Token: 0x04002AB0 RID: 10928
		[FieldOffset(16)]
		public Vector3 BottomLeftPosition;

		// Token: 0x04002AB1 RID: 10929
		[FieldOffset(28)]
		public Vector3 TopRightPosition;

		// Token: 0x04002AB2 RID: 10930
		[FieldOffset(40)]
		public Vector3 BottomRightPosition;

		// Token: 0x04002AB3 RID: 10931
		[FieldOffset(52)]
		public float Length;
	}
}
