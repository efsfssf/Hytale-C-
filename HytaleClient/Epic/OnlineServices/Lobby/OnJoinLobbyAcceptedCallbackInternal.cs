using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000410 RID: 1040
	// (Invoke) Token: 0x06001BC1 RID: 7105
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnJoinLobbyAcceptedCallbackInternal(ref JoinLobbyAcceptedCallbackInfoInternal data);
}
