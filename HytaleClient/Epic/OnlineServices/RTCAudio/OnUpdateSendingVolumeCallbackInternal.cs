using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000251 RID: 593
	// (Invoke) Token: 0x0600108A RID: 4234
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnUpdateSendingVolumeCallbackInternal(ref UpdateSendingVolumeCallbackInfoInternal data);
}
