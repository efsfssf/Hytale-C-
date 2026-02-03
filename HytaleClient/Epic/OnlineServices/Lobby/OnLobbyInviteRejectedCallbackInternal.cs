using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000424 RID: 1060
	// (Invoke) Token: 0x06001C11 RID: 7185
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnLobbyInviteRejectedCallbackInternal(ref LobbyInviteRejectedCallbackInfoInternal data);
}
