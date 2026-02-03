using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000133 RID: 307
	// (Invoke) Token: 0x0600096B RID: 2411
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnStartSessionCallbackInternal(ref StartSessionCallbackInfoInternal data);
}
