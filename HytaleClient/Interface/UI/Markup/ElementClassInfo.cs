using System;
using System.Collections.Generic;
using System.Reflection;

namespace HytaleClient.Interface.UI.Markup
{
	// Token: 0x02000844 RID: 2116
	public class ElementClassInfo
	{
		// Token: 0x04001B14 RID: 6932
		public string Name;

		// Token: 0x04001B15 RID: 6933
		public ConstructorInfo Constructor;

		// Token: 0x04001B16 RID: 6934
		public bool AcceptsChildren;

		// Token: 0x04001B17 RID: 6935
		public Dictionary<string, Type> PropertyTypes = new Dictionary<string, Type>();
	}
}
