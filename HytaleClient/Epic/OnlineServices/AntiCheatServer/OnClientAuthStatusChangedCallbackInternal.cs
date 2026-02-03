using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.AntiCheatCommon;

namespace Epic.OnlineServices.AntiCheatServer
{
	// Token: 0x02000685 RID: 1669
	// (Invoke) Token: 0x06002B5C RID: 11100
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnClientAuthStatusChangedCallbackInternal(ref OnClientAuthStatusChangedCallbackInfoInternal data);
}
