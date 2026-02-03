using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x02000314 RID: 788
	// (Invoke) Token: 0x06001565 RID: 5477
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate WriteResult OnWriteFileDataCallbackInternal(ref WriteFileDataCallbackInfoInternal data, IntPtr outDataBuffer, ref uint outDataWritten);
}
