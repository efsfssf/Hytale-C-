using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x0200006F RID: 111
	// (Invoke) Token: 0x060004FC RID: 1276
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnShowBlockPlayerCallbackInternal(ref OnShowBlockPlayerCallbackInfoInternal data);
}
