using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000420 RID: 1056
	// (Invoke) Token: 0x06001C01 RID: 7169
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnLobbyInviteAcceptedCallbackInternal(ref LobbyInviteAcceptedCallbackInfoInternal data);
}
