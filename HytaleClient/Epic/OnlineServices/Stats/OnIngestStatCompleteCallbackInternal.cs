using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Stats
{
	// Token: 0x020000CF RID: 207
	// (Invoke) Token: 0x06000785 RID: 1925
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnIngestStatCompleteCallbackInternal(ref IngestStatCompleteCallbackInfoInternal data);
}
