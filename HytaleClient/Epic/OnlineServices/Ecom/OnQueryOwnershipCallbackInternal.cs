using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200054B RID: 1355
	// (Invoke) Token: 0x06002354 RID: 9044
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryOwnershipCallbackInternal(ref QueryOwnershipCallbackInfoInternal data);
}
