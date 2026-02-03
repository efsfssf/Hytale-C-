using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000432 RID: 1074
	// (Invoke) Token: 0x06001C49 RID: 7241
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnRTCRoomConnectionChangedCallbackInternal(ref RTCRoomConnectionChangedCallbackInfoInternal data);
}
