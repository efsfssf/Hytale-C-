using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x0200078A RID: 1930
	// (Invoke) Token: 0x060031E1 RID: 12769
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnIncomingConnectionRequestCallbackInternal(ref OnIncomingConnectionRequestInfoInternal data);
}
