using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x0200006D RID: 109
	// (Invoke) Token: 0x060004F4 RID: 1268
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnMemoryMonitorCallbackInternal(ref MemoryMonitorCallbackInfoInternal data);
}
