using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x02000598 RID: 1432
	// (Invoke) Token: 0x06002540 RID: 9536
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnCustomInviteRejectedCallbackInternal(ref CustomInviteRejectedCallbackInfoInternal data);
}
