using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000792 RID: 1938
	// (Invoke) Token: 0x06003227 RID: 12839
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnPeerConnectionEstablishedCallbackInternal(ref OnPeerConnectionEstablishedInfoInternal data);
}
