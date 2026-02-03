using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x02000470 RID: 1136
	// (Invoke) Token: 0x06001DC0 RID: 7616
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryLeaderboardDefinitionsCompleteCallbackInternal(ref OnQueryLeaderboardDefinitionsCompleteCallbackInfoInternal data);
}
