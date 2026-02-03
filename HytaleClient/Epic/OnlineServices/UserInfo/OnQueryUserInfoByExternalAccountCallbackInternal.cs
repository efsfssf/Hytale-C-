using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x0200003B RID: 59
	// (Invoke) Token: 0x060003D9 RID: 985
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryUserInfoByExternalAccountCallbackInternal(ref QueryUserInfoByExternalAccountCallbackInfoInternal data);
}
