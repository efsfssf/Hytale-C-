using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000127 RID: 295
	// (Invoke) Token: 0x0600093B RID: 2363
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnRejectInviteCallbackInternal(ref RejectInviteCallbackInfoInternal data);
}
