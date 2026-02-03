using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000077 RID: 119
	// (Invoke) Token: 0x0600052B RID: 1323
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnShowReportPlayerCallbackInternal(ref OnShowReportPlayerCallbackInfoInternal data);
}
