using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005A0 RID: 1440
	// (Invoke) Token: 0x0600256B RID: 9579
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnRequestToJoinReceivedCallbackInternal(ref RequestToJoinReceivedCallbackInfoInternal data);
}
