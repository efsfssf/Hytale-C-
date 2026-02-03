using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x0200060E RID: 1550
	// (Invoke) Token: 0x06002812 RID: 10258
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnTransferDeviceIdAccountCallbackInternal(ref TransferDeviceIdAccountCallbackInfoInternal data);
}
