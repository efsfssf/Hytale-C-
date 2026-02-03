using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003D0 RID: 976
	public struct LobbyInviteAcceptedCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06001A6B RID: 6763 RVA: 0x000278EA File Offset: 0x00025AEA
		// (set) Token: 0x06001A6C RID: 6764 RVA: 0x000278F2 File Offset: 0x00025AF2
		public object ClientData { get; set; }

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06001A6D RID: 6765 RVA: 0x000278FB File Offset: 0x00025AFB
		// (set) Token: 0x06001A6E RID: 6766 RVA: 0x00027903 File Offset: 0x00025B03
		public Utf8String InviteId { get; set; }

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06001A6F RID: 6767 RVA: 0x0002790C File Offset: 0x00025B0C
		// (set) Token: 0x06001A70 RID: 6768 RVA: 0x00027914 File Offset: 0x00025B14
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06001A71 RID: 6769 RVA: 0x0002791D File Offset: 0x00025B1D
		// (set) Token: 0x06001A72 RID: 6770 RVA: 0x00027925 File Offset: 0x00025B25
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06001A73 RID: 6771 RVA: 0x0002792E File Offset: 0x00025B2E
		// (set) Token: 0x06001A74 RID: 6772 RVA: 0x00027936 File Offset: 0x00025B36
		public Utf8String LobbyId { get; set; }

		// Token: 0x06001A75 RID: 6773 RVA: 0x00027940 File Offset: 0x00025B40
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06001A76 RID: 6774 RVA: 0x0002795C File Offset: 0x00025B5C
		internal void Set(ref LobbyInviteAcceptedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.InviteId = other.InviteId;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
			this.LobbyId = other.LobbyId;
		}
	}
}
