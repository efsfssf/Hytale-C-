using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200024B RID: 587
	// (Invoke) Token: 0x06001072 RID: 4210
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnUpdateReceivingCallbackInternal(ref UpdateReceivingCallbackInfoInternal data);
}
