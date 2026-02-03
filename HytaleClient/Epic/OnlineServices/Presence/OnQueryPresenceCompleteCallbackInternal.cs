using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002CF RID: 719
	// (Invoke) Token: 0x060013DE RID: 5086
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryPresenceCompleteCallbackInternal(ref QueryPresenceCallbackInfoInternal data);
}
