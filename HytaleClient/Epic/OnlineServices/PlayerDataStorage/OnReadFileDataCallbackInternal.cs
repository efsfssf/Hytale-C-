using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x02000310 RID: 784
	// (Invoke) Token: 0x06001555 RID: 5461
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate ReadResult OnReadFileDataCallbackInternal(ref ReadFileDataCallbackInfoInternal data);
}
