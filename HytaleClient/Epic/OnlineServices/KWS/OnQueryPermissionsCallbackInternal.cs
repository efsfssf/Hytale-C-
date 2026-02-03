using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x02000498 RID: 1176
	// (Invoke) Token: 0x06001EA9 RID: 7849
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryPermissionsCallbackInternal(ref QueryPermissionsCallbackInfoInternal data);
}
