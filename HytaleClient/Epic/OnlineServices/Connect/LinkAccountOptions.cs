using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005ED RID: 1517
	public struct LinkAccountOptions
	{
		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x06002764 RID: 10084 RVA: 0x0003A4F6 File Offset: 0x000386F6
		// (set) Token: 0x06002765 RID: 10085 RVA: 0x0003A4FE File Offset: 0x000386FE
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x06002766 RID: 10086 RVA: 0x0003A507 File Offset: 0x00038707
		// (set) Token: 0x06002767 RID: 10087 RVA: 0x0003A50F File Offset: 0x0003870F
		public ContinuanceToken ContinuanceToken { get; set; }
	}
}
