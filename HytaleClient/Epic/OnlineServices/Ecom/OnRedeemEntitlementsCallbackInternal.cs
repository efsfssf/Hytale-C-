using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200054F RID: 1359
	// (Invoke) Token: 0x06002364 RID: 9060
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnRedeemEntitlementsCallbackInternal(ref RedeemEntitlementsCallbackInfoInternal data);
}
