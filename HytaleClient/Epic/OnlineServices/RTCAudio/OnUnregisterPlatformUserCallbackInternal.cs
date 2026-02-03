using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000245 RID: 581
	// (Invoke) Token: 0x0600104F RID: 4175
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnUnregisterPlatformUserCallbackInternal(ref OnUnregisterPlatformUserCallbackInfoInternal data);
}
