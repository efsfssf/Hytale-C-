using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001C7 RID: 455
	// (Invoke) Token: 0x06000D2B RID: 3371
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnLeaveRoomCallbackInternal(ref LeaveRoomCallbackInfoInternal data);
}
