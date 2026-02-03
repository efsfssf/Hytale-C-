using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200012F RID: 303
	// (Invoke) Token: 0x0600095B RID: 2395
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnSessionInviteReceivedCallbackInternal(ref SessionInviteReceivedCallbackInfoInternal data);
}
