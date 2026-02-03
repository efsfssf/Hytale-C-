using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000549 RID: 1353
	// (Invoke) Token: 0x0600234C RID: 9036
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryOwnershipBySandboxIdsCallbackInternal(ref QueryOwnershipBySandboxIdsCallbackInfoInternal data);
}
