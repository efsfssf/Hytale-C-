using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003AB RID: 939
	public struct LeaveLobbyOptions
	{
		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x06001957 RID: 6487 RVA: 0x000251DA File Offset: 0x000233DA
		// (set) Token: 0x06001958 RID: 6488 RVA: 0x000251E2 File Offset: 0x000233E2
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x06001959 RID: 6489 RVA: 0x000251EB File Offset: 0x000233EB
		// (set) Token: 0x0600195A RID: 6490 RVA: 0x000251F3 File Offset: 0x000233F3
		public Utf8String LobbyId { get; set; }
	}
}
