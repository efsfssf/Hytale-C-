using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x02000039 RID: 57
	// (Invoke) Token: 0x060003D1 RID: 977
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryUserInfoByDisplayNameCallbackInternal(ref QueryUserInfoByDisplayNameCallbackInfoInternal data);
}
