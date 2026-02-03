using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x0200059C RID: 1436
	// (Invoke) Token: 0x06002550 RID: 9552
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnRequestToJoinAcceptedCallbackInternal(ref OnRequestToJoinAcceptedCallbackInfoInternal data);
}
