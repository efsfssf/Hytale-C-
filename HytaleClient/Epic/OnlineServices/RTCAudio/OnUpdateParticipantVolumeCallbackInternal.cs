using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000249 RID: 585
	// (Invoke) Token: 0x0600106A RID: 4202
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnUpdateParticipantVolumeCallbackInternal(ref UpdateParticipantVolumeCallbackInfoInternal data);
}
