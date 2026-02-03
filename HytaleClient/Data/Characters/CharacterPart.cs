using System;
using System.Collections.Generic;

namespace HytaleClient.Data.Characters
{
	// Token: 0x02000B53 RID: 2899
	public class CharacterPart
	{
		// Token: 0x04003783 RID: 14211
		public string Id;

		// Token: 0x04003784 RID: 14212
		public string Name;

		// Token: 0x04003785 RID: 14213
		public string Model;

		// Token: 0x04003786 RID: 14214
		public string GradientSet;

		// Token: 0x04003787 RID: 14215
		public string GreyscaleTexture;

		// Token: 0x04003788 RID: 14216
		public Dictionary<string, CharacterPartTexture> Textures;

		// Token: 0x04003789 RID: 14217
		public Dictionary<string, CharacterPartVariant> Variants;

		// Token: 0x0400378A RID: 14218
		public CharacterBodyType DefaultFor = CharacterBodyType.None;

		// Token: 0x0400378B RID: 14219
		public string[] Tags;
	}
}
