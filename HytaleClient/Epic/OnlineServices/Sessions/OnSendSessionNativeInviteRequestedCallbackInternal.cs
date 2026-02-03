using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200012B RID: 299
	// (Invoke) Token: 0x0600094B RID: 2379
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnSendSessionNativeInviteRequestedCallbackInternal(ref SendSessionNativeInviteRequestedCallbackInfoInternal data);
}
