using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x02000308 RID: 776
	// (Invoke) Token: 0x06001535 RID: 5429
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnFileTransferProgressCallbackInternal(ref FileTransferProgressCallbackInfoInternal data);
}
