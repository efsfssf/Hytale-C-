using System;

namespace HytaleClient.Interface.UI.Markup
{
	// Token: 0x0200084E RID: 2126
	public class UIPath
	{
		// Token: 0x06003B05 RID: 15109 RVA: 0x0008A320 File Offset: 0x00088520
		public UIPath(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.Value = value;
		}

		// Token: 0x04001B36 RID: 6966
		public readonly string Value;
	}
}
