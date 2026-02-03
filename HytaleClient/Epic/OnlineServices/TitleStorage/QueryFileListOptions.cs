using System;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x020000B5 RID: 181
	public struct QueryFileListOptions
	{
		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x00009953 File Offset: 0x00007B53
		// (set) Token: 0x060006CA RID: 1738 RVA: 0x0000995B File Offset: 0x00007B5B
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x00009964 File Offset: 0x00007B64
		// (set) Token: 0x060006CC RID: 1740 RVA: 0x0000996C File Offset: 0x00007B6C
		public Utf8String[] ListOfTags { get; set; }
	}
}
