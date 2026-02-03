using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	// Token: 0x020004C2 RID: 1218
	// (Invoke) Token: 0x06001F9A RID: 8090
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntegratedPlatformPreLogoutAction OnUserPreLogoutCallbackInternal(ref UserPreLogoutCallbackInfoInternal data);
}
