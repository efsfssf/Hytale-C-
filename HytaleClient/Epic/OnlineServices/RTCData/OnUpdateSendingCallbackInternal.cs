using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCData
{
	// Token: 0x020001E7 RID: 487
	// (Invoke) Token: 0x06000E09 RID: 3593
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnUpdateSendingCallbackInternal(ref UpdateSendingCallbackInfoInternal data);
}
