using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000137 RID: 311
	// (Invoke) Token: 0x0600097B RID: 2427
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnUpdateSessionCallbackInternal(ref UpdateSessionCallbackInfoInternal data);
}
