using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x02000590 RID: 1424
	// (Invoke) Token: 0x060024FA RID: 9466
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnCustomInviteAcceptedCallbackInternal(ref OnCustomInviteAcceptedCallbackInfoInternal data);
}
