using System;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x020000B7 RID: 183
	public struct QueryFileOptions
	{
		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060006D2 RID: 1746 RVA: 0x00009A26 File Offset: 0x00007C26
		// (set) Token: 0x060006D3 RID: 1747 RVA: 0x00009A2E File Offset: 0x00007C2E
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060006D4 RID: 1748 RVA: 0x00009A37 File Offset: 0x00007C37
		// (set) Token: 0x060006D5 RID: 1749 RVA: 0x00009A3F File Offset: 0x00007C3F
		public Utf8String Filename { get; set; }
	}
}
