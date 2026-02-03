using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Logging
{
	// Token: 0x0200035E RID: 862
	// (Invoke) Token: 0x0600177C RID: 6012
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void LogMessageFuncInternal(ref LogMessageInternal message);
}
