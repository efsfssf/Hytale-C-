using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000428 RID: 1064
	// (Invoke) Token: 0x06001C21 RID: 7201
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnLobbyMemberUpdateReceivedCallbackInternal(ref LobbyMemberUpdateReceivedCallbackInfoInternal data);
}
