using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x02000608 RID: 1544
	// (Invoke) Token: 0x060027FA RID: 10234
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnLogoutCallbackInternal(ref LogoutCallbackInfoInternal data);
}
