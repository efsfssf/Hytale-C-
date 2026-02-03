using System;

namespace Epic.OnlineServices
{
	// Token: 0x0200000A RID: 10
	internal class DynamicBindingException : Exception
	{
		// Token: 0x0600007F RID: 127 RVA: 0x00004035 File Offset: 0x00002235
		public DynamicBindingException(string bindingName) : base(string.Format("Failed to hook dynamic binding for '{0}'", bindingName))
		{
		}
	}
}
