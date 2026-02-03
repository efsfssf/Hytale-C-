using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200051A RID: 1306
	public struct CopyOfferByIdOptions
	{
		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x0600223F RID: 8767 RVA: 0x0003246E File Offset: 0x0003066E
		// (set) Token: 0x06002240 RID: 8768 RVA: 0x00032476 File Offset: 0x00030676
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x06002241 RID: 8769 RVA: 0x0003247F File Offset: 0x0003067F
		// (set) Token: 0x06002242 RID: 8770 RVA: 0x00032487 File Offset: 0x00030687
		public Utf8String OfferId { get; set; }
	}
}
