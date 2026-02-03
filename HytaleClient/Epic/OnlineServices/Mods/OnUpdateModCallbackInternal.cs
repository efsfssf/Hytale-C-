using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x02000344 RID: 836
	// (Invoke) Token: 0x060016DF RID: 5855
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnUpdateModCallbackInternal(ref UpdateModCallbackInfoInternal data);
}
