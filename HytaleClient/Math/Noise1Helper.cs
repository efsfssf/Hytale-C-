using System;
using System.Runtime.InteropServices;
using HytaleClient.Protocol;

namespace HytaleClient.Math
{
	// Token: 0x020007EB RID: 2027
	public static class Noise1Helper
	{
		// Token: 0x060036B2 RID: 14002 RVA: 0x0006B1B0 File Offset: 0x000693B0
		public static int Floor(float value)
		{
			int num = (int)value;
			return (value < 0f) ? (num - 1) : num;
		}

		// Token: 0x060036B3 RID: 14003 RVA: 0x0006B1D4 File Offset: 0x000693D4
		public static int Hash(int seed, int x = 31337)
		{
			int num = seed ^ 1619 * x;
			num = num * num * num * 60493;
			return num >> 13 ^ num;
		}

		// Token: 0x060036B4 RID: 14004 RVA: 0x0006B204 File Offset: 0x00069404
		public static float Rand(int seed, int x)
		{
			int num = seed ^ 1619 * x;
			float value = (float)(num * num * num * 60493) / 2.1474836E+09f;
			return MathHelper.Clamp(value, -1f, 1f);
		}

		// Token: 0x060036B5 RID: 14005 RVA: 0x0006B244 File Offset: 0x00069444
		public static int ToInt(float value)
		{
			return new Noise1Helper.Float2Int
			{
				f = value
			}.i;
		}

		// Token: 0x060036B6 RID: 14006 RVA: 0x0006B26C File Offset: 0x0006946C
		public static Noise1 CreateNoise(NoiseConfig config)
		{
			bool flag = config == null || config.Frequency == 0f || config.Amplitude == 0f;
			Noise1 result;
			if (flag)
			{
				result = Noise1Source.Zero;
			}
			else
			{
				Noise1 noise = Noise1Source.GetSource(config.Type);
				noise = Noise1Modifier.Seeded(noise, config.Seed);
				noise = Noise1Modifier.Clamped(noise, config.Clamp);
				noise = Noise1Modifier.Scaled(noise, config.Frequency, config.Amplitude);
				result = noise;
			}
			return result;
		}

		// Token: 0x060036B7 RID: 14007 RVA: 0x0006B2E4 File Offset: 0x000694E4
		public static Noise1[] CreateNoises(NoiseConfig[] configs)
		{
			bool flag = configs == null || configs.Length == 0;
			Noise1[] result;
			if (flag)
			{
				result = Noise1Helper.EMPTY_ARRAY;
			}
			else
			{
				bool flag2 = false;
				Noise1[] array = new Noise1[configs.Length];
				for (int i = 0; i < configs.Length; i++)
				{
					NoiseConfig config = configs[i];
					Noise1 noise = Noise1Helper.CreateNoise(config);
					array[i] = noise;
					flag2 |= (noise != Noise1Source.Zero);
				}
				bool flag3 = !flag2;
				if (flag3)
				{
					result = Noise1Helper.EMPTY_ARRAY;
				}
				else
				{
					result = array;
				}
			}
			return result;
		}

		// Token: 0x04001823 RID: 6179
		private static readonly Noise1[] EMPTY_ARRAY = new Noise1[0];

		// Token: 0x04001824 RID: 6180
		public const int RandomSeed = 0;

		// Token: 0x04001825 RID: 6181
		public const int XPrime = 1619;

		// Token: 0x04001826 RID: 6182
		public const int YPrime = 31337;

		// Token: 0x04001827 RID: 6183
		public const int HashPrime = 60493;

		// Token: 0x04001828 RID: 6184
		public const float Int2FloatDenom = 2.1474836E+09f;

		// Token: 0x02000CC4 RID: 3268
		[StructLayout(LayoutKind.Explicit)]
		private struct Float2Int
		{
			// Token: 0x04003FE5 RID: 16357
			[FieldOffset(0)]
			public float f;

			// Token: 0x04003FE6 RID: 16358
			[FieldOffset(0)]
			public int i;
		}
	}
}
