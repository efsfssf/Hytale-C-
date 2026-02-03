using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000146 RID: 326
	public struct SendInviteOptions
	{
		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060009DE RID: 2526 RVA: 0x0000DB25 File Offset: 0x0000BD25
		// (set) Token: 0x060009DF RID: 2527 RVA: 0x0000DB2D File Offset: 0x0000BD2D
		public Utf8String SessionName { get; set; }

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060009E0 RID: 2528 RVA: 0x0000DB36 File Offset: 0x0000BD36
		// (set) Token: 0x060009E1 RID: 2529 RVA: 0x0000DB3E File Offset: 0x0000BD3E
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060009E2 RID: 2530 RVA: 0x0000DB47 File Offset: 0x0000BD47
		// (set) Token: 0x060009E3 RID: 2531 RVA: 0x0000DB4F File Offset: 0x0000BD4F
		public ProductUserId TargetUserId { get; set; }
	}
}
