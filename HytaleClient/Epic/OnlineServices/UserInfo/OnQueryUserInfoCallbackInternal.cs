using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x0200003D RID: 61
	// (Invoke) Token: 0x060003E1 RID: 993
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryUserInfoCallbackInternal(ref QueryUserInfoCallbackInfoInternal data);
}
