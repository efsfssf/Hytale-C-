using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000135 RID: 309
	// (Invoke) Token: 0x06000973 RID: 2419
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnUnregisterPlayersCallbackInternal(ref UnregisterPlayersCallbackInfoInternal data);
}
