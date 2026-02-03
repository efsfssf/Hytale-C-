using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000520 RID: 1312
	public struct CopyOfferItemByIndexOptions
	{
		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x0600225D RID: 8797 RVA: 0x000326FF File Offset: 0x000308FF
		// (set) Token: 0x0600225E RID: 8798 RVA: 0x00032707 File Offset: 0x00030907
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x0600225F RID: 8799 RVA: 0x00032710 File Offset: 0x00030910
		// (set) Token: 0x06002260 RID: 8800 RVA: 0x00032718 File Offset: 0x00030918
		public Utf8String OfferId { get; set; }

		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x06002261 RID: 8801 RVA: 0x00032721 File Offset: 0x00030921
		// (set) Token: 0x06002262 RID: 8802 RVA: 0x00032729 File Offset: 0x00030929
		public uint ItemIndex { get; set; }
	}
}
