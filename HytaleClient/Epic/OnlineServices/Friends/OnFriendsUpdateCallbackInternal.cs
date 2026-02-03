using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004EA RID: 1258
	// (Invoke) Token: 0x060020A2 RID: 8354
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnFriendsUpdateCallbackInternal(ref OnFriendsUpdateInfoInternal data);
}
