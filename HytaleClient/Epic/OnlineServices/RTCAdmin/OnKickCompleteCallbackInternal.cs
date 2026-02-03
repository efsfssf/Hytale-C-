using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin
{
	// Token: 0x0200028D RID: 653
	// (Invoke) Token: 0x0600124E RID: 4686
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnKickCompleteCallbackInternal(ref KickCompleteCallbackInfoInternal data);
}
