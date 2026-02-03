using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x0200079A RID: 1946
	// (Invoke) Token: 0x0600326D RID: 12909
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryNATTypeCompleteCallbackInternal(ref OnQueryNATTypeCompleteInfoInternal data);
}
