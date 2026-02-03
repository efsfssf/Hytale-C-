using System;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x020004AF RID: 1199
	public struct UpdateParentEmailOptions
	{
		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06001F5A RID: 8026 RVA: 0x0002DE62 File Offset: 0x0002C062
		// (set) Token: 0x06001F5B RID: 8027 RVA: 0x0002DE6A File Offset: 0x0002C06A
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06001F5C RID: 8028 RVA: 0x0002DE73 File Offset: 0x0002C073
		// (set) Token: 0x06001F5D RID: 8029 RVA: 0x0002DE7B File Offset: 0x0002C07B
		public Utf8String ParentEmail { get; set; }
	}
}
