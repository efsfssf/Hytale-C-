using System;

namespace HytaleClient.Data.Characters
{
	// Token: 0x02000B52 RID: 2898
	public class CharacterHeadAccessoryPart : CharacterPart
	{
		// Token: 0x04003782 RID: 14210
		public CharacterHeadAccessoryPart.CharacterHeadAccessoryType HeadAccessoryType;

		// Token: 0x02000F3E RID: 3902
		public enum CharacterHeadAccessoryType
		{
			// Token: 0x04004A85 RID: 19077
			Simple,
			// Token: 0x04004A86 RID: 19078
			HalfCovering,
			// Token: 0x04004A87 RID: 19079
			FullyCovering
		}
	}
}
