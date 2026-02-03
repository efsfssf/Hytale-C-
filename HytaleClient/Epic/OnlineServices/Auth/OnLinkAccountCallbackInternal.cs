using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000650 RID: 1616
	// (Invoke) Token: 0x060029E5 RID: 10725
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnLinkAccountCallbackInternal(ref LinkAccountCallbackInfoInternal data);
}
