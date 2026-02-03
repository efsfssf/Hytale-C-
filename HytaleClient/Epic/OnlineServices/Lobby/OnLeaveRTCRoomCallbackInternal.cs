using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200041E RID: 1054
	// (Invoke) Token: 0x06001BF9 RID: 7161
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnLeaveRTCRoomCallbackInternal(ref LeaveRTCRoomCallbackInfoInternal data);
}
