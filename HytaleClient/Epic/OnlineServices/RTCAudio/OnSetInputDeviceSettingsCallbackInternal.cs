using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200023D RID: 573
	// (Invoke) Token: 0x06001019 RID: 4121
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnSetInputDeviceSettingsCallbackInternal(ref OnSetInputDeviceSettingsCallbackInfoInternal data);
}
