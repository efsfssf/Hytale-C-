using System;

namespace HytaleClient.Math
{
	// Token: 0x020007E3 RID: 2019
	public static class Easing
	{
		// Token: 0x060035AE RID: 13742 RVA: 0x000634C0 File Offset: 0x000616C0
		public static float Ease(Easing.EasingType easingType, float t, float b = 0f, float c = 1f, float d = 1f)
		{
			float result;
			switch (easingType)
			{
			case Easing.EasingType.Linear:
				result = Easing.Linear(t, b, c, d);
				break;
			case Easing.EasingType.QuadIn:
				result = Easing.QuadIn(t, b, c, d);
				break;
			case Easing.EasingType.QuadOut:
				result = Easing.QuadOut(t, b, c, d);
				break;
			case Easing.EasingType.QuadInOut:
				result = Easing.QuadInOut(t, b, c, d);
				break;
			case Easing.EasingType.CubicIn:
				result = Easing.CubicIn(t, b, c, d);
				break;
			case Easing.EasingType.CubicOut:
				result = Easing.CubicOut(t, b, c, d);
				break;
			case Easing.EasingType.CubicInOut:
				result = Easing.CubicInOut(t, b, c, d);
				break;
			case Easing.EasingType.QuartIn:
				result = Easing.QuartIn(t, b, c, d);
				break;
			case Easing.EasingType.QuartOut:
				result = Easing.QuartOut(t, b, c, d);
				break;
			case Easing.EasingType.QuartInOut:
				result = Easing.QuartInOut(t, b, c, d);
				break;
			case Easing.EasingType.QuintIn:
				result = Easing.QuintIn(t, b, c, d);
				break;
			case Easing.EasingType.QuintOut:
				result = Easing.QuintOut(t, b, c, d);
				break;
			case Easing.EasingType.QuintInOut:
				result = Easing.QuintInOut(t, b, c, d);
				break;
			case Easing.EasingType.SineIn:
				result = Easing.SineIn(t, b, c, d);
				break;
			case Easing.EasingType.SineOut:
				result = Easing.SineOut(t, b, c, d);
				break;
			case Easing.EasingType.SineInOut:
				result = Easing.SineInOut(t, b, c, d);
				break;
			case Easing.EasingType.ExpoIn:
				result = Easing.ExpoIn(t, b, c, d);
				break;
			case Easing.EasingType.ExpoOut:
				result = Easing.ExpoOut(t, b, c, d);
				break;
			case Easing.EasingType.ExpoInOut:
				result = Easing.ExpoInOut(t, b, c, d);
				break;
			case Easing.EasingType.CircIn:
				result = Easing.CircIn(t, b, c, d);
				break;
			case Easing.EasingType.CircOut:
				result = Easing.CircOut(t, b, c, d);
				break;
			case Easing.EasingType.CircInOut:
				result = Easing.CircInOut(t, b, c, d);
				break;
			case Easing.EasingType.ElasticIn:
				result = Easing.ElasticIn(t, b, c, d);
				break;
			case Easing.EasingType.ElasticOut:
				result = Easing.ElasticOut(t, b, c, d);
				break;
			case Easing.EasingType.ElasticInOut:
				result = Easing.ElasticInOut(t, b, c, d);
				break;
			case Easing.EasingType.BackIn:
				result = Easing.BackIn(t, b, c, d);
				break;
			case Easing.EasingType.BackOut:
				result = Easing.BackOut(t, b, c, d);
				break;
			case Easing.EasingType.BackInOut:
				result = Easing.BackInOut(t, b, c, d);
				break;
			case Easing.EasingType.BounceIn:
				result = Easing.BounceIn(t, b, c, d);
				break;
			case Easing.EasingType.BounceOut:
				result = Easing.BounceOut(t, b, c, d);
				break;
			case Easing.EasingType.BounceInOut:
				result = Easing.BounceInOut(t, b, c, d);
				break;
			default:
				throw new Exception(string.Format("Invalid easing type - {0}", easingType));
			}
			return result;
		}

