using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004E4 RID: 1252
	// (Invoke) Token: 0x0600207B RID: 8315
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnAcceptInviteCallbackInternal(ref AcceptInviteCallbackInfoInternal data);
}
