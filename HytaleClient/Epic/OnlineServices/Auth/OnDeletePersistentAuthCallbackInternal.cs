using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x0200064E RID: 1614
	// (Invoke) Token: 0x060029DD RID: 10717
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnDeletePersistentAuthCallbackInternal(ref DeletePersistentAuthCallbackInfoInternal data);
}
