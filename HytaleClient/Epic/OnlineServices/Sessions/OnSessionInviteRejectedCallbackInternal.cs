using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000131 RID: 305
	// (Invoke) Token: 0x06000963 RID: 2403
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnSessionInviteRejectedCallbackInternal(ref SessionInviteRejectedCallbackInfoInternal data);
}
