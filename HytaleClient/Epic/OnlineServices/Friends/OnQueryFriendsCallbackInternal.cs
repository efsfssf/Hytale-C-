using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004EE RID: 1262
	// (Invoke) Token: 0x060020C5 RID: 8389
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryFriendsCallbackInternal(ref QueryFriendsCallbackInfoInternal data);
}
