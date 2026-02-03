using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200042A RID: 1066
	// (Invoke) Token: 0x06001C29 RID: 7209
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnLobbyUpdateReceivedCallbackInternal(ref LobbyUpdateReceivedCallbackInfoInternal data);
}
