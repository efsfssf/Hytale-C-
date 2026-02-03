using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.AntiCheatCommon;

namespace Epic.OnlineServices.AntiCheatServer
{
	// Token: 0x02000687 RID: 1671
	// (Invoke) Token: 0x06002B64 RID: 11108
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnMessageToClientCallbackInternal(ref OnMessageToClientCallbackInfoInternal data);
}
