using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCData
{
	// Token: 0x020001E1 RID: 481
	// (Invoke) Token: 0x06000DF1 RID: 3569
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnDataReceivedCallbackInternal(ref DataReceivedCallbackInfoInternal data);
}
