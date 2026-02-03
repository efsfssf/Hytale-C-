using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ProgressionSnapshot
{
	// Token: 0x020002B0 RID: 688
	// (Invoke) Token: 0x06001324 RID: 4900
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnDeleteSnapshotCallbackInternal(ref DeleteSnapshotCallbackInfoInternal data);
}
