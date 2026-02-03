using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200040C RID: 1036
	// (Invoke) Token: 0x06001BB1 RID: 7089
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnDestroyLobbyCallbackInternal(ref DestroyLobbyCallbackInfoInternal data);
}
