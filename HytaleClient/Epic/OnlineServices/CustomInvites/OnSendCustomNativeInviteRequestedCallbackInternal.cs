using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005AA RID: 1450
	// (Invoke) Token: 0x0600259E RID: 9630
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnSendCustomNativeInviteRequestedCallbackInternal(ref SendCustomNativeInviteRequestedCallbackInfoInternal data);
}
