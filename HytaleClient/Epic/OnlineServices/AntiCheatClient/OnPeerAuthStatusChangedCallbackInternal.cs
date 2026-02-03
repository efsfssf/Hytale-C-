using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.AntiCheatCommon;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006F1 RID: 1777
	// (Invoke) Token: 0x06002DD3 RID: 11731
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnPeerAuthStatusChangedCallbackInternal(ref OnClientAuthStatusChangedCallbackInfoInternal data);
}
