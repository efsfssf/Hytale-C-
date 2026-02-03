using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.AntiCheatCommon;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006EF RID: 1775
	// (Invoke) Token: 0x06002DCB RID: 11723
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnPeerActionRequiredCallbackInternal(ref OnClientActionRequiredCallbackInfoInternal data);
}
