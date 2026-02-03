using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	// Token: 0x020004C0 RID: 1216
	// (Invoke) Token: 0x06001F92 RID: 8082
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnUserLoginStatusChangedCallbackInternal(ref UserLoginStatusChangedCallbackInfoInternal data);
}
