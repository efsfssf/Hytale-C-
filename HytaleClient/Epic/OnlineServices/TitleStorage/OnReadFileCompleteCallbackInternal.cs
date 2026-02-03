using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x020000AE RID: 174
	// (Invoke) Token: 0x06000694 RID: 1684
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnReadFileCompleteCallbackInternal(ref ReadFileCallbackInfoInternal data);
}
