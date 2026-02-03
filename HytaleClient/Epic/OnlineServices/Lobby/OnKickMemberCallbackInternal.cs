using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000418 RID: 1048
	// (Invoke) Token: 0x06001BE1 RID: 7137
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnKickMemberCallbackInternal(ref KickMemberCallbackInfoInternal data);
}
