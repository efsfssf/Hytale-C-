using System;
using System.Runtime.CompilerServices;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A4E RID: 2638
	public struct UInt32Color
	{
		// Token: 0x060053F0 RID: 21488 RVA: 0x00180ACC File Offset: 0x0017ECCC
		public static UInt32Color FromRGBA(byte r, byte g, byte b, byte a)
		{
			return new UInt32Color
			{
				ABGR = (uint)((int)r | (int)g << 8 | (int)b << 16 | (int)a << 24)
			};
		}

		// Token: 0x060053F1 RID: 21489 RVA: 0x00180B00 File Offset: 0x0017ED00
		public static UInt32Color FromRGBA(uint rgba)
		{
			uint num = rgba >> 24 & 255U;
			uint num2 = rgba >> 16 & 255U;
			uint num3 = rgba >> 8 & 255U;
			uint num4 = rgba & 255U;
			return new UInt32Color
			{
				ABGR = (num | num2 << 8 | num3 << 16 | num4 << 24)
			};
		}

		// Token: 0x060053F2 RID: 21490 RVA: 0x00180B60 File Offset: 0x0017ED60
		public string ToHexString(bool includeAlphaChannel = true)
		{
			string text = "#";
			text += ((byte)(this.ABGR & 255U)).ToString("x2");
			text += ((int)((byte)(this.ABGR >> 8) & byte.MaxValue)).ToString("x2");
			text += ((int)((byte)(this.ABGR >> 16) & byte.MaxValue)).ToString("x2");
			if (includeAlphaChannel)
			{
				text += ((int)((byte)(this.ABGR >> 24) & byte.MaxValue)).ToString("x2");
			}
			return text;
		}

		// Token: 0x060053F3 RID: 21491 RVA: 0x00180C0C File Offset: 0x0017EE0C
		public string ToShortHexString()
		{
			string str = "#";
			str += ((byte)((this.ABGR & 255U) / 255f * 15f)).ToString("x1");
			str += ((byte)((this.ABGR >> 8 & 255U) / 255f * 15f)).ToString("x1");
			return str + ((byte)((this.ABGR >> 16 & 255U) / 255f * 15f)).ToString("x1");
		}

		// Token: 0x060053F4 RID: 21492 RVA: 0x00180CB8 File Offset: 0x0017EEB8
		public static UInt32Color FromShortHexString(string text)
		{
			byte b = Convert.ToByte(text.Substring(1, 1), 16);
			byte b2 = Convert.ToByte(text.Substring(2, 1), 16);
			byte b3 = Convert.ToByte(text.Substring(3, 1), 16);
			byte b4 = (text.Length < 5) ? byte.MaxValue : Convert.ToByte(text.Substring(4, 1), 16);
			return UInt32Color.FromRGBA((byte)((int)b << 4 | (int)b), (byte)((int)b2 << 4 | (int)b2), (byte)((int)b3 << 4 | (int)b3), (byte)((int)b4 << 4 | (int)b4));
		}

		// Token: 0x060053F5 RID: 21493 RVA: 0x00180D3C File Offset: 0x0017EF3C
		public static UInt32Color FromHexString(string text)
		{
			byte r = Convert.ToByte(text.Substring(1, 2), 16);
			byte g = Convert.ToByte(text.Substring(3, 2), 16);
			byte b = Convert.ToByte(text.Substring(5, 2), 16);
			byte a = (text.Length == 7) ? byte.MaxValue : Convert.ToByte(text.Substring(7, 2), 16);
			return UInt32Color.FromRGBA(r, g, b, a);
		}

		// Token: 0x170012F7 RID: 4855
		// (get) Token: 0x060053F6 RID: 21494 RVA: 0x00180DA9 File Offset: 0x0017EFA9
		// (set) Token: 0x060053F7 RID: 21495 RVA: 0x00180DB1 File Offset: 0x0017EFB1
		public uint ABGR { get; private set; }

		// Token: 0x170012F8 RID: 4856
		// (get) Token: 0x060053F8 RID: 21496 RVA: 0x00180DBA File Offset: 0x0017EFBA
		public bool IsTransparent
		{
			get
			{
				return this.ABGR == 0U;
			}
		}

		// Token: 0x060053F9 RID: 21497 RVA: 0x00180DC5 File Offset: 0x0017EFC5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetA(byte a)
		{
			this.ABGR = (uint)((int)a << 24 | (int)(this.ABGR & 16777215U));
		}

		// Token: 0x060053FA RID: 21498 RVA: 0x00180DE0 File Offset: 0x0017EFE0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetB(byte b)
		{
			this.ABGR = (uint)((int)b << 16 | (int)(this.ABGR & 4278255615U));
		}

		// Token: 0x060053FB RID: 21499 RVA: 0x00180DFB File Offset: 0x0017EFFB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetG(byte g)
		{
			this.ABGR = (uint)((int)g << 8 | (int)(this.ABGR & 4294902015U));
		}

		// Token: 0x060053FC RID: 21500 RVA: 0x00180E15 File Offset: 0x0017F015
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetR(byte r)
		{
			this.ABGR = ((uint)r | (this.ABGR & 4294967040U));
		}

		// Token: 0x060053FD RID: 21501 RVA: 0x00180E30 File Offset: 0x0017F030
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public byte GetA()
		{
			return (byte)(this.ABGR >> 24 & 255U);
		}

		// Token: 0x060053FE RID: 21502 RVA: 0x00180E54 File Offset: 0x0017F054
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public byte GetB()
		{
			return (byte)(this.ABGR >> 16 & 255U);
		}

		// Token: 0x060053FF RID: 21503 RVA: 0x00180E78 File Offset: 0x0017F078
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public byte GetG()
		{
			return (byte)(this.ABGR >> 8 & 255U);
		}

		// Token: 0x06005400 RID: 21504 RVA: 0x00180E9C File Offset: 0x0017F09C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public byte GetR()
		{
			return (byte)(this.ABGR & 255U);
		}

		// Token: 0x04002EE7 RID: 12007
		public static readonly UInt32Color White = new UInt32Color
		{
			ABGR = uint.MaxValue
		};

		// Token: 0x04002EE8 RID: 12008
		public static readonly UInt32Color Black = new UInt32Color
		{
			ABGR = 4278190080U
		};

		// Token: 0x04002EE9 RID: 12009
		public static readonly UInt32Color Transparent = new UInt32Color
		{
			ABGR = 0U
		};
	}
}
