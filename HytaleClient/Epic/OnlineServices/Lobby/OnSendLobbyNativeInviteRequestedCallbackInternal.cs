using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000436 RID: 1078
	// (Invoke) Token: 0x06001C59 RID: 7257
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnSendLobbyNativeInviteRequestedCallbackInternal(ref SendLobbyNativeInviteRequestedCallbackInfoInternal data);
}
