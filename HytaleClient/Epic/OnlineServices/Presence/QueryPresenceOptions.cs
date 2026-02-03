using System;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002E2 RID: 738
	public struct QueryPresenceOptions
	{
		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x0600144B RID: 5195 RVA: 0x0001DAAB File Offset: 0x0001BCAB
		// (set) Token: 0x0600144C RID: 5196 RVA: 0x0001DAB3 File Offset: 0x0001BCB3
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x0600144D RID: 5197 RVA: 0x0001DABC File Offset: 0x0001BCBC
		// (set) Token: 0x0600144E RID: 5198 RVA: 0x0001DAC4 File Offset: 0x0001BCC4
		public EpicAccountId TargetUserId { get; set; }
	}
}
