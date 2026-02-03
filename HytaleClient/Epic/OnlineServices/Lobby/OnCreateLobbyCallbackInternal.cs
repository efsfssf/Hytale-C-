using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200040A RID: 1034
	// (Invoke) Token: 0x06001BA9 RID: 7081
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnCreateLobbyCallbackInternal(ref CreateLobbyCallbackInfoInternal data);
}
