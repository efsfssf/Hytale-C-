using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000547 RID: 1351
	// (Invoke) Token: 0x06002344 RID: 9028
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryOffersCallbackInternal(ref QueryOffersCallbackInfoInternal data);
}
