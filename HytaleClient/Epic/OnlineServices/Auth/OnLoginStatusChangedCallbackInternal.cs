using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000654 RID: 1620
	// (Invoke) Token: 0x060029F5 RID: 10741
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnLoginStatusChangedCallbackInternal(ref LoginStatusChangedCallbackInfoInternal data);
}
