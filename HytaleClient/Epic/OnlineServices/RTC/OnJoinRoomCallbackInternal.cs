using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001C5 RID: 453
	// (Invoke) Token: 0x06000D23 RID: 3363
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnJoinRoomCallbackInternal(ref JoinRoomCallbackInfoInternal data);
}
