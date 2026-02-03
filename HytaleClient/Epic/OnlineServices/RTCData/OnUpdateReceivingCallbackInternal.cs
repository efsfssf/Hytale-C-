using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCData
{
	// Token: 0x020001E5 RID: 485
	// (Invoke) Token: 0x06000E01 RID: 3585
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnUpdateReceivingCallbackInternal(ref UpdateReceivingCallbackInfoInternal data);
}
