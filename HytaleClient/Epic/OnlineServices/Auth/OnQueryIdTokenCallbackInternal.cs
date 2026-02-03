using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000658 RID: 1624
	// (Invoke) Token: 0x06002A05 RID: 10757
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryIdTokenCallbackInternal(ref QueryIdTokenCallbackInfoInternal data);
}
