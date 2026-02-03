using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000231 RID: 561
	// (Invoke) Token: 0x06000FD0 RID: 4048
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryInputDevicesInformationCallbackInternal(ref OnQueryInputDevicesInformationCallbackInfoInternal data);
}
