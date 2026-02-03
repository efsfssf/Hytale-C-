using System;

namespace HytaleClient.Utils
{
	// Token: 0x020007D1 RID: 2001
	internal static class StaticRandom
	{
		// Token: 0x06003434 RID: 13364 RVA: 0x00053BD4 File Offset: 0x00051DD4
		public static float NextFloat(long seed)
		{
			return (float)(seed & 16777215L) / 16777216f;
		}

		// Token: 0x06003435 RID: 13365 RVA: 0x00053BF8 File Offset: 0x00051DF8
		public static float NextFloat(long seed, float min, float max)
		{
			return min + StaticRandom.NextFloat(seed) * (max - min);
		}
	}
}
