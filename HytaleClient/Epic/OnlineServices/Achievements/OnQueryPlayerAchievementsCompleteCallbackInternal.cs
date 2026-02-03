using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x0200074F RID: 1871
	// (Invoke) Token: 0x06003092 RID: 12434
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnQueryPlayerAchievementsCompleteCallbackInternal(ref OnQueryPlayerAchievementsCompleteCallbackInfoInternal data);
}
