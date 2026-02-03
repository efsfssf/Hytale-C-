using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000543 RID: 1347
	// (Invoke) Token: 0x06002334 RID: 9012
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryEntitlementsCallbackInternal(ref QueryEntitlementsCallbackInfoInternal data);
}
