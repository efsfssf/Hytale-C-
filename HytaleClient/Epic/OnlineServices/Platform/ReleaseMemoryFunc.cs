using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x02000719 RID: 1817
	// (Invoke) Token: 0x06002F16 RID: 12054
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void ReleaseMemoryFunc(IntPtr pointer);
}
