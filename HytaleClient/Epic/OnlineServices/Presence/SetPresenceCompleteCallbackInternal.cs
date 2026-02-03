using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002E7 RID: 743
	// (Invoke) Token: 0x0600146C RID: 5228
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void SetPresenceCompleteCallbackInternal(ref SetPresenceCallbackInfoInternal data);
}
