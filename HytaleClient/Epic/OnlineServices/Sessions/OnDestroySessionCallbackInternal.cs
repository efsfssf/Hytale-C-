using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000117 RID: 279
	// (Invoke) Token: 0x06000903 RID: 2307
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnDestroySessionCallbackInternal(ref DestroySessionCallbackInfoInternal data);
}
