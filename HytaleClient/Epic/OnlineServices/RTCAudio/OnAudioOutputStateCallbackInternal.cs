using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200022D RID: 557
	// (Invoke) Token: 0x06000FC0 RID: 4032
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnAudioOutputStateCallbackInternal(ref AudioOutputStateCallbackInfoInternal data);
}
