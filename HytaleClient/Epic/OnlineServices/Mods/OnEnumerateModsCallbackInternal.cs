using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x0200033E RID: 830
	// (Invoke) Token: 0x060016C7 RID: 5831
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnEnumerateModsCallbackInternal(ref EnumerateModsCallbackInfoInternal data);
}
