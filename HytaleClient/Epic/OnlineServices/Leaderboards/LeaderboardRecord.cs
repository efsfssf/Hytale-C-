using System;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x0200046A RID: 1130
	public struct LeaderboardRecord
	{
		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06001D88 RID: 7560 RVA: 0x0002B32D File Offset: 0x0002952D
		// (set) Token: 0x06001D89 RID: 7561 RVA: 0x0002B335 File Offset: 0x00029535
		public ProductUserId UserId { get; set; }

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06001D8A RID: 7562 RVA: 0x0002B33E File Offset: 0x0002953E
		// (set) Token: 0x06001D8B RID: 7563 RVA: 0x0002B346 File Offset: 0x00029546
		public uint Rank { get; set; }

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06001D8C RID: 7564 RVA: 0x0002B34F File Offset: 0x0002954F
		// (set) Token: 0x06001D8D RID: 7565 RVA: 0x0002B357 File Offset: 0x00029557
		public int Score { get; set; }

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06001D8E RID: 7566 RVA: 0x0002B360 File Offset: 0x00029560
		// (set) Token: 0x06001D8F RID: 7567 RVA: 0x0002B368 File Offset: 0x00029568
		public Utf8String UserDisplayName { get; set; }

		// Token: 0x06001D90 RID: 7568 RVA: 0x0002B371 File Offset: 0x00029571
		internal void Set(ref LeaderboardRecordInternal other)
		{
			this.UserId = other.UserId;
			this.Rank = other.Rank;
			this.Score = other.Score;
			this.UserDisplayName = other.UserDisplayName;
		}
	}
}
