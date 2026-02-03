using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x02000610 RID: 1552
	// (Invoke) Token: 0x0600281A RID: 10266
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnUnlinkAccountCallbackInternal(ref UnlinkAccountCallbackInfoInternal data);
}
