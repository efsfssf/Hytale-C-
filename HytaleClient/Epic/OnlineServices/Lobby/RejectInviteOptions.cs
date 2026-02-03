using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000445 RID: 1093
	public struct RejectInviteOptions
	{
		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06001CB5 RID: 7349 RVA: 0x00029F32 File Offset: 0x00028132
		// (set) Token: 0x06001CB6 RID: 7350 RVA: 0x00029F3A File Offset: 0x0002813A
		public Utf8String InviteId { get; set; }

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06001CB7 RID: 7351 RVA: 0x00029F43 File Offset: 0x00028143
		// (set) Token: 0x06001CB8 RID: 7352 RVA: 0x00029F4B File Offset: 0x0002814B
		public ProductUserId LocalUserId { get; set; }
	}
}
