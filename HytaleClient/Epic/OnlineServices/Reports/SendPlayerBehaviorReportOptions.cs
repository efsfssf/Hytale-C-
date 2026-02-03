using System;

namespace Epic.OnlineServices.Reports
{
	// Token: 0x020002A3 RID: 675
	public struct SendPlayerBehaviorReportOptions
	{
		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x060012DC RID: 4828 RVA: 0x0001B7D9 File Offset: 0x000199D9
		// (set) Token: 0x060012DD RID: 4829 RVA: 0x0001B7E1 File Offset: 0x000199E1
		public ProductUserId ReporterUserId { get; set; }

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x060012DE RID: 4830 RVA: 0x0001B7EA File Offset: 0x000199EA
		// (set) Token: 0x060012DF RID: 4831 RVA: 0x0001B7F2 File Offset: 0x000199F2
		public ProductUserId ReportedUserId { get; set; }

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x060012E0 RID: 4832 RVA: 0x0001B7FB File Offset: 0x000199FB
		// (set) Token: 0x060012E1 RID: 4833 RVA: 0x0001B803 File Offset: 0x00019A03
		public PlayerReportsCategory Category { get; set; }

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x060012E2 RID: 4834 RVA: 0x0001B80C File Offset: 0x00019A0C
		// (set) Token: 0x060012E3 RID: 4835 RVA: 0x0001B814 File Offset: 0x00019A14
		public Utf8String Message { get; set; }

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x060012E4 RID: 4836 RVA: 0x0001B81D File Offset: 0x00019A1D
		// (set) Token: 0x060012E5 RID: 4837 RVA: 0x0001B825 File Offset: 0x00019A25
		public Utf8String Context { get; set; }
	}
}
