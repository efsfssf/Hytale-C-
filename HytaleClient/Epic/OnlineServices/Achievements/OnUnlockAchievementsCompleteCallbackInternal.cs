using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000753 RID: 1875
	// (Invoke) Token: 0x060030B1 RID: 12465
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnUnlockAchievementsCompleteCallbackInternal(ref OnUnlockAchievementsCompleteCallbackInfoInternal data);
}
