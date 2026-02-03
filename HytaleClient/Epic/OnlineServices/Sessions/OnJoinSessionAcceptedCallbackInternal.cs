using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200011B RID: 283
	// (Invoke) Token: 0x06000913 RID: 2323
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnJoinSessionAcceptedCallbackInternal(ref JoinSessionAcceptedCallbackInfoInternal data);
}
