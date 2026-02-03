using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.AntiCheatCommon;

namespace Epic.OnlineServices.AntiCheatServer
{
	// Token: 0x02000683 RID: 1667
	// (Invoke) Token: 0x06002B54 RID: 11092
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnClientActionRequiredCallbackInternal(ref OnClientActionRequiredCallbackInfoInternal data);
}
