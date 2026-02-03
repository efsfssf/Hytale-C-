using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000391 RID: 913
	public struct HardMuteMemberCallbackInfo : ICallbackInfo
	{
		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06001874 RID: 6260 RVA: 0x00023C6E File Offset: 0x00021E6E
		// (set) Token: 0x06001875 RID: 6261 RVA: 0x00023C76 File Offset: 0x00021E76
		public Result ResultCode { get; set; }

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06001876 RID: 6262 RVA: 0x00023C7F File Offset: 0x00021E7F
		// (set) Token: 0x06001877 RID: 6263 RVA: 0x00023C87 File Offset: 0x00021E87
		public object ClientData { get; set; }

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06001878 RID: 6264 RVA: 0x00023C90 File Offset: 0x00021E90
		// (set) Token: 0x06001879 RID: 6265 RVA: 0x00023C98 File Offset: 0x00021E98
		public Utf8String LobbyId { get; set; }

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x0600187A RID: 6266 RVA: 0x00023CA1 File Offset: 0x00021EA1
		// (set) Token: 0x0600187B RID: 6267 RVA: 0x00023CA9 File Offset: 0x00021EA9
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x0600187C RID: 6268 RVA: 0x00023CB4 File Offset: 0x00021EB4
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x00023CD1 File Offset: 0x00021ED1
		internal void Set(ref HardMuteMemberCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
			this.TargetUserId = other.TargetUserId;
		}
	}
}
