using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.AntiCheatCommon;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006E9 RID: 1769
	// (Invoke) Token: 0x06002DAC RID: 11692
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnMessageToPeerCallbackInternal(ref OnMessageToClientCallbackInfoInternal data);
}
