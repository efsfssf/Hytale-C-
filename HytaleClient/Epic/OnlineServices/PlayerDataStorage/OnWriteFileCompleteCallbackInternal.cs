using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x02000312 RID: 786
	// (Invoke) Token: 0x0600155D RID: 5469
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnWriteFileCompleteCallbackInternal(ref WriteFileCallbackInfoInternal data);
}
