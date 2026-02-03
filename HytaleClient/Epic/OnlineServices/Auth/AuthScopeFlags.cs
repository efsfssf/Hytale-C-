using System;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x0200062E RID: 1582
	[Flags]
	public enum AuthScopeFlags
	{
		// Token: 0x0400119F RID: 4511
		NoFlags = 0,
		// Token: 0x040011A0 RID: 4512
		BasicProfile = 1,
		// Token: 0x040011A1 RID: 4513
		FriendsList = 2,
		// Token: 0x040011A2 RID: 4514
		Presence = 4,
		// Token: 0x040011A3 RID: 4515
		FriendsManagement = 8,
		// Token: 0x040011A4 RID: 4516
		Email = 16,
		// Token: 0x040011A5 RID: 4517
		Country = 32
	}
}
