using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001C3 RID: 451
	// (Invoke) Token: 0x06000D1B RID: 3355
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnDisconnectedCallbackInternal(ref DisconnectedCallbackInfoInternal data);
}
