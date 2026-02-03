using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000747 RID: 1863
	// (Invoke) Token: 0x0600305C RID: 12380
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnAchievementsUnlockedCallbackV2Internal(ref OnAchievementsUnlockedCallbackV2InfoInternal data);
}
