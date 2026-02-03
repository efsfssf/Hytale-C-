using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x020000A8 RID: 168
	// (Invoke) Token: 0x0600067C RID: 1660
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnFileTransferProgressCallbackInternal(ref FileTransferProgressCallbackInfoInternal data);
}
