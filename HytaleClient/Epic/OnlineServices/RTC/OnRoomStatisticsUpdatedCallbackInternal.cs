using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001CB RID: 459
	// (Invoke) Token: 0x06000D3B RID: 3387
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnRoomStatisticsUpdatedCallbackInternal(ref RoomStatisticsUpdatedInfoInternal data);
}
