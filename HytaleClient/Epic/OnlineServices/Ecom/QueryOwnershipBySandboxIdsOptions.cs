using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200055F RID: 1375
	public struct QueryOwnershipBySandboxIdsOptions
	{
		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x060023DC RID: 9180 RVA: 0x00034C33 File Offset: 0x00032E33
		// (set) Token: 0x060023DD RID: 9181 RVA: 0x00034C3B File Offset: 0x00032E3B
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x060023DE RID: 9182 RVA: 0x00034C44 File Offset: 0x00032E44
		// (set) Token: 0x060023DF RID: 9183 RVA: 0x00034C4C File Offset: 0x00032E4C
		public Utf8String[] SandboxIds { get; set; }
	}
}
