using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000422 RID: 1058
	// (Invoke) Token: 0x06001C09 RID: 7177
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnLobbyInviteReceivedCallbackInternal(ref LobbyInviteReceivedCallbackInfoInternal data);
}
