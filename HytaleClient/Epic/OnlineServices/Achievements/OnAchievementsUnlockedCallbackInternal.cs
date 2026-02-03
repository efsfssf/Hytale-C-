using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000743 RID: 1859
	// (Invoke) Token: 0x06003041 RID: 12353
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnAchievementsUnlockedCallbackInternal(ref OnAchievementsUnlockedCallbackInfoInternal data);
}
