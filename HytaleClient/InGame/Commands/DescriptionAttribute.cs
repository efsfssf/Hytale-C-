using System;

namespace HytaleClient.InGame.Commands
{
	// Token: 0x020008E8 RID: 2280
	public class DescriptionAttribute : Attribute
	{
		// Token: 0x0600437E RID: 17278 RVA: 0x000D6824 File Offset: 0x000D4A24
		public DescriptionAttribute(string description)
		{
			this.Description = description;
		}

		// Token: 0x04002140 RID: 8512
		public readonly string Description;
	}
}
