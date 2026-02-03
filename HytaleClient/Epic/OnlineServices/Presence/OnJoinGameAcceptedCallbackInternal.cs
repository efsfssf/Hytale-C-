using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002CB RID: 715
	// (Invoke) Token: 0x060013CE RID: 5070
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnJoinGameAcceptedCallbackInternal(ref JoinGameAcceptedCallbackInfoInternal data);
}
