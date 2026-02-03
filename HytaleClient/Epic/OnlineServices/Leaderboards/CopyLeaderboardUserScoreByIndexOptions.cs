using System;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x0200045D RID: 1117
	public struct CopyLeaderboardUserScoreByIndexOptions
	{
		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06001D51 RID: 7505 RVA: 0x0002AE1D File Offset: 0x0002901D
		// (set) Token: 0x06001D52 RID: 7506 RVA: 0x0002AE25 File Offset: 0x00029025
		public uint LeaderboardUserScoreIndex { get; set; }

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06001D53 RID: 7507 RVA: 0x0002AE2E File Offset: 0x0002902E
		// (set) Token: 0x06001D54 RID: 7508 RVA: 0x0002AE36 File Offset: 0x00029036
		public Utf8String StatName { get; set; }
	}
}
