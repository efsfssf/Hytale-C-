using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200022F RID: 559
	// (Invoke) Token: 0x06000FC8 RID: 4040
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnParticipantUpdatedCallbackInternal(ref ParticipantUpdatedCallbackInfoInternal data);
}
