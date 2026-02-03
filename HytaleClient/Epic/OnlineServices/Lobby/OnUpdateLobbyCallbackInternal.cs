using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000438 RID: 1080
	// (Invoke) Token: 0x06001C61 RID: 7265
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnUpdateLobbyCallbackInternal(ref UpdateLobbyCallbackInfoInternal data);
}
