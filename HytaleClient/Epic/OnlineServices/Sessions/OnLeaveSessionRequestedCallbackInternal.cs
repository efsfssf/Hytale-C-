using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200011F RID: 287
	// (Invoke) Token: 0x06000923 RID: 2339
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnLeaveSessionRequestedCallbackInternal(ref LeaveSessionRequestedCallbackInfoInternal data);
}
