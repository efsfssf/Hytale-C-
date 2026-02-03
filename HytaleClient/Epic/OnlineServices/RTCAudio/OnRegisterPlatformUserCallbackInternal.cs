using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000239 RID: 569
	// (Invoke) Token: 0x06000FFE RID: 4094
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnRegisterPlatformUserCallbackInternal(ref OnRegisterPlatformUserCallbackInfoInternal data);
}
