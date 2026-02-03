using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200041C RID: 1052
	// (Invoke) Token: 0x06001BF1 RID: 7153
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnLeaveLobbyRequestedCallbackInternal(ref LeaveLobbyRequestedCallbackInfoInternal data);
}
