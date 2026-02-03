using System;
using HytaleClient.Graphics;
using HytaleClient.Math;

namespace HytaleClient.Utils
{
	// Token: 0x020007B9 RID: 1977
	public struct ColorHsva
	{
		// Token: 0x0600334A RID: 13130 RVA: 0x0004EA75 File Offset: 0x0004CC75
		public ColorHsva(float h, float s, float v, float a)
		{
			this.H = h;
			this.S = s;
			this.V = v;
			this.A = a;
		}

		// Token: 0x0600334B RID: 13131 RVA: 0x0004EA98 File Offset: 0x0004CC98
		public static ColorHsva FromRgba(byte r, byte g, byte b, byte a)
		{
			return ColorHsva.FromRgba((float)r / 255f, (float)g / 255f, (float)b / 255f, (float)a / 255f);
		}

		// Token: 0x0600334C RID: 13132 RVA: 0x0004EAD0 File Offset: 0x0004CCD0
		public static ColorHsva FromRgba(ColorRgba rgba)
		{
			return ColorHsva.FromRgba(rgba.R, rgba.G, rgba.B, rgba.A);
		}

		// Token: 0x0600334D RID: 13133 RVA: 0x0004EB00 File Offset: 0x0004CD00
		public static ColorHsva FromUInt32Color(UInt32Color color)
		{
			uint num = color.ABGR >> 24 & 255U;
			uint num2 = color.ABGR >> 16 & 255U;
			uint num3 = color.ABGR >> 8 & 255U;
			uint num4 = color.ABGR & 255U;
			return ColorHsva.FromRgba((byte)num4, (byte)num3, (byte)num2, (byte)num);
		}

		// Token: 0x0600334E RID: 13134 RVA: 0x0004EB64 File Offset: 0x0004CD64
		public static ColorHsva FromRgba(float r, float g, float b, float a)
		{
			float num = MathHelper.Min(MathHelper.Min(r, g), b);
			float num2 = MathHelper.Max(MathHelper.Max(r, g), b);
			float num3 = num2 - num;
			float v = num2;
			bool flag = Math.Abs(num3) < 0.001f;
			float s;
			float num4;
			if (flag)
			{
				s = 0f;
				num4 = -1f;
			}
			else
			{
				s = num3 / num2;
				bool flag2 = r == num2;
				if (flag2)
				{
					num4 = (g - b) / num3;
				}
				else
				{
					bool flag3 = g == num2;
					if (flag3)
					{
						num4 = 2f + (b - r) / num3;
					}
					else
					{
						num4 = 4f + (r - g) / num3;
					}
				}
				num4 /= 6f;
				bool flag4 = num4 < 0f;
				if (flag4)
				{
					num4 += 1f;
				}
			}
			return new ColorHsva(num4, s, v, a);
		}

		// Token: 0x0600334F RID: 13135 RVA: 0x0004EC28 File Offset: 0x0004CE28
		public void ToRgba(out byte r, out byte g, out byte b, out byte a)
		{
			float num;
			float num2;
			float num3;
			float num4;
			this.ToRgba(out num, out num2, out num3, out num4);
			r = (byte)Math.Round((double)(num * 255f));
			g = (byte)Math.Round((double)(num2 * 255f));
			b = (byte)Math.Round((double)(num3 * 255f));
			a = (byte)Math.Round((double)(num4 * 255f));
		}

		// Token: 0x06003350 RID: 13136 RVA: 0x0004EC88 File Offset: 0x0004CE88
		public UInt32Color ToUInt32Color()
		{
			byte r;
			byte g;
			byte b;
			byte a;
			this.ToRgba(out r, out g, out b, out a);
			return UInt32Color.FromRGBA(r, g, b, a);
		}

		// Token: 0x06003351 RID: 13137 RVA: 0x0004ECB4 File Offset: 0x0004CEB4
		public void ToRgba(out float r, out float g, out float b, out float a)
		{
			a = this.A;
			bool flag = this.S == 0f;
			if (flag)
			{
				r = (g = (b = this.V));
			}
			else
			{
				float num = this.H % 1f * 6f;
				int num2 = (int)Math.Floor((double)num);
				float num3 = num - (float)num2;
				float num4 = this.V * (1f - this.S);
				float num5 = this.V * (1f - this.S * num3);
				float num6 = this.V * (1f - this.S * (1f - num3));
				switch (num2)
				{
				case 0:
					r = this.V;
					g = num6;
					b = num4;
					break;
				case 1:
					r = num5;
					g = this.V;
					b = num4;
					break;
				case 2:
					r = num4;
					g = this.V;
					b = num6;
					break;
				case 3:
					r = num4;
					g = num5;
					b = this.V;
					break;
				case 4:
					r = num6;
					g = num4;
					b = this.V;
					break;
				default:
					r = this.V;
					g = num4;
					b = num5;
					break;
				}
			}
		}

		// Token: 0x06003352 RID: 13138 RVA: 0x0004EDF0 File Offset: 0x0004CFF0
		public static ColorHsva Lerp(ColorHsva color1, ColorHsva color2, float t)
		{
			float s = MathHelper.Lerp(color1.S, color2.S, t);
			float v = MathHelper.Lerp(color1.V, color2.V, t);
			float a = MathHelper.Lerp(color1.A, color2.A, t);
			bool flag = color1.H == -1f || color1.S == 0f;
			if (flag)
			{
				color1.H = color2.H;
			}
			else
			{
				bool flag2 = color2.H == -1f || color2.S == 0f;
				if (flag2)
				{
					color2.H = color1.H;
				}
			}
			float num = color2.H - color1.H;
			bool flag3 = color1.H > color2.H;
			if (flag3)
			{
				float h = color2.H;
				color2.H = color1.H;
				color1.H = h;
				num = -num;
				t = 1f - t;
			}
			bool flag4 = (double)num > 0.5;
			float h2;
			if (flag4)
			{
				color1.H += 1f;
				h2 = (color1.H + t * (color2.H - color1.H)) % 1f;
			}
			else
			{
				h2 = color1.H + t * num;
			}
			return new ColorHsva(h2, s, v, a);
		}

		// Token: 0x04001706 RID: 5894
		public float H;

		// Token: 0x04001707 RID: 5895
		public float S;

		// Token: 0x04001708 RID: 5896
		public float V;

		// Token: 0x04001709 RID: 5897
		public float A;
	}
}
