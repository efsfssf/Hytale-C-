using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000553 RID: 1363
	public struct QueryEntitlementsOptions
	{
		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x0600237A RID: 9082 RVA: 0x000342B6 File Offset: 0x000324B6
		// (set) Token: 0x0600237B RID: 9083 RVA: 0x000342BE File Offset: 0x000324BE
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x0600237C RID: 9084 RVA: 0x000342C7 File Offset: 0x000324C7
		// (set) Token: 0x0600237D RID: 9085 RVA: 0x000342CF File Offset: 0x000324CF
		public Utf8String[] EntitlementNames { get; set; }

		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x0600237E RID: 9086 RVA: 0x000342D8 File Offset: 0x000324D8
		// (set) Token: 0x0600237F RID: 9087 RVA: 0x000342E0 File Offset: 0x000324E0
		public bool IncludeRedeemed { get; set; }

		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x06002380 RID: 9088 RVA: 0x000342E9 File Offset: 0x000324E9
		// (set) Token: 0x06002381 RID: 9089 RVA: 0x000342F1 File Offset: 0x000324F1
		public Utf8String OverrideCatalogNamespace { get; set; }
	}
}
