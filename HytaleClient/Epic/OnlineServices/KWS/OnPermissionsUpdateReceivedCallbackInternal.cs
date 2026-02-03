using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x02000494 RID: 1172
	// (Invoke) Token: 0x06001E99 RID: 7833
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnPermissionsUpdateReceivedCallbackInternal(ref PermissionsUpdateReceivedCallbackInfoInternal data);
}