		// Token: 0x060035AF RID: 13743 RVA: 0x00063748 File Offset: 0x00061948
		public static float Linear(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			return b + (c - b) * t / d;
		}

		// Token: 0x060035B0 RID: 13744 RVA: 0x00063764 File Offset: 0x00061964
		public static float QuadIn(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			return c * (t /= d) * t + b;
		}

		// Token: 0x060035B1 RID: 13745 RVA: 0x00063784 File Offset: 0x00061984
		public static float QuadOut(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			return -c * (t /= d) * (t - 2f) + b;
		}

		// Token: 0x060035B2 RID: 13746 RVA: 0x000637AC File Offset: 0x000619AC
		public static float QuadInOut(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			bool flag = (t /= d / 2f) < 1f;
			float result;
			if (flag)
			{
				result = c / 2f * t * t + b;
			}
			else
			{
				result = -c / 2f * ((t -= 1f) * (t - 2f) - 1f) + b;
			}
			return result;
		}

		// Token: 0x060035B3 RID: 13747 RVA: 0x00063808 File Offset: 0x00061A08
		public static float CubicIn(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			return c * (t /= d) * t * t + b;
		}

		// Token: 0x060035B4 RID: 13748 RVA: 0x00063828 File Offset: 0x00061A28
		public static float CubicOut(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			return c * ((t = t / d - 1f) * t * t + 1f) + b;
		}

		// Token: 0x060035B5 RID: 13749 RVA: 0x00063854 File Offset: 0x00061A54
		public static float CubicInOut(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			bool flag = (t /= d / 2f) < 1f;
			float result;
			if (flag)
			{
				result = c / 2f * t * t * t + b;
			}
			else
			{
				result = c / 2f * ((t -= 2f) * t * t + 2f) + b;
			}
			return result;
		}

		// Token: 0x060035B6 RID: 13750 RVA: 0x000638B0 File Offset: 0x00061AB0
		public static float QuartIn(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			return c * (t /= d) * t * t * t + b;
		}

		// Token: 0x060035B7 RID: 13751 RVA: 0x000638D4 File Offset: 0x00061AD4
		public static float QuartOut(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			return -c * ((t = t / d - 1f) * t * t * t - 1f) + b;
		}

		// Token: 0x060035B8 RID: 13752 RVA: 0x00063904 File Offset: 0x00061B04
		public static float QuartInOut(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			bool flag = (t /= d / 2f) < 1f;
			float result;
			if (flag)
			{
				result = c / 2f * t * t * t * t + b;
			}
			else
			{
				result = -c / 2f * ((t -= 2f) * t * t * t - 2f) + b;
			}
			return result;
		}

		// Token: 0x060035B9 RID: 13753 RVA: 0x00063964 File Offset: 0x00061B64
		public static float QuintIn(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			return c * (t /= d) * t * t * t * t + b;
		}

		// Token: 0x060035BA RID: 13754 RVA: 0x00063988 File Offset: 0x00061B88
		public static float QuintOut(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			return c * ((t = t / d - 1f) * t * t * t * t + 1f) + b;
		}

		// Token: 0x060035BB RID: 13755 RVA: 0x000639B8 File Offset: 0x00061BB8
		public static float QuintInOut(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			bool flag = (t /= d / 2f) < 1f;
			float result;
			if (flag)
			{
				result = c / 2f * t * t * t * t * t + b;
			}
			else
			{
				result = c / 2f * ((t -= 2f) * t * t * t * t + 2f) + b;
			}
			return result;
		}

		// Token: 0x060035BC RID: 13756 RVA: 0x00063A1C File Offset: 0x00061C1C
		public static float SineIn(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			return -c * (float)Math.Cos((double)(t / d * 1.5707964f)) + c + b;
		}

		// Token: 0x060035BD RID: 13757 RVA: 0x00063A48 File Offset: 0x00061C48
		public static float SineOut(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			return c * (float)Math.Sin((double)(t / d * 1.5707964f)) + b;
		}

