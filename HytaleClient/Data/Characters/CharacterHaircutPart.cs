using System;

namespace HytaleClient.Data.Characters
{
	// Token: 0x02000B51 RID: 2897
	public class CharacterHaircutPart : CharacterPart
	{
		// Token: 0x04003780 RID: 14208
		public CharacterHaircutPart.CharacterHairType HairType;

		// Token: 0x04003781 RID: 14209
		public bool RequiresGenericHaircut;

		// Token: 0x02000F3D RID: 3901
		public enum CharacterHairType
		{
			// Token: 0x04004A81 RID: 19073
			Short,
			// Token: 0x04004A82 RID: 19074
			Medium,
			// Token: 0x04004A83 RID: 19075
			Long
		}
	}
}
