using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x0200060A RID: 1546
	// (Invoke) Token: 0x06002802 RID: 10242
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryExternalAccountMappingsCallbackInternal(ref QueryExternalAccountMappingsCallbackInfoInternal data);
}
