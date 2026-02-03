using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000161 RID: 353
	public struct SessionModificationAddAttributeOptions
	{
		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x0000F522 File Offset: 0x0000D722
		// (set) Token: 0x06000AC7 RID: 2759 RVA: 0x0000F52A File Offset: 0x0000D72A
		public AttributeData? SessionAttribute { get; set; }

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x0000F533 File Offset: 0x0000D733
		// (set) Token: 0x06000AC9 RID: 2761 RVA: 0x0000F53B File Offset: 0x0000D73B
		public SessionAttributeAdvertisementType AdvertisementType { get; set; }
	}
}
