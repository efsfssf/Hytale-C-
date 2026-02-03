using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x02000604 RID: 1540
	// (Invoke) Token: 0x060027EA RID: 10218
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnLoginCallbackInternal(ref LoginCallbackInfoInternal data);
}
