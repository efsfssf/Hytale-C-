using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x02000474 RID: 1140
	// (Invoke) Token: 0x06001DD7 RID: 7639
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryLeaderboardRanksCompleteCallbackInternal(ref OnQueryLeaderboardRanksCompleteCallbackInfoInternal data);
}