		// Token: 0x060035BE RID: 13758 RVA: 0x00063A70 File Offset: 0x00061C70
		public static float SineInOut(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			return -c / 2f * (float)(Math.Cos((double)(3.1415927f * t / d)) - 1.0) + b;
		}

		// Token: 0x060035BF RID: 13759 RVA: 0x00063AA8 File Offset: 0x00061CA8
		public static float ExpoIn(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			return (t == 0f) ? b : (c * (float)Math.Pow(2.0, (double)(10f * (t / d - 1f))) + b);
		}

		// Token: 0x060035C0 RID: 13760 RVA: 0x00063AE8 File Offset: 0x00061CE8
		public static float ExpoOut(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			return (t == d) ? (b + c) : (c * (-(float)Math.Pow(2.0, (double)(-10f * t / d)) + 1f) + b);
		}

		// Token: 0x060035C1 RID: 13761 RVA: 0x00063B28 File Offset: 0x00061D28
		public static float ExpoInOut(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			bool flag = t == 0f;
			float result;
			if (flag)
			{
				result = b;
			}
			else
			{
				bool flag2 = t == d;
				if (flag2)
				{
					result = b + c;
				}
				else
				{
					bool flag3 = (t /= d / 2f) < 1f;
					if (flag3)
					{
						result = c / 2f * (float)Math.Pow(2.0, (double)(10f * (t - 1f))) + b;
					}
					else
					{
						result = c / 2f * (-(float)Math.Pow(2.0, (double)(-10f * (t -= 1f))) + 2f) + b;
					}
				}
			}
			return result;
		}

		// Token: 0x060035C2 RID: 13762 RVA: 0x00063BD0 File Offset: 0x00061DD0
		public static float CircIn(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			return -c * ((float)Math.Sqrt((double)(1f - (t /= d) * t)) - 1f) + b;
		}

		// Token: 0x060035C3 RID: 13763 RVA: 0x00063C04 File Offset: 0x00061E04
		public static float CircOut(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			return c * (float)Math.Sqrt((double)(1f - (t = t / d - 1f) * t)) + b;
		}

		// Token: 0x060035C4 RID: 13764 RVA: 0x00063C38 File Offset: 0x00061E38
		public static float CircInOut(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			bool flag = (t /= d / 2f) < 1f;
			float result;
			if (flag)
			{
				result = -c / 2f * ((float)Math.Sqrt((double)(1f - t * t)) - 1f) + b;
			}
			else
			{
				result = c / 2f * ((float)Math.Sqrt((double)(1f - (t -= 2f) * t)) + 1f) + b;
			}
			return result;
		}

		// Token: 0x060035C5 RID: 13765 RVA: 0x00063CB0 File Offset: 0x00061EB0
		public static float ElasticIn(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			float num = c;
			bool flag = t == 0f;
			float result;
			if (flag)
			{
				result = b;
			}
			else
			{
				bool flag2 = (t /= d) == 1f;
				if (flag2)
				{
					result = b + c;
				}
				else
				{
					double num2 = (double)d * 0.3;
					bool flag3 = num < Math.Abs(c);
					double num3;
					if (flag3)
					{
						num = c;
						num3 = num2 / 4.0;
					}
					else
					{
						num3 = num2 / 6.2831854820251465 * Math.Asin((double)(c / num));
					}
					result = -(float)((double)num * Math.Pow(2.0, (double)(10f * (t -= 1f))) * Math.Sin(((double)(t * d) - num3) * 6.2831854820251465 / num2)) + b;
				}
			}
			return result;
		}

		// Token: 0x060035C6 RID: 13766 RVA: 0x00063D84 File Offset: 0x00061F84
		public static float ElasticOut(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			float num = c;
			bool flag = t == 0f;
			float result;
			if (flag)
			{
				result = b;
			}
			else
			{
				bool flag2 = (t /= d) == 1f;
				if (flag2)
				{
					result = b + c;
				}
				else
				{
					double num2 = (double)d * 0.3;
					bool flag3 = num < Math.Abs(c);
					double num3;
					if (flag3)
					{
						num = c;
						num3 = num2 / 4.0;
					}
					else
					{
						num3 = num2 / 6.2831854820251465 * Math.Asin((double)(c / num));
					}
					result = num * (float)(Math.Pow(2.0, (double)(-10f * t)) * Math.Sin(((double)(t * d) - num3) * 6.2831854820251465 / num2)) + c + b;
				}
			}
			return result;
		}

