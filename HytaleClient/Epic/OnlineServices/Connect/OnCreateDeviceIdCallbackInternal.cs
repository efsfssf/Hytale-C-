using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005FC RID: 1532
	// (Invoke) Token: 0x060027CA RID: 10186
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnCreateDeviceIdCallbackInternal(ref CreateDeviceIdCallbackInfoInternal data);
}
