using System;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x0200048B RID: 1163
	public struct GetPermissionByKeyOptions
	{
		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06001E6B RID: 7787 RVA: 0x0002C903 File Offset: 0x0002AB03
		// (set) Token: 0x06001E6C RID: 7788 RVA: 0x0002C90B File Offset: 0x0002AB0B
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06001E6D RID: 7789 RVA: 0x0002C914 File Offset: 0x0002AB14
		// (set) Token: 0x06001E6E RID: 7790 RVA: 0x0002C91C File Offset: 0x0002AB1C
		public Utf8String Key { get; set; }
	}
}
