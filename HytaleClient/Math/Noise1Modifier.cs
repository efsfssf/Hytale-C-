using System;
using HytaleClient.Protocol;

namespace HytaleClient.Math
{
	// Token: 0x020007ED RID: 2029
	public static class Noise1Modifier
	{
		// Token: 0x060036BA RID: 14010 RVA: 0x0006B3A8 File Offset: 0x000695A8
		public static Noise1 Clamped(Noise1 noise, NoiseConfig.ClampConfig config)
		{
			bool flag = config == null;
			Noise1 result;
			if (flag)
			{
				result = noise;
			}
			else
			{
				result = Noise1Modifier.Clamped(noise, config.Min, config.Max, config.Normalize);
			}
			return result;
		}

		// Token: 0x060036BB RID: 14011 RVA: 0x0006B3E0 File Offset: 0x000695E0
		public static Noise1 Clamped(Noise1 noise, float min, float max, bool normalize = false)
		{
			bool flag = min >= max;
			Noise1 result;
			if (flag)
			{
				result = Noise1Source.Zero;
			}
			else
			{
				bool flag2 = noise == Noise1Source.Zero || (min == -1f && max == 1f);
				if (flag2)
				{
					result = noise;
				}
				else
				{
					noise = new Noise1Modifier.Clamp(noise, min, max);
					if (normalize)
					{
						result = Noise1Modifier.Normalized(noise, min, max);
					}
					else
					{
						result = noise;
					}
				}
			}
			return result;
		}

		// Token: 0x060036BC RID: 14012 RVA: 0x0006B448 File Offset: 0x00069648
		public static Noise1 Normalized(Noise1 noise, float min, float max)
		{
			bool flag = min == max;
			Noise1 result;
			if (flag)
			{
				result = Noise1Source.Zero;
			}
			else
			{
				result = new Noise1Modifier.Normalize(noise, min, max);
			}
			return result;
		}

		// Token: 0x060036BD RID: 14013 RVA: 0x0006B474 File Offset: 0x00069674
		public static Noise1 Scaled(Noise1 noise, float frequency, float amplitude)
		{
			bool flag = noise == Noise1Source.Zero || (frequency == 1f && amplitude == 1f);
			Noise1 result;
			if (flag)
			{
				result = noise;
			}
			else
			{
				result = new Noise1Modifier.Scale(noise, frequency, amplitude);
			}
			return result;
		}

		// Token: 0x060036BE RID: 14014 RVA: 0x0006B4B4 File Offset: 0x000696B4
		public static Noise1 Seeded(Noise1 noise, int seed)
		{
			bool flag = noise == Noise1Source.Zero;
			Noise1 result;
			if (flag)
			{
				result = noise;
			}
			else
			{
				bool flag2 = seed == 0;
				if (flag2)
				{
					seed = Noise1Helper.Hash(Environment.TickCount, 31337);
				}
				result = new Noise1Modifier.Seed(noise, seed);
			}
			return result;
		}

		// Token: 0x02000CC6 RID: 3270
		public class Clamp : Noise1
		{
			// Token: 0x060063D3 RID: 25555 RVA: 0x0020D0D6 File Offset: 0x0020B2D6
			public Clamp(Noise1 noise, float min, float max)
			{
				this._noise = noise;
				this._min = min;
				this._max = max;
			}

			// Token: 0x060063D4 RID: 25556 RVA: 0x0020D0F8 File Offset: 0x0020B2F8
			public float Eval(int seed, float t)
			{
				float value = this._noise.Eval(seed, t);
				return MathHelper.Clamp(value, this._min, this._max);
			}

			// Token: 0x04003FE8 RID: 16360
			private readonly Noise1 _noise;

			// Token: 0x04003FE9 RID: 16361
			private readonly float _min;

			// Token: 0x04003FEA RID: 16362
			private readonly float _max;
		}

		// Token: 0x02000CC7 RID: 3271
		public class Normalize : Noise1
		{
			// Token: 0x060063D5 RID: 25557 RVA: 0x0020D12A File Offset: 0x0020B32A
			public Normalize(Noise1 noise, float min, float max)
			{
				this._noise = noise;
				this._min = min;
				this._invRange = 2f / (max - min);
			}

			// Token: 0x060063D6 RID: 25558 RVA: 0x0020D154 File Offset: 0x0020B354
			public float Eval(int seed, float t)
			{
				float num = this._noise.Eval(seed, t);
				return -1f + (num - this._min) * this._invRange;
			}

			// Token: 0x04003FEB RID: 16363
			private readonly Noise1 _noise;

			// Token: 0x04003FEC RID: 16364
			private readonly float _min;

			// Token: 0x04003FED RID: 16365
			private readonly float _invRange;
		}

		// Token: 0x02000CC8 RID: 3272
		public class Scale : Noise1
		{
			// Token: 0x060063D7 RID: 25559 RVA: 0x0020D189 File Offset: 0x0020B389
			public Scale(Noise1 noise, float frequency, float amplitude)
			{
				this._noise = noise;
				this._frequency = frequency;
				this._amplitude = amplitude;
			}

			// Token: 0x060063D8 RID: 25560 RVA: 0x0020D1A8 File Offset: 0x0020B3A8
			public float Eval(int seed, float t)
			{
				return this._noise.Eval(seed, t * this._frequency) * this._amplitude;
			}

			// Token: 0x04003FEE RID: 16366
			private readonly Noise1 _noise;

			// Token: 0x04003FEF RID: 16367
			private readonly float _frequency;

			// Token: 0x04003FF0 RID: 16368
			private readonly float _amplitude;
		}

		// Token: 0x02000CC9 RID: 3273
		public class Seed : Noise1
		{
			// Token: 0x060063D9 RID: 25561 RVA: 0x0020D1D5 File Offset: 0x0020B3D5
			public Seed(Noise1 noise, int seedOffset)
			{
				this._noise = noise;
				this._seedOffset = seedOffset;
			}

			// Token: 0x060063DA RID: 25562 RVA: 0x0020D1F0 File Offset: 0x0020B3F0
			public float Eval(int seed, float t)
			{
				return this._noise.Eval(seed + this._seedOffset, t);
			}

			// Token: 0x04003FF1 RID: 16369
			private readonly Noise1 _noise;

			// Token: 0x04003FF2 RID: 16370
			private readonly int _seedOffset;
		}
	}
}
