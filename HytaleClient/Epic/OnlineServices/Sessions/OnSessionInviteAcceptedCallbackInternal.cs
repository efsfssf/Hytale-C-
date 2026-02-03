using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200012D RID: 301
	// (Invoke) Token: 0x06000953 RID: 2387
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnSessionInviteAcceptedCallbackInternal(ref SessionInviteAcceptedCallbackInfoInternal data);
}
