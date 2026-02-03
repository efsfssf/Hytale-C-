using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200010A RID: 266
	public struct GetInviteIdByIndexOptions
	{
		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x0000CA35 File Offset: 0x0000AC35
		// (set) Token: 0x060008A9 RID: 2217 RVA: 0x0000CA3D File Offset: 0x0000AC3D
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x0000CA46 File Offset: 0x0000AC46
		// (set) Token: 0x060008AB RID: 2219 RVA: 0x0000CA4E File Offset: 0x0000AC4E
		public uint Index { get; set; }
	}
}
