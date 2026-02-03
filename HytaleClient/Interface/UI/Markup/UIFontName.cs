using System;

namespace HytaleClient.Interface.UI.Markup
{
	// Token: 0x02000849 RID: 2121
	public class UIFontName
	{
		// Token: 0x06003AFF RID: 15103 RVA: 0x0008A2AB File Offset: 0x000884AB
		public UIFontName(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.Value = value;
		}

		// Token: 0x04001B31 RID: 6961
		public readonly string Value;
	}
}
