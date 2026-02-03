using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200022B RID: 555
	// (Invoke) Token: 0x06000FB8 RID: 4024
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnAudioInputStateCallbackInternal(ref AudioInputStateCallbackInfoInternal data);
}
