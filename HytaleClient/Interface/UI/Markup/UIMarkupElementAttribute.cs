using System;

namespace HytaleClient.Interface.UI.Markup
{
	// Token: 0x0200084C RID: 2124
	[AttributeUsage(AttributeTargets.Class)]
	internal class UIMarkupElementAttribute : Attribute
	{
		// Token: 0x04001B34 RID: 6964
		public bool AcceptsChildren = false;

		// Token: 0x04001B35 RID: 6965
		public bool ExposeInheritedProperties = true;
	}
}
