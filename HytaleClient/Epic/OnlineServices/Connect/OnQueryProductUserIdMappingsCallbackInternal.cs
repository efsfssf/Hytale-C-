using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x0200060C RID: 1548
	// (Invoke) Token: 0x0600280A RID: 10250
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryProductUserIdMappingsCallbackInternal(ref QueryProductUserIdMappingsCallbackInfoInternal data);
}
