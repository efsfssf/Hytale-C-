using System;

namespace HytaleClient.InGame.Modules.AmbienceFX
{
	// Token: 0x02000993 RID: 2451
	internal class AmbienceFXConditionSettings
	{
		// Token: 0x0400297C RID: 10620
		public int[] EnvironmentIndices;

		// Token: 0x0400297D RID: 10621
		public int[] WeatherIndices;

		// Token: 0x0400297E RID: 10622
		public int[] FluidFXIndices;

		// Token: 0x0400297F RID: 10623
		public AmbienceFXConditionSettings.AmbienceFXBlockSoundSet[] SurroundingBlockSoundSets;

		// Token: 0x04002980 RID: 10624
		public Range Altitude;

		// Token: 0x04002981 RID: 10625
		public Range Walls;

		// Token: 0x04002982 RID: 10626
		public bool Roof;

		// Token: 0x04002983 RID: 10627
		public bool Floor;

		// Token: 0x04002984 RID: 10628
		public Range SunLightLevel;

		// Token: 0x04002985 RID: 10629
		public Range TorchLightLevel;

		// Token: 0x04002986 RID: 10630
		public Range GlobalLightLevel;

		// Token: 0x04002987 RID: 10631
		public Rangef DayTime;

		// Token: 0x02000E84 RID: 3716
		public struct AmbienceFXBlockSoundSet
		{
			// Token: 0x040046E2 RID: 18146
			public int BlockSoundSetIndex;

			// Token: 0x040046E3 RID: 18147
			public Rangef Percent;
		}
	}
}
