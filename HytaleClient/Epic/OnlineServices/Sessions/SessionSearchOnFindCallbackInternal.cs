using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200017D RID: 381
	// (Invoke) Token: 0x06000B2E RID: 2862
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void SessionSearchOnFindCallbackInternal(ref SessionSearchFindCallbackInfoInternal data);
}
