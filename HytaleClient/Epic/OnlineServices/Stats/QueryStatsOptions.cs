using System;

namespace Epic.OnlineServices.Stats
{
	// Token: 0x020000D4 RID: 212
	public struct QueryStatsOptions
	{
		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060007A7 RID: 1959 RVA: 0x0000B043 File Offset: 0x00009243
		// (set) Token: 0x060007A8 RID: 1960 RVA: 0x0000B04B File Offset: 0x0000924B
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060007A9 RID: 1961 RVA: 0x0000B054 File Offset: 0x00009254
		// (set) Token: 0x060007AA RID: 1962 RVA: 0x0000B05C File Offset: 0x0000925C
		public DateTimeOffset? StartTime { get; set; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060007AB RID: 1963 RVA: 0x0000B065 File Offset: 0x00009265
		// (set) Token: 0x060007AC RID: 1964 RVA: 0x0000B06D File Offset: 0x0000926D
		public DateTimeOffset? EndTime { get; set; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x0000B076 File Offset: 0x00009276
		// (set) Token: 0x060007AE RID: 1966 RVA: 0x0000B07E File Offset: 0x0000927E
		public Utf8String[] StatNames { get; set; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x0000B087 File Offset: 0x00009287
		// (set) Token: 0x060007B0 RID: 1968 RVA: 0x0000B08F File Offset: 0x0000928F
		public ProductUserId TargetUserId { get; set; }
	}
}
