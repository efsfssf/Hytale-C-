using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x02000594 RID: 1428
	// (Invoke) Token: 0x0600251D RID: 9501
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnCustomInviteReceivedCallbackInternal(ref OnCustomInviteReceivedCallbackInfoInternal data);
}
