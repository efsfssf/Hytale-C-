using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin
{
	// Token: 0x0200028F RID: 655
	// (Invoke) Token: 0x06001256 RID: 4694
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryJoinRoomTokenCompleteCallbackInternal(ref QueryJoinRoomTokenCompleteCallbackInfoInternal data);
}
