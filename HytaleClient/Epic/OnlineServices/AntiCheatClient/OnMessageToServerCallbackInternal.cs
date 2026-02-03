using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006EB RID: 1771
	// (Invoke) Token: 0x06002DB4 RID: 11700
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnMessageToServerCallbackInternal(ref OnMessageToServerCallbackInfoInternal data);
}
