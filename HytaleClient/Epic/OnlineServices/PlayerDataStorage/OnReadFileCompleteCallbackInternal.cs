using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x0200030E RID: 782
	// (Invoke) Token: 0x0600154D RID: 5453
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnReadFileCompleteCallbackInternal(ref ReadFileCallbackInfoInternal data);
}
