using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x020000AC RID: 172
	// (Invoke) Token: 0x0600068C RID: 1676
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryFileListCompleteCallbackInternal(ref QueryFileListCallbackInfoInternal data);
}
