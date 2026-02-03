using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x02000718 RID: 1816
	// (Invoke) Token: 0x06002F12 RID: 12050
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate IntPtr ReallocateMemoryFunc(IntPtr pointer, UIntPtr sizeInBytes, UIntPtr alignment);
}
