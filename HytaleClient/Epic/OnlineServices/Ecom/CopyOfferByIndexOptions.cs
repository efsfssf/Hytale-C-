using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200051C RID: 1308
	public struct CopyOfferByIndexOptions
	{
		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x06002248 RID: 8776 RVA: 0x0003253A File Offset: 0x0003073A
		// (set) Token: 0x06002249 RID: 8777 RVA: 0x00032542 File Offset: 0x00030742
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x0600224A RID: 8778 RVA: 0x0003254B File Offset: 0x0003074B
		// (set) Token: 0x0600224B RID: 8779 RVA: 0x00032553 File Offset: 0x00030753
		public uint OfferIndex { get; set; }
	}
}
