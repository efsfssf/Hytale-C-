using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000541 RID: 1345
	// (Invoke) Token: 0x0600232C RID: 9004
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnCheckoutCallbackInternal(ref CheckoutCallbackInfoInternal data);
}
