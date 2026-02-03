using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000229 RID: 553
	// (Invoke) Token: 0x06000FB0 RID: 4016
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnAudioDevicesChangedCallbackInternal(ref AudioDevicesChangedCallbackInfoInternal data);
}
