using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000073 RID: 115
	// (Invoke) Token: 0x0600051B RID: 1307
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnShowFriendsCallbackInternal(ref ShowFriendsCallbackInfoInternal data);
}
