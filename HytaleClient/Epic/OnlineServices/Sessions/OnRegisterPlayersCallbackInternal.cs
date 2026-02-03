using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000125 RID: 293
	// (Invoke) Token: 0x06000933 RID: 2355
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnRegisterPlayersCallbackInternal(ref RegisterPlayersCallbackInfoInternal data);
}
