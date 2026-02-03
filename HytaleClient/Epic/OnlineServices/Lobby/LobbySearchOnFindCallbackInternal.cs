using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003FA RID: 1018
	// (Invoke) Token: 0x06001B59 RID: 7001
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void LobbySearchOnFindCallbackInternal(ref LobbySearchFindCallbackInfoInternal data);
}
