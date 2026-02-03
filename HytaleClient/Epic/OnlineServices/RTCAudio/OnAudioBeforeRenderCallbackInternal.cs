using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000225 RID: 549
	// (Invoke) Token: 0x06000FA0 RID: 4000
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnAudioBeforeRenderCallbackInternal(ref AudioBeforeRenderCallbackInfoInternal data);
}
