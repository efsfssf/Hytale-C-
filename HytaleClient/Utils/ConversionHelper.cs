using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Utils
{
	// Token: 0x020007BD RID: 1981
	public static class ConversionHelper
	{
		// Token: 0x0600335D RID: 13149 RVA: 0x0004F1C4 File Offset: 0x0004D3C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Point RangeToPoint(Range range)
		{
			return (range.Min > range.Max) ? new Point(range.Max, range.Min) : new Point(range.Min, range.Max);
		}

		// Token: 0x0600335E RID: 13150 RVA: 0x0004F208 File Offset: 0x0004D408
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UShortVector2 RangeToUShortVector2(Range range)
		{
			Debug.Assert(range.Min >= 0 && range.Min <= 65535 && range.Max >= 0 && range.Max <= 65535);
			return (range.Min > range.Max) ? new UShortVector2((ushort)range.Max, (ushort)range.Min) : new UShortVector2((ushort)range.Min, (ushort)range.Max);
		}

		// Token: 0x0600335F RID: 13151 RVA: 0x0004F288 File Offset: 0x0004D488
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ByteVector2 RangeToByteVector2(Range range)
		{
			Debug.Assert(range.Min >= 0 && range.Min <= 255 && range.Max >= 0 && range.Max <= 255);
			return (range.Min > range.Max) ? new ByteVector2((byte)range.Max, (byte)range.Min) : new ByteVector2((byte)range.Min, (byte)range.Max);
		}

		// Token: 0x06003360 RID: 13152 RVA: 0x0004F308 File Offset: 0x0004D508
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 RangeToVector2(Rangef range)
		{
			return (range.Min > range.Max) ? new Vector2(range.Max, range.Min) : new Vector2(range.Min, range.Max);
		}
	}
}
