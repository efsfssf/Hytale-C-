using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200011D RID: 285
	// (Invoke) Token: 0x0600091B RID: 2331
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnJoinSessionCallbackInternal(ref JoinSessionCallbackInfoInternal data);
}
