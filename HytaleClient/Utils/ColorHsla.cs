using System;
using HytaleClient.Math;

namespace HytaleClient.Utils
{
	// Token: 0x020007B8 RID: 1976
	public struct ColorHsla
	{
		// Token: 0x0600333D RID: 13117 RVA: 0x0004E6A0 File Offset: 0x0004C8A0
		public ColorHsla(float h, float s, float l, float a = 1f)
		{
			this.H = h;
			this.S = s;
			this.L = l;
			this.A = a;
		}

		// Token: 0x0600333E RID: 13118 RVA: 0x0004E6C0 File Offset: 0x0004C8C0
		public static ColorHsla FromRgba(byte r, byte g, byte b, byte a = 255)
		{
			return ColorHsla.FromRgba((float)r / 255f, (float)g / 255f, (float)b / 255f, (float)a / 255f);
		}

		// Token: 0x0600333F RID: 13119 RVA: 0x0004E6F8 File Offset: 0x0004C8F8
		public static ColorHsla FromRgba(ColorRgba rgba)
		{
			return ColorHsla.FromRgba(rgba.R, rgba.G, rgba.B, rgba.A);
		}

		// Token: 0x06003340 RID: 13120 RVA: 0x0004E728 File Offset: 0x0004C928
		public static ColorHsla FromRgba(float r, float g, float b, float a)
		{
			float num = MathHelper.Min(MathHelper.Min(r, g), b);
			float num2 = MathHelper.Max(MathHelper.Max(r, g), b);
			float num3 = num2 - num;
			float num4 = (num2 + num) / 2f;
			float s = (num2 + num) / 2f;
			float num5 = (num2 + num) / 2f;
			bool flag = num2 == num;
			if (flag)
			{
				s = 0f;
				num4 = 0f;
			}
			else
			{
				s = ((num5 > 0.5f) ? (num3 / (2f - num2 - num)) : (num3 / (num2 + num)));
				bool flag2 = num2 == r;
				if (flag2)
				{
					num4 = (g - b) / num3 + (float)((g < b) ? 6 : 0);
				}
				else
				{
					bool flag3 = num2 == g;
					if (flag3)
					{
						num4 = (b - r) / num3 + 2f;
					}
					else
					{
						bool flag4 = num2 == b;
						if (flag4)
						{
							num4 = (r - g) / num3 + 4f;
						}
					}
				}
				num4 /= 6f;
			}
			return new ColorHsla(num4, s, num5, a);
		}

		// Token: 0x06003341 RID: 13121 RVA: 0x0004E814 File Offset: 0x0004CA14
		private float HueToRgbComponent(float p, float q, float t)
		{
			bool flag = t < 0f;
			if (flag)
			{
				t += 1f;
			}
			bool flag2 = t > 1f;
			if (flag2)
			{
				t -= 1f;
			}
			bool flag3 = t < 0.16666667f;
			float result;
			if (flag3)
			{
				result = p + (q - p) * 6f * t;
			}
			else
			{
				bool flag4 = t < 0.5f;
				if (flag4)
				{
					result = q;
				}
				else
				{
					bool flag5 = t < 0.6666667f;
					if (flag5)
					{
						result = p + (q - p) * (0.6666667f - t) * 6f;
					}
					else
					{
						result = p;
					}
				}
			}
			return result;
		}

		// Token: 0x06003342 RID: 13122 RVA: 0x0004E8A3 File Offset: 0x0004CAA3
		public void ToRgba(out byte r, out byte g, out byte b, out byte a)
		{
			this.ToRgb(out r, out g, out b);
			a = (byte)Math.Round((double)(this.A * 255f));
		}

		// Token: 0x06003343 RID: 13123 RVA: 0x0004E8C8 File Offset: 0x0004CAC8
		public void ToRgb(out byte r, out byte g, out byte b)
		{
			float num;
			float num2;
			float num3;
			this.ToRgb(out num, out num2, out num3);
			r = (byte)((int)Math.Round((double)(num * 255f)));
			g = (byte)((int)Math.Round((double)(num2 * 255f)));
			b = (byte)((int)Math.Round((double)(num3 * 255f)));
		}

		// Token: 0x06003344 RID: 13124 RVA: 0x0004E916 File Offset: 0x0004CB16
		public void ToRgba(out float r, out float g, out float b, out float a)
		{
			this.ToRgb(out r, out g, out b);
			a = this.A;
		}

		// Token: 0x06003345 RID: 13125 RVA: 0x0004E92C File Offset: 0x0004CB2C
		public void ToRgb(out float r, out float g, out float b)
		{
			bool flag = this.S == 0f;
			if (flag)
			{
				r = this.L;
				g = this.L;
				b = this.L;
			}
			else
			{
				float num = (this.L < 0.5f) ? (this.L * (1f + this.S)) : (this.L + this.S - this.L * this.S);
				float p = 2f * this.L - num;
				r = this.HueToRgbComponent(p, num, this.H + 0.33333334f);
				g = this.HueToRgbComponent(p, num, this.H);
				b = this.HueToRgbComponent(p, num, this.H - 0.33333334f);
			}
		}

		// Token: 0x06003346 RID: 13126 RVA: 0x0004E9F5 File Offset: 0x0004CBF5
		public void Saturate(float amount)
		{
			this.S = MathHelper.Clamp(this.S + amount, 0f, 1f);
		}

		// Token: 0x06003347 RID: 13127 RVA: 0x0004EA15 File Offset: 0x0004CC15
		public void Desaturate(float amount)
		{
			this.S = MathHelper.Clamp(this.S - amount, 0f, 1f);
		}

		// Token: 0x06003348 RID: 13128 RVA: 0x0004EA35 File Offset: 0x0004CC35
		public void Lighten(float amount)
		{
			this.L = MathHelper.Clamp(this.L + amount, 0f, 1f);
		}

		// Token: 0x06003349 RID: 13129 RVA: 0x0004EA55 File Offset: 0x0004CC55
		public void Darken(float amount)
		{
			this.L = MathHelper.Clamp(this.L - amount, 0f, 1f);
		}

		// Token: 0x04001702 RID: 5890
		public float H;

		// Token: 0x04001703 RID: 5891
		public float S;

		// Token: 0x04001704 RID: 5892
		public float L;

		// Token: 0x04001705 RID: 5893
		public float A;
	}
}
