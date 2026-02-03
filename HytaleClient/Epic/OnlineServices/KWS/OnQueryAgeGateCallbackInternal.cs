using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x02000496 RID: 1174
	// (Invoke) Token: 0x06001EA1 RID: 7841
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryAgeGateCallbackInternal(ref QueryAgeGateCallbackInfoInternal data);
}
