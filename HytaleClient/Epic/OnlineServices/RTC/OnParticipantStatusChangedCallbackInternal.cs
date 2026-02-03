using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001C9 RID: 457
	// (Invoke) Token: 0x06000D33 RID: 3379
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnParticipantStatusChangedCallbackInternal(ref ParticipantStatusChangedCallbackInfoInternal data);
}
