using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005A2 RID: 1442
	// (Invoke) Token: 0x06002573 RID: 9587
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnRequestToJoinRejectedCallbackInternal(ref OnRequestToJoinRejectedCallbackInfoInternal data);
}
