using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000414 RID: 1044
	// (Invoke) Token: 0x06001BD1 RID: 7121
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnJoinLobbyCallbackInternal(ref JoinLobbyCallbackInfoInternal data);
}
