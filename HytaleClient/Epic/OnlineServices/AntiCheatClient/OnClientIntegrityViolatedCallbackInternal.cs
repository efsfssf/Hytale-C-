using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006E5 RID: 1765
	// (Invoke) Token: 0x06002D91 RID: 11665
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnClientIntegrityViolatedCallbackInternal(ref OnClientIntegrityViolatedCallbackInfoInternal data);
}
