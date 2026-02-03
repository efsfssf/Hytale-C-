using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000241 RID: 577
	// (Invoke) Token: 0x06001034 RID: 4148
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnSetOutputDeviceSettingsCallbackInternal(ref OnSetOutputDeviceSettingsCallbackInfoInternal data);
}
