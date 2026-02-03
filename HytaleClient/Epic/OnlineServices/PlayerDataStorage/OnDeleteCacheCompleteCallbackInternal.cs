using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x02000302 RID: 770
	// (Invoke) Token: 0x0600151D RID: 5405
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnDeleteCacheCompleteCallbackInternal(ref DeleteCacheCallbackInfoInternal data);
}
