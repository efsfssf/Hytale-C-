using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.Platform;

namespace Epic.OnlineServices
{
	// Token: 0x02000012 RID: 18
	public static class AndroidBindings
	{
		// Token: 0x0600030F RID: 783
		[DllImport("EOSSDK-Win64-Shipping.dll")]
		internal static extern Result EOS_Initialize(ref AndroidInitializeOptionsInternal options);
	}
}
