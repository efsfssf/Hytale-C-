using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005FE RID: 1534
	// (Invoke) Token: 0x060027D2 RID: 10194
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnCreateUserCallbackInternal(ref CreateUserCallbackInfoInternal data);
}
