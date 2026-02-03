using System;

namespace HytaleClient.InGame.Modules.AmbienceFX
{
	// Token: 0x02000995 RID: 2453
	public struct Range
	{
		// Token: 0x06004E6E RID: 20078 RVA: 0x0015C58A File Offset: 0x0015A78A
		public Range(int min, int max)
		{
			this.Min = min;
			this.Max = max;
		}

		// Token: 0x040029B0 RID: 10672
		public int Min;

		// Token: 0x040029B1 RID: 10673
		public int Max;
	}
}
