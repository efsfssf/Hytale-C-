using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005AC RID: 1452
	// (Invoke) Token: 0x060025A6 RID: 9638
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnSendRequestToJoinCallbackInternal(ref SendRequestToJoinCallbackInfoInternal data);
}
