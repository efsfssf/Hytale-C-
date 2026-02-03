using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x0200058E RID: 1422
	// (Invoke) Token: 0x060024F2 RID: 9458
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnAcceptRequestToJoinCallbackInternal(ref AcceptRequestToJoinCallbackInfoInternal data);
}
