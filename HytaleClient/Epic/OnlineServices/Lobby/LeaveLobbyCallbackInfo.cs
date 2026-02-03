using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003A9 RID: 937
	public struct LeaveLobbyCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06001944 RID: 6468 RVA: 0x0002500B File Offset: 0x0002320B
		// (set) Token: 0x06001945 RID: 6469 RVA: 0x00025013 File Offset: 0x00023213
		public Result ResultCode { get; set; }

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06001946 RID: 6470 RVA: 0x0002501C File Offset: 0x0002321C
		// (set) Token: 0x06001947 RID: 6471 RVA: 0x00025024 File Offset: 0x00023224
		public object ClientData { get; set; }

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06001948 RID: 6472 RVA: 0x0002502D File Offset: 0x0002322D
		// (set) Token: 0x06001949 RID: 6473 RVA: 0x00025035 File Offset: 0x00023235
		public Utf8String LobbyId { get; set; }

		// Token: 0x0600194A RID: 6474 RVA: 0x00025040 File Offset: 0x00023240
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600194B RID: 6475 RVA: 0x0002505D File Offset: 0x0002325D
		internal void Set(ref LeaveLobbyCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
		}
	}
}
