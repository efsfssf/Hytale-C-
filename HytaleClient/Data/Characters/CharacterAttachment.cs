using System;

namespace HytaleClient.Data.Characters
{
	// Token: 0x02000B4F RID: 2895
	public class CharacterAttachment
	{
		// Token: 0x06005997 RID: 22935 RVA: 0x001BAB75 File Offset: 0x001B8D75
		public CharacterAttachment(string model, string texture, bool isUsingBaseNodeOnly, byte gradientId = 0)
		{
			this.Model = model;
			this.Texture = texture;
			this.IsUsingBaseNodeOnly = isUsingBaseNodeOnly;
			this.GradientId = gradientId;
		}

		// Token: 0x04003778 RID: 14200
		public readonly string Model;

		// Token: 0x04003779 RID: 14201
		public readonly string Texture;

		// Token: 0x0400377A RID: 14202
		public readonly bool IsUsingBaseNodeOnly;

		// Token: 0x0400377B RID: 14203
		public readonly byte GradientId;
	}
}
