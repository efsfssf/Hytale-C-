using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003D2 RID: 978
	public struct LobbyInviteReceivedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x06001A86 RID: 6790 RVA: 0x00027BEC File Offset: 0x00025DEC
		// (set) Token: 0x06001A87 RID: 6791 RVA: 0x00027BF4 File Offset: 0x00025DF4
		public object ClientData { get; set; }

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x06001A88 RID: 6792 RVA: 0x00027BFD File Offset: 0x00025DFD
		// (set) Token: 0x06001A89 RID: 6793 RVA: 0x00027C05 File Offset: 0x00025E05
		public Utf8String InviteId { get; set; }

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06001A8A RID: 6794 RVA: 0x00027C0E File Offset: 0x00025E0E
		// (set) Token: 0x06001A8B RID: 6795 RVA: 0x00027C16 File Offset: 0x00025E16
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06001A8C RID: 6796 RVA: 0x00027C1F File Offset: 0x00025E1F
		// (set) Token: 0x06001A8D RID: 6797 RVA: 0x00027C27 File Offset: 0x00025E27
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x06001A8E RID: 6798 RVA: 0x00027C30 File Offset: 0x00025E30
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x00027C4B File Offset: 0x00025E4B
		internal void Set(ref LobbyInviteReceivedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.InviteId = other.InviteId;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}
	}
}
