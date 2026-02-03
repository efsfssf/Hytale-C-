using System;

namespace Epic.OnlineServices.Stats
{
	// Token: 0x020000C4 RID: 196
	public struct CopyStatByNameOptions
	{
		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x0000A816 File Offset: 0x00008A16
		// (set) Token: 0x06000742 RID: 1858 RVA: 0x0000A81E File Offset: 0x00008A1E
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000743 RID: 1859 RVA: 0x0000A827 File Offset: 0x00008A27
		// (set) Token: 0x06000744 RID: 1860 RVA: 0x0000A82F File Offset: 0x00008A2F
		public Utf8String Name { get; set; }
	}
}
