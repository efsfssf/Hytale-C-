using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200042E RID: 1070
	// (Invoke) Token: 0x06001C39 RID: 7225
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryInvitesCallbackInternal(ref QueryInvitesCallbackInfoInternal data);
}
