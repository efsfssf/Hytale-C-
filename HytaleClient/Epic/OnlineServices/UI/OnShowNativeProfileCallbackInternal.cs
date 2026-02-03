using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000075 RID: 117
	// (Invoke) Token: 0x06000523 RID: 1315
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnShowNativeProfileCallbackInternal(ref ShowNativeProfileCallbackInfoInternal data);
}
