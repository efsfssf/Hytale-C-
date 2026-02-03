using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001C1 RID: 449
	// (Invoke) Token: 0x06000D13 RID: 3347
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnBlockParticipantCallbackInternal(ref BlockParticipantCallbackInfoInternal data);
}
