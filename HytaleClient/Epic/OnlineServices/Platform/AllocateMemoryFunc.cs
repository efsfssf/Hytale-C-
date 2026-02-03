using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x02000707 RID: 1799
	// (Invoke) Token: 0x06002E7E RID: 11902
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate IntPtr AllocateMemoryFunc(UIntPtr sizeInBytes, UIntPtr alignment);
}
