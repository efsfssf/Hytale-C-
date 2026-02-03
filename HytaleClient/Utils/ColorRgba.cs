using System;
using HytaleClient.Graphics;
using HytaleClient.Math;

namespace HytaleClient.Utils
{
	// Token: 0x020007BB RID: 1979
	public struct ColorRgba
	{
		// Token: 0x06003355 RID: 13141 RVA: 0x0004EF76 File Offset: 0x0004D176
		public ColorRgba(byte r, byte g, byte b, byte a = 255)
		{
			this.R = r;
			this.G = g;
			this.B = b;
			this.A = a;
		}

		// Token: 0x06003356 RID: 13142 RVA: 0x0004EF98 File Offset: 0x0004D198
		public ColorRgba(uint color)
		{
			this.R = (byte)(color & 255U);
			this.G = (byte)(color >> 8 & 255U);
			this.B = (byte)(color >> 16 & 255U);
			this.A = (byte)(color >> 24 & 255U);
		}

		// Token: 0x06003357 RID: 13143 RVA: 0x0004EFE8 File Offset: 0x0004D1E8
		public void Darken(float percent)
		{
			this.R = (byte)MathHelper.Clamp((int)((float)this.R * (100f + percent) / 100f), 0, 255);
			this.G = (byte)MathHelper.Clamp((int)((float)this.G * (100f + percent) / 100f), 0, 255);
			this.B = (byte)MathHelper.Clamp((int)((float)this.B * (100f + percent) / 100f), 0, 255);
		}

		// Token: 0x06003358 RID: 13144 RVA: 0x0004F06E File Offset: 0x0004D26E
		public void Lighten(float percent)
		{
			this.Darken(-percent);
		}

		// Token: 0x06003359 RID: 13145 RVA: 0x0004F07C File Offset: 0x0004D27C
		public static ColorRgba FromUInt32Color(UInt32Color color)
		{
			uint num = color.ABGR >> 24 & 255U;
			uint num2 = color.ABGR >> 16 & 255U;
			uint num3 = color.ABGR >> 8 & 255U;
			uint num4 = color.ABGR & 255U;
			return new ColorRgba((byte)num4, (byte)num3, (byte)num2, (byte)num);
		}

		// Token: 0x0400170E RID: 5902
		public byte R;

		// Token: 0x0400170F RID: 5903
		public byte G;

		// Token: 0x04001710 RID: 5904
		public byte B;

		// Token: 0x04001711 RID: 5905
		public byte A;
	}
}
