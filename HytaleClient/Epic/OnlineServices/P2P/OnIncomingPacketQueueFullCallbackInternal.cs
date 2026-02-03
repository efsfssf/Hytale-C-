using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x0200078E RID: 1934
	// (Invoke) Token: 0x06003200 RID: 12800
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnIncomingPacketQueueFullCallbackInternal(ref OnIncomingPacketQueueFullInfoInternal data);
}
