using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200052E RID: 1326
	public struct GetItemImageInfoCountOptions
	{
		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x060022CF RID: 8911 RVA: 0x00033905 File Offset: 0x00031B05
		// (set) Token: 0x060022D0 RID: 8912 RVA: 0x0003390D File Offset: 0x00031B0D
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x060022D1 RID: 8913 RVA: 0x00033916 File Offset: 0x00031B16
		// (set) Token: 0x060022D2 RID: 8914 RVA: 0x0003391E File Offset: 0x00031B1E
		public Utf8String ItemId { get; set; }
	}
}
