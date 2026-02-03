using System;

namespace HytaleClient.InGame.Modules.AmbienceFX
{
	// Token: 0x02000997 RID: 2455
	internal class AmbienceFXSettings
	{
		// Token: 0x040029B4 RID: 10676
		public string Id;

		// Token: 0x040029B5 RID: 10677
		public AmbienceFXConditionSettings Conditions;

		// Token: 0x040029B6 RID: 10678
		public AmbienceFXSoundSettings[] Sounds;

		// Token: 0x040029B7 RID: 10679
		public uint MusicSoundEventIndex;

		// Token: 0x040029B8 RID: 10680
		public uint AmbientBedSoundEventIndex;

		// Token: 0x040029B9 RID: 10681
		public uint EffectSoundEventIndex;
	}
}
