using System;

namespace Epic.OnlineServices
{
	// Token: 0x0200000F RID: 15
	[AttributeUsage(AttributeTargets.Method)]
	internal sealed class MonoPInvokeCallbackAttribute : Attribute
	{
		// Token: 0x06000086 RID: 134 RVA: 0x0000404A File Offset: 0x0000224A
		public MonoPInvokeCallbackAttribute(Type type)
		{
		}
	}
}
