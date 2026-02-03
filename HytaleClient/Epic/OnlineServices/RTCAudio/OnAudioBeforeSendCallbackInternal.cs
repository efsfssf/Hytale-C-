using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000227 RID: 551
	// (Invoke) Token: 0x06000FA8 RID: 4008
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnAudioBeforeSendCallbackInternal(ref AudioBeforeSendCallbackInfoInternal data);
}
