using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x0200049A RID: 1178
	// (Invoke) Token: 0x06001EB1 RID: 7857
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnRequestPermissionsCallbackInternal(ref RequestPermissionsCallbackInfoInternal data);
}
