using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003CD RID: 973
	public struct LobbyDetailsMemberInfo
	{
		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06001A12 RID: 6674 RVA: 0x000265B6 File Offset: 0x000247B6
		// (set) Token: 0x06001A13 RID: 6675 RVA: 0x000265BE File Offset: 0x000247BE
		public ProductUserId UserId { get; set; }

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06001A14 RID: 6676 RVA: 0x000265C7 File Offset: 0x000247C7
		// (set) Token: 0x06001A15 RID: 6677 RVA: 0x000265CF File Offset: 0x000247CF
		public uint Platform { get; set; }

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06001A16 RID: 6678 RVA: 0x000265D8 File Offset: 0x000247D8
		// (set) Token: 0x06001A17 RID: 6679 RVA: 0x000265E0 File Offset: 0x000247E0
		public bool AllowsCrossplay { get; set; }

		// Token: 0x06001A18 RID: 6680 RVA: 0x000265E9 File Offset: 0x000247E9
		internal void Set(ref LobbyDetailsMemberInfoInternal other)
		{
			this.UserId = other.UserId;
			this.Platform = other.Platform;
			this.AllowsCrossplay = other.AllowsCrossplay;
		}
	}
}
