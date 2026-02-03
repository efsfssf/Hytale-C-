using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x02000342 RID: 834
	// (Invoke) Token: 0x060016D7 RID: 5847
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnUninstallModCallbackInternal(ref UninstallModCallbackInfoInternal data);
}
