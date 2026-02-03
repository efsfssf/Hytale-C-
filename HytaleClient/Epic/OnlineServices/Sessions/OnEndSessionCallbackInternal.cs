using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000119 RID: 281
	// (Invoke) Token: 0x0600090B RID: 2315
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnEndSessionCallbackInternal(ref EndSessionCallbackInfoInternal data);
}
