using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000412 RID: 1042
	// (Invoke) Token: 0x06001BC9 RID: 7113
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnJoinLobbyByIdCallbackInternal(ref JoinLobbyByIdCallbackInfoInternal data);
}
