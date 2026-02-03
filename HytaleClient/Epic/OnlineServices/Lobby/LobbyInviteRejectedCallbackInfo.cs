using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003D4 RID: 980
	public struct LobbyInviteRejectedCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06001A9D RID: 6813 RVA: 0x00027E53 File Offset: 0x00026053
		// (set) Token: 0x06001A9E RID: 6814 RVA: 0x00027E5B File Offset: 0x0002605B
		public object ClientData { get; set; }

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06001A9F RID: 6815 RVA: 0x00027E64 File Offset: 0x00026064
		// (set) Token: 0x06001AA0 RID: 6816 RVA: 0x00027E6C File Offset: 0x0002606C
		public Utf8String InviteId { get; set; }

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06001AA1 RID: 6817 RVA: 0x00027E75 File Offset: 0x00026075
		// (set) Token: 0x06001AA2 RID: 6818 RVA: 0x00027E7D File Offset: 0x0002607D
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x06001AA3 RID: 6819 RVA: 0x00027E86 File Offset: 0x00026086
		// (set) Token: 0x06001AA4 RID: 6820 RVA: 0x00027E8E File Offset: 0x0002608E
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x06001AA5 RID: 6821 RVA: 0x00027E97 File Offset: 0x00026097
		// (set) Token: 0x06001AA6 RID: 6822 RVA: 0x00027E9F File Offset: 0x0002609F
		public Utf8String LobbyId { get; set; }

		// Token: 0x06001AA7 RID: 6823 RVA: 0x00027EA8 File Offset: 0x000260A8
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06001AA8 RID: 6824 RVA: 0x00027EC4 File Offset: 0x000260C4
		internal void Set(ref LobbyInviteRejectedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.InviteId = other.InviteId;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
			this.LobbyId = other.LobbyId;
		}
	}
}
