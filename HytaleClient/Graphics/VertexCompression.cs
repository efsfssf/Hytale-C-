using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using HytaleClient.Math;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A4F RID: 2639
	public static class VertexCompression
	{
		// Token: 0x06005402 RID: 21506 RVA: 0x00180F14 File Offset: 0x0017F114
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint NormalizedXYZToUint(float x, float y, float z)
		{
			uint num = (uint)((byte)Math.Round((double)((x * 0.5f + 0.5f) * 255f)) & byte.MaxValue);
			uint num2 = (uint)((byte)Math.Round((double)((y * 0.5f + 0.5f) * 255f)) & byte.MaxValue);
			uint num3 = (uint)((byte)Math.Round((double)((z * 0.5f + 0.5f) * 255f)) & byte.MaxValue);
			return num | num2 << 8 | num3 << 16;
		}

		// Token: 0x06005403 RID: 21507 RVA: 0x00180F94 File Offset: 0x0017F194
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort NormalizedTexCoordToUshort(float u)
		{
			return (ushort)Math.Round((double)(u * 65535f)) & ushort.MaxValue;
		}

		// Token: 0x06005404 RID: 21508 RVA: 0x00180FBC File Offset: 0x0017F1BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short FloatToSnorm16(float v)
		{
			return (short)MathHelper.Clamp((v >= 0f) ? (v * 32767f + 0.5f) : (v * 32767f - 0.5f), -32768f, 32767f);
		}

		// Token: 0x06005405 RID: 21509 RVA: 0x00181004 File Offset: 0x0017F204
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Snorm16ToFloat(short v)
		{
			return Math.Max((float)v / 32767f, -1f);
		}

		// Token: 0x06005406 RID: 21510 RVA: 0x00181028 File Offset: 0x0017F228
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ShortVector2 Vector2ToShortVector2(Vector2 value, float maxRange = 64f)
		{
			ShortVector2 result;
			result.X = VertexCompression.FloatToSnorm16(value.X / maxRange);
			result.Y = VertexCompression.FloatToSnorm16(value.Y / maxRange);
			return result;
		}

		// Token: 0x06005407 RID: 21511 RVA: 0x00181064 File Offset: 0x0017F264
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ShortVector3 Vector3PositionToShortVector3(Vector3 position, float maxRange = 64f)
		{
			ShortVector3 result;
			result.X = VertexCompression.FloatToSnorm16(position.X / maxRange);
			result.Y = VertexCompression.FloatToSnorm16(position.Y / maxRange);
			result.Z = VertexCompression.FloatToSnorm16(position.Z / maxRange);
			return result;
		}

		// Token: 0x06005408 RID: 21512 RVA: 0x001810B4 File Offset: 0x0017F2B4
		public static ushort CompressBlockLocalPosition(int x, int y, int z)
		{
			Debug.Assert(x < 32 && x >= 0 && y < 32 && y >= 0 && z < 32 && z >= 0);
			return (ushort)(x | y << 5 | z << 10);
		}
	}
}
