using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000235 RID: 565
	// (Invoke) Token: 0x06000FE7 RID: 4071
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryOutputDevicesInformationCallbackInternal(ref OnQueryOutputDevicesInformationCallbackInfoInternal data);
}
