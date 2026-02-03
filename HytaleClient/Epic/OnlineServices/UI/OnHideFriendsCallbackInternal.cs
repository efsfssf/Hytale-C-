using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x0200006B RID: 107
	// (Invoke) Token: 0x060004EC RID: 1260
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnHideFriendsCallbackInternal(ref HideFriendsCallbackInfoInternal data);
}
