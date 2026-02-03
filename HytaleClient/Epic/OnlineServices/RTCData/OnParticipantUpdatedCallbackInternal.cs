using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCData
{
	// Token: 0x020001E3 RID: 483
	// (Invoke) Token: 0x06000DF9 RID: 3577
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnParticipantUpdatedCallbackInternal(ref ParticipantUpdatedCallbackInfoInternal data);
}
