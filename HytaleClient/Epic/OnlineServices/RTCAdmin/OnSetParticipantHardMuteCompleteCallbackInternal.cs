using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin
{
	// Token: 0x02000291 RID: 657
	// (Invoke) Token: 0x0600125E RID: 4702
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnSetParticipantHardMuteCompleteCallbackInternal(ref SetParticipantHardMuteCompleteCallbackInfoInternal data);
}
