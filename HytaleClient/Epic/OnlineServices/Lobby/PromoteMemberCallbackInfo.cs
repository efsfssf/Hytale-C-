using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200043B RID: 1083
	public struct PromoteMemberCallbackInfo : ICallbackInfo
	{
		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x06001C6A RID: 7274 RVA: 0x0002982D File Offset: 0x00027A2D
		// (set) Token: 0x06001C6B RID: 7275 RVA: 0x00029835 File Offset: 0x00027A35
		public Result ResultCode { get; set; }

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x06001C6C RID: 7276 RVA: 0x0002983E File Offset: 0x00027A3E
		// (set) Token: 0x06001C6D RID: 7277 RVA: 0x00029846 File Offset: 0x00027A46
		public object ClientData { get; set; }

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x06001C6E RID: 7278 RVA: 0x0002984F File Offset: 0x00027A4F
		// (set) Token: 0x06001C6F RID: 7279 RVA: 0x00029857 File Offset: 0x00027A57
		public Utf8String LobbyId { get; set; }

		// Token: 0x06001C70 RID: 7280 RVA: 0x00029860 File Offset: 0x00027A60
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x0002987D File Offset: 0x00027A7D
		internal void Set(ref PromoteMemberCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
		}
	}
}
