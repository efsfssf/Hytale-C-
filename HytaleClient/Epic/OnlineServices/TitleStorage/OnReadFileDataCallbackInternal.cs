using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x020000B0 RID: 176
	// (Invoke) Token: 0x0600069C RID: 1692
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ReadResult OnReadFileDataCallbackInternal(ref ReadFileDataCallbackInfoInternal data);
}
