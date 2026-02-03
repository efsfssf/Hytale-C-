using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000796 RID: 1942
	// (Invoke) Token: 0x0600324E RID: 12878
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnPeerConnectionInterruptedCallbackInternal(ref OnPeerConnectionInterruptedInfoInternal data);
}
