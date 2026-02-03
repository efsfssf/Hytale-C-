using System;
using HytaleClient.Protocol;

namespace HytaleClient.Math
{
	// Token: 0x020007EE RID: 2030
	public static class Noise1Source
	{
		// Token: 0x060036BF RID: 14015 RVA: 0x0006B4F8 File Offset: 0x000696F8
		public static Noise1 GetSource(NoiseType type)
		{
			Noise1 result;
			switch (type)
			{
			case 0:
				result = Noise1Source.Sin;
				break;
			case 1:
				result = Noise1Source.Cos;
				break;
			case 2:
				result = Noise1Source.PerlinLinear;
				break;
			case 3:
				result = Noise1Source.PerlinHermite;
				break;
			case 4:
				result = Noise1Source.PerlinQuintic;
				break;
			case 5:
				result = Noise1Source.Rand;
				break;
			default:
				result = Noise1Source.Zero;
				break;
			}
			return result;
		}

		// Token: 0x04001829 RID: 6185
		public static readonly Func<float, float> LinearInterpolation = (float t) => t;

		// Token: 0x0400182A RID: 6186
		public static readonly Func<float, float> HermiteInterpolation = (float t) => t * t * (3f - 2f * t);

		// Token: 0x0400182B RID: 6187
		public static readonly Func<float, float> QuinticInterpolation = (float t) => t * t * t * (t * (t * 6f - 15f) + 10f);

		// Token: 0x0400182C RID: 6188
		public static readonly Noise1Source.Constant Zero = new Noise1Source.Constant(0f);

		// Token: 0x0400182D RID: 6189
		public static readonly Noise1Source.Constant One = new Noise1Source.Constant(1f);

		// Token: 0x0400182E RID: 6190
		public static readonly Noise1Source.Perlin PerlinLinear = new Noise1Source.Perlin(Noise1Source.LinearInterpolation);

		// Token: 0x0400182F RID: 6191
		public static readonly Noise1Source.Perlin PerlinHermite = new Noise1Source.Perlin(Noise1Source.HermiteInterpolation);

		// Token: 0x04001830 RID: 6192
		public static readonly Noise1Source.Perlin PerlinQuintic = new Noise1Source.Perlin(Noise1Source.HermiteInterpolation);

		// Token: 0x04001831 RID: 6193
		public static readonly Noise1Source.Random Rand = new Noise1Source.Random();

		// Token: 0x04001832 RID: 6194
		public static readonly Noise1Source.Sine Sin = new Noise1Source.Sine();

		// Token: 0x04001833 RID: 6195
		public static readonly Noise1Source.Cosine Cos = new Noise1Source.Cosine();

		// Token: 0x02000CCA RID: 3274
		public class Constant : Noise1
		{
			// Token: 0x060063DB RID: 25563 RVA: 0x0020D216 File Offset: 0x0020B416
			public Constant(float value)
			{
				this._value = value;
			}

			// Token: 0x060063DC RID: 25564 RVA: 0x0020D228 File Offset: 0x0020B428
			public float Eval(int seed, float t)
			{
				return this._value;
			}

			// Token: 0x04003FF3 RID: 16371
			private readonly float _value;
		}

		// Token: 0x02000CCB RID: 3275
		public class Perlin : Noise1
		{
			// Token: 0x060063DD RID: 25565 RVA: 0x0020D240 File Offset: 0x0020B440
			public Perlin(Func<float, float> interpolation)
			{
				this._interpolation = interpolation;
			}

			// Token: 0x060063DE RID: 25566 RVA: 0x0020D254 File Offset: 0x0020B454
			public float Eval(int seed, float t)
			{
				int num = Noise1Helper.Floor(t);
				int num2 = Noise1Helper.Hash(seed, num);
				int num3 = Noise1Helper.Hash(seed, num + 1);
				float num4 = t - (float)num;
				float value = Noise1Source.Perlin.NOISE[num2 & Noise1Source.Perlin.MASK] * num4;
				float value2 = Noise1Source.Perlin.NOISE[num3 & Noise1Source.Perlin.MASK] * (num4 - 1f);
				float num5 = MathHelper.Lerp(value, value2, this._interpolation(num4));
				return MathHelper.Clamp(num5 * 2f, -1f, 1f);
			}

			// Token: 0x060063DF RID: 25567 RVA: 0x0020D2DC File Offset: 0x0020B4DC
			private static float[] CreateNoiseArray(int length, float min, float max, System.Random rnd)
			{
				float[] array = new float[length];
				float num = 1f;
				float num2 = 0f;
				for (int i = 0; i < length; i++)
				{
					float num3 = rnd.NextFloat(0f, 1f);
					array[i] = num3;
					num = Math.Min(num, num3);
					num2 = Math.Max(num2, num3);
				}
				float num4 = max - min;
				float num5 = num2 - num;
				for (int j = 0; j < length; j++)
				{
					float num6 = (array[j] - num) / num5;
					array[j] = min + num6 * num4;
				}
				return array;
			}

			// Token: 0x04003FF4 RID: 16372
			private static readonly int LENGTH = 32;

			// Token: 0x04003FF5 RID: 16373
			private static readonly int MASK = Noise1Source.Perlin.LENGTH - 1;

			// Token: 0x04003FF6 RID: 16374
			private static readonly float[] NOISE = Noise1Source.Perlin.CreateNoiseArray(Noise1Source.Perlin.LENGTH, -1f, 1f, new System.Random());

			// Token: 0x04003FF7 RID: 16375
			private Func<float, float> _interpolation;
		}

		// Token: 0x02000CCC RID: 3276
		public class Random : Noise1
		{
			// Token: 0x060063E1 RID: 25569 RVA: 0x0020D3B0 File Offset: 0x0020B5B0
			public float Eval(int seed, float t)
			{
				int x = Noise1Helper.ToInt(t);
				return Noise1Helper.Rand(seed, x);
			}
		}

		// Token: 0x02000CCD RID: 3277
		public class Sine : Noise1
		{
			// Token: 0x060063E3 RID: 25571 RVA: 0x0020D3DC File Offset: 0x0020B5DC
			public float Eval(int seed, float t)
			{
				return MathHelper.Sin(t);
			}
		}

		// Token: 0x02000CCE RID: 3278
		public class Cosine : Noise1
		{
			// Token: 0x060063E5 RID: 25573 RVA: 0x0020D400 File Offset: 0x0020B600
			public float Eval(int seed, float t)
			{
				return MathHelper.Cos(t);
			}
		}
	}
}
