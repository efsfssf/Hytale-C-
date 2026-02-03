using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000652 RID: 1618
	// (Invoke) Token: 0x060029ED RID: 10733
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnLoginCallbackInternal(ref LoginCallbackInfoInternal data);
}
