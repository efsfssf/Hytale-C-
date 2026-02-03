using System;
using System.Collections.Generic;

namespace HytaleClient.Interface.UI.Markup
{
	// Token: 0x02000846 RID: 2118
	public class ResolutionContext
	{
		// Token: 0x06003AED RID: 15085 RVA: 0x00089A50 File Offset: 0x00087C50
		public ResolutionContext(IUIProvider provider)
		{
			this.Provider = provider;
		}

		// Token: 0x04001B28 RID: 6952
		public readonly IUIProvider Provider;

		// Token: 0x04001B29 RID: 6953
		public readonly HashSet<Expression> ExpressionPath = new HashSet<Expression>();
	}
}
