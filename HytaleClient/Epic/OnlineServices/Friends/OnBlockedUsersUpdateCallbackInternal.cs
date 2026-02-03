using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004E6 RID: 1254
	// (Invoke) Token: 0x06002083 RID: 8323
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnBlockedUsersUpdateCallbackInternal(ref OnBlockedUsersUpdateInfoInternal data);
}
