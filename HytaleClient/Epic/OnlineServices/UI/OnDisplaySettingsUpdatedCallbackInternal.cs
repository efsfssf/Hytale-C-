using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000067 RID: 103
	// (Invoke) Token: 0x060004D1 RID: 1233
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnDisplaySettingsUpdatedCallbackInternal(ref OnDisplaySettingsUpdatedCallbackInfoInternal data);
}
