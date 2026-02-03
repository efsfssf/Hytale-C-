using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sanctions
{
	// Token: 0x0200019A RID: 410
	// (Invoke) Token: 0x06000BEB RID: 3051
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void CreatePlayerSanctionAppealCallbackInternal(ref CreatePlayerSanctionAppealCallbackInfoInternal data);
}
