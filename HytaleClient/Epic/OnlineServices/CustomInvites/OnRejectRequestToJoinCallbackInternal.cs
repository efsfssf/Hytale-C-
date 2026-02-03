using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x0200059A RID: 1434
	// (Invoke) Token: 0x06002548 RID: 9544
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnRejectRequestToJoinCallbackInternal(ref RejectRequestToJoinCallbackInfoInternal data);
}
