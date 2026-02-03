using System;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x0200046D RID: 1133
	public struct LeaderboardUserScore
	{
		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06001DAE RID: 7598 RVA: 0x0002B9CB File Offset: 0x00029BCB
		// (set) Token: 0x06001DAF RID: 7599 RVA: 0x0002B9D3 File Offset: 0x00029BD3
		public ProductUserId UserId { get; set; }

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x06001DB0 RID: 7600 RVA: 0x0002B9DC File Offset: 0x00029BDC
		// (set) Token: 0x06001DB1 RID: 7601 RVA: 0x0002B9E4 File Offset: 0x00029BE4
		public int Score { get; set; }

		// Token: 0x06001DB2 RID: 7602 RVA: 0x0002B9ED File Offset: 0x00029BED
		internal void Set(ref LeaderboardUserScoreInternal other)
		{
			this.UserId = other.UserId;
			this.Score = other.Score;
		}
	}
}
