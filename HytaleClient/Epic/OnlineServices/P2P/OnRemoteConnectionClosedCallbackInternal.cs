using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x0200079E RID: 1950
	// (Invoke) Token: 0x06003288 RID: 12936
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnRemoteConnectionClosedCallbackInternal(ref OnRemoteConnectionClosedInfoInternal data);
}
