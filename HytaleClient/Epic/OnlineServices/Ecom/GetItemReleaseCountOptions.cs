using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000530 RID: 1328
	public struct GetItemReleaseCountOptions
	{
		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x060022D8 RID: 8920 RVA: 0x000339D2 File Offset: 0x00031BD2
		// (set) Token: 0x060022D9 RID: 8921 RVA: 0x000339DA File Offset: 0x00031BDA
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x060022DA RID: 8922 RVA: 0x000339E3 File Offset: 0x00031BE3
		// (set) Token: 0x060022DB RID: 8923 RVA: 0x000339EB File Offset: 0x00031BEB
		public Utf8String ItemId { get; set; }
	}
}
