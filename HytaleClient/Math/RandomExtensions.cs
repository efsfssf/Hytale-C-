using System;

namespace HytaleClient.Math
{
	// Token: 0x020007F3 RID: 2035
	internal static class RandomExtensions
	{
		// Token: 0x06003728 RID: 14120 RVA: 0x0006D4B8 File Offset: 0x0006B6B8
		public static float NextFloat(this Random random, float min, float max)
		{
			return (float)((double)min + random.NextDouble() * (double)(max - min));
		}
	}
}
