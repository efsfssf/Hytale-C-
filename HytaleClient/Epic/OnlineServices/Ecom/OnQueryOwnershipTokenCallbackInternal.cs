using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200054D RID: 1357
	// (Invoke) Token: 0x0600235C RID: 9052
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryOwnershipTokenCallbackInternal(ref QueryOwnershipTokenCallbackInfoInternal data);
}
