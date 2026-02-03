using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000426 RID: 1062
	// (Invoke) Token: 0x06001C19 RID: 7193
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnLobbyMemberStatusReceivedCallbackInternal(ref LobbyMemberStatusReceivedCallbackInfoInternal data);
}
