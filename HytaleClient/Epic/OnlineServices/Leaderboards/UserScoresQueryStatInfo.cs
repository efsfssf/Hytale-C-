using System;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x02000481 RID: 1153
	public struct UserScoresQueryStatInfo
	{
		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x06001E2B RID: 7723 RVA: 0x0002C2F9 File Offset: 0x0002A4F9
		// (set) Token: 0x06001E2C RID: 7724 RVA: 0x0002C301 File Offset: 0x0002A501
		public Utf8String StatName { get; set; }

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x06001E2D RID: 7725 RVA: 0x0002C30A File Offset: 0x0002A50A
		// (set) Token: 0x06001E2E RID: 7726 RVA: 0x0002C312 File Offset: 0x0002A512
		public LeaderboardAggregation Aggregation { get; set; }

		// Token: 0x06001E2F RID: 7727 RVA: 0x0002C31B File Offset: 0x0002A51B
		internal void Set(ref UserScoresQueryStatInfoInternal other)
		{
			this.StatName = other.StatName;
			this.Aggregation = other.Aggregation;
		}
	}
}