		// Token: 0x060035C7 RID: 13767 RVA: 0x00063E4C File Offset: 0x0006204C
		public static float ElasticInOut(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			float num = c;
			bool flag = t == 0f;
			float result;
			if (flag)
			{
				result = b;
			}
			else
			{
				bool flag2 = (t /= d / 2f) == 2f;
				if (flag2)
				{
					result = b + c;
				}
				else
				{
					double num2 = (double)d * 0.44999999999999996;
					bool flag3 = num < Math.Abs(c);
					double num3;
					if (flag3)
					{
						num = c;
						num3 = num2 / 4.0;
					}
					else
					{
						num3 = num2 / 6.2831854820251465 * Math.Asin((double)(c / num));
					}
					bool flag4 = t < 1f;
					if (flag4)
					{
						result = -0.5f * (float)((double)num * Math.Pow(2.0, (double)(10f * (t -= 1f))) * Math.Sin(((double)(t * d) - num3) * 6.2831854820251465 / num2)) + b;
					}
					else
					{
						result = (float)((double)num * Math.Pow(2.0, (double)(-10f * (t -= 1f))) * Math.Sin(((double)(t * d) - num3) * 6.2831854820251465 / num2) * 0.5 + (double)c + (double)b);
					}
				}
			}
			return result;
		}

		// Token: 0x060035C8 RID: 13768 RVA: 0x00063F88 File Offset: 0x00062188
		public static float BackIn(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			float num = 1.70158f;
			return c * (t /= d) * t * ((num + 1f) * t - num) + b;
		}

