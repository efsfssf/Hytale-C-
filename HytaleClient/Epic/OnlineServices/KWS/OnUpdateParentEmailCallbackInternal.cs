using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x0200049C RID: 1180
	// (Invoke) Token: 0x06001EB9 RID: 7865
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnUpdateParentEmailCallbackInternal(ref UpdateParentEmailCallbackInfoInternal data);
}
