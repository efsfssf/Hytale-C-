using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200041A RID: 1050
	// (Invoke) Token: 0x06001BE9 RID: 7145
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnLeaveLobbyCallbackInternal(ref LeaveLobbyCallbackInfoInternal data);
}
