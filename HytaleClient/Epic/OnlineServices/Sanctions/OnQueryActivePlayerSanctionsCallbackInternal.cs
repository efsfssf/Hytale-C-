using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sanctions
{
	// Token: 0x020001A2 RID: 418
	// (Invoke) Token: 0x06000C18 RID: 3096
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryActivePlayerSanctionsCallbackInternal(ref QueryActivePlayerSanctionsCallbackInfoInternal data);
}
