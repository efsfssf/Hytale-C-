using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005A8 RID: 1448
	// (Invoke) Token: 0x06002596 RID: 9622
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnSendCustomInviteCallbackInternal(ref SendCustomInviteCallbackInfoInternal data);
}
