using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200042C RID: 1068
	// (Invoke) Token: 0x06001C31 RID: 7217
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnPromoteMemberCallbackInternal(ref PromoteMemberCallbackInfoInternal data);
}
