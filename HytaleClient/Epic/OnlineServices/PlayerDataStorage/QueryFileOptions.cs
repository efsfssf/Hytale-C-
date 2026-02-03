using System;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x0200031D RID: 797
	public struct QueryFileOptions
	{
		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x060015B4 RID: 5556 RVA: 0x0001F849 File Offset: 0x0001DA49
		// (set) Token: 0x060015B5 RID: 5557 RVA: 0x0001F851 File Offset: 0x0001DA51
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x060015B6 RID: 5558 RVA: 0x0001F85A File Offset: 0x0001DA5A
		// (set) Token: 0x060015B7 RID: 5559 RVA: 0x0001F862 File Offset: 0x0001DA62
		public Utf8String Filename { get; set; }
	}
}
