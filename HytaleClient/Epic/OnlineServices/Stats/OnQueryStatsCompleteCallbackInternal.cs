using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Stats
{
	// Token: 0x020000D1 RID: 209
	// (Invoke) Token: 0x0600078D RID: 1933
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryStatsCompleteCallbackInternal(ref OnQueryStatsCompleteCallbackInfoInternal data);
}
