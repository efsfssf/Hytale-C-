using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000536 RID: 1334
	public struct GetOfferImageInfoCountOptions
	{
		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x060022ED RID: 8941 RVA: 0x00033B9D File Offset: 0x00031D9D
		// (set) Token: 0x060022EE RID: 8942 RVA: 0x00033BA5 File Offset: 0x00031DA5
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x060022EF RID: 8943 RVA: 0x00033BAE File Offset: 0x00031DAE
		// (set) Token: 0x060022F0 RID: 8944 RVA: 0x00033BB6 File Offset: 0x00031DB6
		public Utf8String OfferId { get; set; }
	}
}
