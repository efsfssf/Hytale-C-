using System;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x0200045F RID: 1119
	public struct CopyLeaderboardUserScoreByUserIdOptions
	{
		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06001D5A RID: 7514 RVA: 0x0002AEDA File Offset: 0x000290DA
		// (set) Token: 0x06001D5B RID: 7515 RVA: 0x0002AEE2 File Offset: 0x000290E2
		public ProductUserId UserId { get; set; }

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x06001D5C RID: 7516 RVA: 0x0002AEEB File Offset: 0x000290EB
		// (set) Token: 0x06001D5D RID: 7517 RVA: 0x0002AEF3 File Offset: 0x000290F3
		public Utf8String StatName { get; set; }
	}
}
