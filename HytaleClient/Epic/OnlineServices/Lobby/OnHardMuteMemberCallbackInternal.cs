using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200040E RID: 1038
	// (Invoke) Token: 0x06001BB9 RID: 7097
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnHardMuteMemberCallbackInternal(ref HardMuteMemberCallbackInfoInternal data);
}
