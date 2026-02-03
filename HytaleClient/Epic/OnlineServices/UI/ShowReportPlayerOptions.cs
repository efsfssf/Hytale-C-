using System;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000092 RID: 146
	public struct ShowReportPlayerOptions
	{
		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060005E1 RID: 1505 RVA: 0x000083D2 File Offset: 0x000065D2
		// (set) Token: 0x060005E2 RID: 1506 RVA: 0x000083DA File Offset: 0x000065DA
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x000083E3 File Offset: 0x000065E3
		// (set) Token: 0x060005E4 RID: 1508 RVA: 0x000083EB File Offset: 0x000065EB
		public EpicAccountId TargetUserId { get; set; }
	}
}
