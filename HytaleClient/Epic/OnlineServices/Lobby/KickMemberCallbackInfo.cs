using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003A5 RID: 933
	public struct KickMemberCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06001925 RID: 6437 RVA: 0x00024D1F File Offset: 0x00022F1F
		// (set) Token: 0x06001926 RID: 6438 RVA: 0x00024D27 File Offset: 0x00022F27
		public Result ResultCode { get; set; }

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06001927 RID: 6439 RVA: 0x00024D30 File Offset: 0x00022F30
		// (set) Token: 0x06001928 RID: 6440 RVA: 0x00024D38 File Offset: 0x00022F38
		public object ClientData { get; set; }

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06001929 RID: 6441 RVA: 0x00024D41 File Offset: 0x00022F41
		// (set) Token: 0x0600192A RID: 6442 RVA: 0x00024D49 File Offset: 0x00022F49
		public Utf8String LobbyId { get; set; }

		// Token: 0x0600192B RID: 6443 RVA: 0x00024D54 File Offset: 0x00022F54
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x00024D71 File Offset: 0x00022F71
		internal void Set(ref KickMemberCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
		}
	}
}
