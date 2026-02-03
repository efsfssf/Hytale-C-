using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005FA RID: 1530
	// (Invoke) Token: 0x060027C2 RID: 10178
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnAuthExpirationCallbackInternal(ref AuthExpirationCallbackInfoInternal data);
}
