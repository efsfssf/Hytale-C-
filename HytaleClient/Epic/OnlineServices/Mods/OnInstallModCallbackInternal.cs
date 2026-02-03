using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x02000340 RID: 832
	// (Invoke) Token: 0x060016CF RID: 5839
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnInstallModCallbackInternal(ref InstallModCallbackInfoInternal data);
}
