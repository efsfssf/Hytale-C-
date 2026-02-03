using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x02000304 RID: 772
	// (Invoke) Token: 0x06001525 RID: 5413
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnDeleteFileCompleteCallbackInternal(ref DeleteFileCallbackInfoInternal data);
}
