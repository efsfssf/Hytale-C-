using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Reports
{
	// Token: 0x0200029E RID: 670
	// (Invoke) Token: 0x060012C6 RID: 4806
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnSendPlayerBehaviorReportCompleteCallbackInternal(ref SendPlayerBehaviorReportCompleteCallbackInfoInternal data);
}
