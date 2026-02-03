using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002CD RID: 717
	// (Invoke) Token: 0x060013D6 RID: 5078
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnPresenceChangedCallbackInternal(ref PresenceChangedCallbackInfoInternal data);
}
