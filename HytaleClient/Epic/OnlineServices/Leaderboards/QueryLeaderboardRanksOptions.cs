using System;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x0200047D RID: 1149
	public struct QueryLeaderboardRanksOptions
	{
		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06001E10 RID: 7696 RVA: 0x0002C06F File Offset: 0x0002A26F
		// (set) Token: 0x06001E11 RID: 7697 RVA: 0x0002C077 File Offset: 0x0002A277
		public Utf8String LeaderboardId { get; set; }

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06001E12 RID: 7698 RVA: 0x0002C080 File Offset: 0x0002A280
		// (set) Token: 0x06001E13 RID: 7699 RVA: 0x0002C088 File Offset: 0x0002A288
		public ProductUserId LocalUserId { get; set; }
	}
}
