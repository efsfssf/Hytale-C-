using System;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x02000461 RID: 1121
	public struct Definition
	{
		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x06001D63 RID: 7523 RVA: 0x0002AFA6 File Offset: 0x000291A6
		// (set) Token: 0x06001D64 RID: 7524 RVA: 0x0002AFAE File Offset: 0x000291AE
		public Utf8String LeaderboardId { get; set; }

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x06001D65 RID: 7525 RVA: 0x0002AFB7 File Offset: 0x000291B7
		// (set) Token: 0x06001D66 RID: 7526 RVA: 0x0002AFBF File Offset: 0x000291BF
		public Utf8String StatName { get; set; }

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06001D67 RID: 7527 RVA: 0x0002AFC8 File Offset: 0x000291C8
		// (set) Token: 0x06001D68 RID: 7528 RVA: 0x0002AFD0 File Offset: 0x000291D0
		public LeaderboardAggregation Aggregation { get; set; }

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06001D69 RID: 7529 RVA: 0x0002AFD9 File Offset: 0x000291D9
		// (set) Token: 0x06001D6A RID: 7530 RVA: 0x0002AFE1 File Offset: 0x000291E1
		public DateTimeOffset? StartTime { get; set; }

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06001D6B RID: 7531 RVA: 0x0002AFEA File Offset: 0x000291EA
		// (set) Token: 0x06001D6C RID: 7532 RVA: 0x0002AFF2 File Offset: 0x000291F2
		public DateTimeOffset? EndTime { get; set; }

		// Token: 0x06001D6D RID: 7533 RVA: 0x0002AFFC File Offset: 0x000291FC
		internal void Set(ref DefinitionInternal other)
		{
			this.LeaderboardId = other.LeaderboardId;
			this.StatName = other.StatName;
			this.Aggregation = other.Aggregation;
			this.StartTime = other.StartTime;
			this.EndTime = other.EndTime;
		}
	}
}
