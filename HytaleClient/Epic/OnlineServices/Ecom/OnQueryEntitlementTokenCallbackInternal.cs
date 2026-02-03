using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000545 RID: 1349
	// (Invoke) Token: 0x0600233C RID: 9020
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryEntitlementTokenCallbackInternal(ref QueryEntitlementTokenCallbackInfoInternal data);
}
