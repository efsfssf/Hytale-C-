using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x02000306 RID: 774
	// (Invoke) Token: 0x0600152D RID: 5421
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnDuplicateFileCompleteCallbackInternal(ref DuplicateFileCallbackInfoInternal data);
}
