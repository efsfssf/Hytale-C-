using System;
using System.Collections.Generic;
using HytaleClient.Interface.UI.Elements;

namespace HytaleClient.Interface.UI.Markup
{
	// Token: 0x0200084A RID: 2122
	public class UIFragment
	{
		// Token: 0x06003B00 RID: 15104 RVA: 0x0008A2CB File Offset: 0x000884CB
		public T Get<T>(string name) where T : Element
		{
			return this.ElementsByName[name] as T;
		}

		// Token: 0x04001B32 RID: 6962
		public List<Element> RootElements;

		// Token: 0x04001B33 RID: 6963
		public readonly Dictionary<string, Element> ElementsByName = new Dictionary<string, Element>();
	}
}
