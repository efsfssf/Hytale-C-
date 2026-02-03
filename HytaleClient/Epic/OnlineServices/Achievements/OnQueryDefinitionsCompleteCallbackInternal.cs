using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x0200074B RID: 1867
	// (Invoke) Token: 0x0600307B RID: 12411
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryDefinitionsCompleteCallbackInternal(ref OnQueryDefinitionsCompleteCallbackInfoInternal data);
}
