using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x02000600 RID: 1536
	// (Invoke) Token: 0x060027DA RID: 10202
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnDeleteDeviceIdCallbackInternal(ref DeleteDeviceIdCallbackInfoInternal data);
}