		// Token: 0x060035C9 RID: 13769 RVA: 0x00063FB8 File Offset: 0x000621B8
		public static float BackOut(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			float num = 1.70158f;
			return c * ((t = t / d - 1f) * t * ((num + 1f) * t + num) + 1f) + b;
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x00063FF4 File Offset: 0x000621F4
		public static float BackInOut(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			float num = 1.70158f;
			bool flag = (t /= d / 2f) < 1f;
			float result;
			if (flag)
			{
				result = c / 2f * (t * t * (((num *= 1.525f) + 1f) * t - num)) + b;
			}
			else
			{
				result = c / 2f * ((t -= 2f) * t * (((num *= 1.525f) + 1f) * t + num) + 2f) + b;
			}
			return result;
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x00064078 File Offset: 0x00062278
		public static float BounceIn(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			return c - Easing.BounceOut(d - t, 0f, c, d) + b;
		}

		// Token: 0x060035CC RID: 13772 RVA: 0x000640A0 File Offset: 0x000622A0
		public static float BounceOut(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			bool flag = (double)(t /= d) < 0.36363636363636365;
			float result;
			if (flag)
			{
				result = c * (7.5625f * t * t) + b;
			}
			else
			{
				bool flag2 = (double)t < 0.7272727272727273;
				if (flag2)
				{
					result = c * (7.5625f * (t -= 0.54545456f) * t + 0.75f) + b;
				}
				else
				{
					bool flag3 = (double)t < 0.9090909090909091;
					if (flag3)
					{
						result = c * (7.5625f * (t -= 0.8181818f) * t + 0.9375f) + b;
					}
					else
					{
						result = c * (7.5625f * (t -= 0.95454544f) * t + 0.984375f) + b;
					}
				}
			}
			return result;
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x0006415C File Offset: 0x0006235C
		public static float BounceInOut(float t, float b = 0f, float c = 1f, float d = 1f)
		{
			bool flag = t < d / 2f;
			float result;
			if (flag)
			{
				result = Easing.BounceIn(t * 2f, 0f, c, d) * 0.5f + b;
			}
			else
			{
				result = Easing.BounceOut(t * 2f - d, 0f, c, d) * 0.5f + c * 0.5f + b;
			}
			return result;
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x000641C0 File Offset: 0x000623C0
		public static double QuadEaseOut(double currentTime, double startValue, double endValue, double duration)
		{
			double num = currentTime / duration;
			return -endValue * num * (num - 2.0) + startValue;
		}

		// Token: 0x060035CF RID: 13775 RVA: 0x000641E8 File Offset: 0x000623E8
		public static double QuadEaseIn(double currentTime, double startValue, double endValue, double duration)
		{
			double num = currentTime / duration;
			return endValue * num * num + startValue;
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x00064208 File Offset: 0x00062408
		public static double BackEaseOutExtended(double currentTime, double startValue, double endValue, double duration)
		{
			double num = currentTime / duration - 1.0;
			return endValue * (num * num * (5.70158 * num + 4.70158) + 1.0) + startValue;
		}

		// Token: 0x060035D1 RID: 13777 RVA: 0x00064250 File Offset: 0x00062450
		public static float CubicEaseInAndOut(float t)
		{
			float num = (float)Math.Pow((double)(1f - t), 3.0) * 0f * 2f + 3f * (float)Math.Pow((double)(1f - t), 2.0) * t * 0.05f * 1f + 3f * (1f - t) * t * t * 0.95f * 2f + t * t * t * 1f * 1f;
			float num2 = (float)Math.Pow((double)(1f - t), 3.0) * 2f + 3f * (float)Math.Pow((double)(1f - t), 2.0) * t * 1f + 3f * (1f - t) * t * t * 2f + t * t * t * 1f;
			return num / num2;
		}

		// Token: 0x02000CC3 RID: 3267
		public enum EasingType
		{
			// Token: 0x04003FC6 RID: 16326
			Linear,
			// Token: 0x04003FC7 RID: 16327
			QuadIn,
			// Token: 0x04003FC8 RID: 16328
			QuadOut,
			// Token: 0x04003FC9 RID: 16329
			QuadInOut,
			// Token: 0x04003FCA RID: 16330
			CubicIn,
			// Token: 0x04003FCB RID: 16331
			CubicOut,
			// Token: 0x04003FCC RID: 16332
			CubicInOut,
			// Token: 0x04003FCD RID: 16333
			QuartIn,
			// Token: 0x04003FCE RID: 16334
			QuartOut,
			// Token: 0x04003FCF RID: 16335
			QuartInOut,
			// Token: 0x04003FD0 RID: 16336
			QuintIn,
			// Token: 0x04003FD1 RID: 16337
			QuintOut,
			// Token: 0x04003FD2 RID: 16338
			QuintInOut,
			// Token: 0x04003FD3 RID: 16339
			SineIn,
			// Token: 0x04003FD4 RID: 16340
			SineOut,
			// Token: 0x04003FD5 RID: 16341
			SineInOut,
			// Token: 0x04003FD6 RID: 16342
			ExpoIn,
			// Token: 0x04003FD7 RID: 16343
			ExpoOut,
			// Token: 0x04003FD8 RID: 16344
			ExpoInOut,
			// Token: 0x04003FD9 RID: 16345
			CircIn,
			// Token: 0x04003FDA RID: 16346
			CircOut,
			// Token: 0x04003FDB RID: 16347
			CircInOut,
			// Token: 0x04003FDC RID: 16348
			ElasticIn,
			// Token: 0x04003FDD RID: 16349
			ElasticOut,
			// Token: 0x04003FDE RID: 16350
			ElasticInOut,
			// Token: 0x04003FDF RID: 16351
			BackIn,
			// Token: 0x04003FE0 RID: 16352
			BackOut,
			// Token: 0x04003FE1 RID: 16353
			BackInOut,
			// Token: 0x04003FE2 RID: 16354
			BounceIn,
			// Token: 0x04003FE3 RID: 16355
			BounceOut,
			// Token: 0x04003FE4 RID: 16356
			BounceInOut
		}
	}
}
