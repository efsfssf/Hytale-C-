using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005A6 RID: 1446
	// (Invoke) Token: 0x0600258E RID: 9614
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnRequestToJoinResponseReceivedCallbackInternal(ref RequestToJoinResponseReceivedCallbackInfoInternal data);
}
