using System;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x0200047F RID: 1151
	public struct QueryLeaderboardUserScoresOptions
	{
		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06001E19 RID: 7705 RVA: 0x0002C13E File Offset: 0x0002A33E
		// (set) Token: 0x06001E1A RID: 7706 RVA: 0x0002C146 File Offset: 0x0002A346
		public ProductUserId[] UserIds { get; set; }

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06001E1B RID: 7707 RVA: 0x0002C14F File Offset: 0x0002A34F
		// (set) Token: 0x06001E1C RID: 7708 RVA: 0x0002C157 File Offset: 0x0002A357
		public UserScoresQueryStatInfo[] StatInfo { get; set; }

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x06001E1D RID: 7709 RVA: 0x0002C160 File Offset: 0x0002A360
		// (set) Token: 0x06001E1E RID: 7710 RVA: 0x0002C168 File Offset: 0x0002A368
		public DateTimeOffset? StartTime { get; set; }

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x06001E1F RID: 7711 RVA: 0x0002C171 File Offset: 0x0002A371
		// (set) Token: 0x06001E20 RID: 7712 RVA: 0x0002C179 File Offset: 0x0002A379
		public DateTimeOffset? EndTime { get; set; }

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x06001E21 RID: 7713 RVA: 0x0002C182 File Offset: 0x0002A382
		// (set) Token: 0x06001E22 RID: 7714 RVA: 0x0002C18A File Offset: 0x0002A38A
		public ProductUserId LocalUserId { get; set; }
	}
}
