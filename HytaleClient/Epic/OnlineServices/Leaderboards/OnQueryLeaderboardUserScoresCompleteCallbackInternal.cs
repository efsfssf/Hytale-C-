using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x02000478 RID: 1144
	// (Invoke) Token: 0x06001DF2 RID: 7666
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryLeaderboardUserScoresCompleteCallbackInternal(ref OnQueryLeaderboardUserScoresCompleteCallbackInfoInternal data);
}
