using System;

namespace Epic.OnlineServices.Stats
{
	// Token: 0x020000C2 RID: 194
	public struct CopyStatByIndexOptions
	{
		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000738 RID: 1848 RVA: 0x0000A75A File Offset: 0x0000895A
		// (set) Token: 0x06000739 RID: 1849 RVA: 0x0000A762 File Offset: 0x00008962
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x0600073A RID: 1850 RVA: 0x0000A76B File Offset: 0x0000896B
		// (set) Token: 0x0600073B RID: 1851 RVA: 0x0000A773 File Offset: 0x00008973
		public uint StatIndex { get; set; }
	}
}
