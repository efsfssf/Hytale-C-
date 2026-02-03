using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x02000492 RID: 1170
	// (Invoke) Token: 0x06001E91 RID: 7825
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnCreateUserCallbackInternal(ref CreateUserCallbackInfoInternal data);
}
