using System;

namespace HytaleClient.InGame.Modules.AmbienceFX
{
	// Token: 0x02000998 RID: 2456
	internal class AmbienceFXSoundSettings
	{
		// Token: 0x040029BA RID: 10682
		public uint SoundEventIndex;

		// Token: 0x040029BB RID: 10683
		public AmbienceFXSoundSettings.AmbienceFXSoundPlay3D Play3D;

		// Token: 0x040029BC RID: 10684
		public int BlockSoundSetIndex;

		// Token: 0x040029BD RID: 10685
		public AmbienceFXSoundSettings.AmbienceFXAltitude Altitude;

		// Token: 0x040029BE RID: 10686
		public Rangef Frequency;

		// Token: 0x040029BF RID: 10687
		public float LastTime;

		// Token: 0x040029C0 RID: 10688
		public float NextTime;

		// Token: 0x040029C1 RID: 10689
		public Range Radius;

		// Token: 0x02000E86 RID: 3718
		public enum AmbienceFXSoundPlay3D
		{
			// Token: 0x040046E7 RID: 18151
			Random,
			// Token: 0x040046E8 RID: 18152
			LocationName,
			// Token: 0x040046E9 RID: 18153
			No
		}

		// Token: 0x02000E87 RID: 3719
		public enum AmbienceFXAltitude
		{
			// Token: 0x040046EB RID: 18155
			Normal,
			// Token: 0x040046EC RID: 18156
			Lowest,
			// Token: 0x040046ED RID: 18157
			Highest,
			// Token: 0x040046EE RID: 18158
			Random
		}
	}
}
