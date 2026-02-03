using System;

namespace HytaleClient.Math
{
	// Token: 0x020007EC RID: 2028
	public static class Noise1Combiner
	{
		// Token: 0x060036B9 RID: 14009 RVA: 0x0006B378 File Offset: 0x00069578
		public static Noise1 Summed(Noise1[] noises)
		{
			bool flag = noises == null || noises.Length == 0;
			Noise1 result;
			if (flag)
			{
				result = Noise1Source.Zero;
			}
			else
			{
				result = new Noise1Combiner.Sum(noises);
			}
			return result;
		}

		// Token: 0x02000CC5 RID: 3269
		public class Sum : Noise1
		{
			// Token: 0x060063D1 RID: 25553 RVA: 0x0020D082 File Offset: 0x0020B282
			public Sum(Noise1[] noise)
			{
				this._noise = noise;
			}

			// Token: 0x060063D2 RID: 25554 RVA: 0x0020D094 File Offset: 0x0020B294
			public float Eval(int seed, float t)
			{
				float num = 0f;
				foreach (Noise1 noise2 in this._noise)
				{
					num += noise2.Eval(seed, t);
				}
				return num;
			}

			// Token: 0x04003FE7 RID: 16359
			private readonly Noise1[] _noise;
		}
	}
}
