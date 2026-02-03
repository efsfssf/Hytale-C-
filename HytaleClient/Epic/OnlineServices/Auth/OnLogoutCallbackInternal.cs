using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000656 RID: 1622
	// (Invoke) Token: 0x060029FD RID: 10749
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnLogoutCallbackInternal(ref LogoutCallbackInfoInternal data);
}
