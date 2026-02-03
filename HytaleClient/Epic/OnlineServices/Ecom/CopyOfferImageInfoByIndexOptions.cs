using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200051E RID: 1310
	public struct CopyOfferImageInfoByIndexOptions
	{
		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x06002251 RID: 8785 RVA: 0x000325F6 File Offset: 0x000307F6
		// (set) Token: 0x06002252 RID: 8786 RVA: 0x000325FE File Offset: 0x000307FE
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x06002253 RID: 8787 RVA: 0x00032607 File Offset: 0x00030807
		// (set) Token: 0x06002254 RID: 8788 RVA: 0x0003260F File Offset: 0x0003080F
		public Utf8String OfferId { get; set; }

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x06002255 RID: 8789 RVA: 0x00032618 File Offset: 0x00030818
		// (set) Token: 0x06002256 RID: 8790 RVA: 0x00032620 File Offset: 0x00030820
		public uint ImageInfoIndex { get; set; }
	}
}
