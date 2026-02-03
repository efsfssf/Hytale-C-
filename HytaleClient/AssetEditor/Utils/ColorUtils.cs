using System;
using HytaleClient.Graphics;
using HytaleClient.Math;

namespace HytaleClient.AssetEditor.Utils
{
	// Token: 0x02000B90 RID: 2960
	public static class ColorUtils
	{
		// Token: 0x06005B5D RID: 23389 RVA: 0x001C83F8 File Offset: 0x001C65F8
		public static bool TryParseColor(string text, out UInt32Color color, out ColorUtils.ColorFormatType formatType)
		{
			color = UInt32Color.White;
			formatType = ColorUtils.ColorFormatType.Hex;
			bool flag = text.StartsWith("#") && text.Length == 7;
			bool result;
			if (flag)
			{
				try
				{
					color = UInt32Color.FromHexString(text);
				}
				catch (Exception)
				{
					return false;
				}
				formatType = ColorUtils.ColorFormatType.Hex;
				result = true;
			}
			else
			{
				bool flag2 = text.StartsWith("rgb(");
				if (flag2)
				{
					text = text.Substring("rgb(".Length).TrimEnd(new char[]
					{
						')'
					});
					string[] array = text.Split(new char[]
					{
						','
					});
					bool flag3 = array.Length != 3;
					if (flag3)
					{
						result = false;
					}
					else
					{
						uint num;
						uint num2;
						uint num3;
						bool flag4 = !uint.TryParse(array[0].Trim(), out num) || !uint.TryParse(array[1].Trim(), out num2) || !uint.TryParse(array[2].Trim(), out num3);
						if (flag4)
						{
							result = false;
						}
						else
						{
							color = UInt32Color.FromRGBA((byte)num, (byte)num2, (byte)num3, byte.MaxValue);
							formatType = ColorUtils.ColorFormatType.Rgb;
							result = true;
						}
					}
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06005B5E RID: 23390 RVA: 0x001C8528 File Offset: 0x001C6728
		public static bool TryParseColorAlpha(string text, out UInt32Color color, out ColorUtils.ColorFormatType formatType)
		{
			color = UInt32Color.White;
			formatType = ColorUtils.ColorFormatType.Hex;
			bool flag = text.StartsWith("#") && (text.Length == 9 || text.Length == 7);
			bool result;
			if (flag)
			{
				try
				{
					formatType = ColorUtils.ColorFormatType.HexAlpha;
					color = UInt32Color.FromHexString(text);
				}
				catch (Exception)
				{
					return false;
				}
				result = true;
			}
			else
			{
				bool flag2 = text.StartsWith("rgba(");
				if (flag2)
				{
					text = text.Substring("rgba(".Length).TrimEnd(new char[]
					{
						')'
					});
					string[] array = text.Split(new char[]
					{
						','
					});
					bool flag3 = text.StartsWith("#");
					if (flag3)
					{
						bool flag4 = array.Length != 2;
						if (flag4)
						{
							result = false;
						}
						else
						{
							string text2 = array[0].Trim();
							int num = (int)(float.Parse(array[1].Trim()) * 255f);
							num = MathHelper.Clamp(num, 0, 255);
							try
							{
								color = UInt32Color.FromHexString(text2);
							}
							catch (Exception)
							{
								return false;
							}
							color.SetA((byte)num);
							formatType = ColorUtils.ColorFormatType.RgbaHex;
							result = true;
						}
					}
					else
					{
						bool flag5 = array.Length != 4;
						if (flag5)
						{
							result = false;
						}
						else
						{
							uint num2;
							uint num3;
							uint num4;
							uint num5;
							bool flag6 = !uint.TryParse(array[0].Trim(), out num2) || !uint.TryParse(array[1].Trim(), out num3) || !uint.TryParse(array[2].Trim(), out num4) || !uint.TryParse(array[3].Trim(), out num5);
							if (flag6)
							{
								result = false;
							}
							else
							{
								color = UInt32Color.FromRGBA((byte)num2, (byte)num3, (byte)num4, (byte)num5);
								formatType = ColorUtils.ColorFormatType.Rgba;
								result = true;
							}
						}
					}
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06005B5F RID: 23391 RVA: 0x001C8704 File Offset: 0x001C6904
		public static string FormatColor(UInt32Color color, ColorUtils.ColorFormatType formatType)
		{
			string result;
			switch (formatType)
			{
			case ColorUtils.ColorFormatType.Hex:
				result = color.ToHexString(false);
				break;
			case ColorUtils.ColorFormatType.Rgb:
				result = ColorUtils.FormatRgbColor(color);
				break;
			case ColorUtils.ColorFormatType.HexAlpha:
				result = color.ToHexString(true);
				break;
			case ColorUtils.ColorFormatType.Rgba:
				result = ColorUtils.FormatRgbaColor(color);
				break;
			case ColorUtils.ColorFormatType.RgbaHex:
				result = ColorUtils.FormatRgbaHexColor(color);
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		// Token: 0x06005B60 RID: 23392 RVA: 0x001C8770 File Offset: 0x001C6970
		public static string FormatRgbaHexColor(UInt32Color color)
		{
			string str = "rgba(";
			str += color.ToHexString(false);
			str += ", ";
			str += ((float)color.GetA() / 255f).ToString("0.###");
			return str + ")";
		}

		// Token: 0x06005B61 RID: 23393 RVA: 0x001C87D4 File Offset: 0x001C69D4
		public static string FormatRgbColor(UInt32Color color)
		{
			string str = "rgb(";
			str += color.GetR().ToString();
			str += ", ";
			str += color.GetG().ToString();
			str += ", ";
			str += color.GetB().ToString();
			return str + ")";
		}

		// Token: 0x06005B62 RID: 23394 RVA: 0x001C8854 File Offset: 0x001C6A54
		public static string FormatRgbaColor(UInt32Color color)
		{
			string str = "rgb(";
			str += color.GetR().ToString();
			str += ", ";
			str += color.GetG().ToString();
			str += ", ";
			str += color.GetB().ToString();
			str += ", ";
			str += color.GetA().ToString();
			return str + ")";
		}

		// Token: 0x04003935 RID: 14645
		public const string DefaultRgbaColor = "rgba(#ffffff, 1)";

		// Token: 0x04003936 RID: 14646
		public const string DefaultRgbColor = "#ffffff";

		// Token: 0x04003937 RID: 14647
		public const string DefaultRgbShortColor = "#fffff";

		// Token: 0x02000F82 RID: 3970
		public enum ColorFormatType
		{
			// Token: 0x04004B40 RID: 19264
			Hex,
			// Token: 0x04004B41 RID: 19265
			Rgb,
			// Token: 0x04004B42 RID: 19266
			HexAlpha,
			// Token: 0x04004B43 RID: 19267
			Rgba,
			// Token: 0x04004B44 RID: 19268
			RgbaHex
		}
	}
}
