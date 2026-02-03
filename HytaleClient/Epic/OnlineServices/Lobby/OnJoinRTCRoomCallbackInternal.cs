using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000416 RID: 1046
	// (Invoke) Token: 0x06001BD9 RID: 7129
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnJoinRTCRoomCallbackInternal(ref JoinRTCRoomCallbackInfoInternal data);
}
