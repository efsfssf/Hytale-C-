using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x0200065C RID: 1628
	// (Invoke) Token: 0x06002A15 RID: 10773
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnVerifyUserAuthCallbackInternal(ref VerifyUserAuthCallbackInfoInternal data);
}
