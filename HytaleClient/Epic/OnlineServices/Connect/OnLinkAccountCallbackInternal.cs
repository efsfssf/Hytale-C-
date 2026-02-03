using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x02000602 RID: 1538
	// (Invoke) Token: 0x060027E2 RID: 10210
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnLinkAccountCallbackInternal(ref LinkAccountCallbackInfoInternal data);
}
