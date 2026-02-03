using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ProgressionSnapshot
{
	// Token: 0x020002B2 RID: 690
	// (Invoke) Token: 0x0600132C RID: 4908
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnSubmitSnapshotCallbackInternal(ref SubmitSnapshotCallbackInfoInternal data);
}
