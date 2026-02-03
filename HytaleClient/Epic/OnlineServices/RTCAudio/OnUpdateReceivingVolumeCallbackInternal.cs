using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200024D RID: 589
	// (Invoke) Token: 0x0600107A RID: 4218
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnUpdateReceivingVolumeCallbackInternal(ref UpdateReceivingVolumeCallbackInfoInternal data);
}
