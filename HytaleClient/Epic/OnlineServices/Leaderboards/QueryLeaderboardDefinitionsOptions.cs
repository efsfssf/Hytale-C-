using System;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x0200047B RID: 1147
	public struct QueryLeaderboardDefinitionsOptions
	{
		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06001E04 RID: 7684 RVA: 0x0002BF69 File Offset: 0x0002A169
		// (set) Token: 0x06001E05 RID: 7685 RVA: 0x0002BF71 File Offset: 0x0002A171
		public DateTimeOffset? StartTime { get; set; }

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06001E06 RID: 7686 RVA: 0x0002BF7A File Offset: 0x0002A17A
		// (set) Token: 0x06001E07 RID: 7687 RVA: 0x0002BF82 File Offset: 0x0002A182
		public DateTimeOffset? EndTime { get; set; }

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06001E08 RID: 7688 RVA: 0x0002BF8B File Offset: 0x0002A18B
		// (set) Token: 0x06001E09 RID: 7689 RVA: 0x0002BF93 File Offset: 0x0002A193
		public ProductUserId LocalUserId { get; set; }
	}
}
