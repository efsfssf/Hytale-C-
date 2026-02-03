using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200044F RID: 1103
	public struct UpdateLobbyCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x06001D17 RID: 7447 RVA: 0x0002A92C File Offset: 0x00028B2C
		// (set) Token: 0x06001D18 RID: 7448 RVA: 0x0002A934 File Offset: 0x00028B34
		public Result ResultCode { get; set; }

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06001D19 RID: 7449 RVA: 0x0002A93D File Offset: 0x00028B3D
		// (set) Token: 0x06001D1A RID: 7450 RVA: 0x0002A945 File Offset: 0x00028B45
		public object ClientData { get; set; }

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x06001D1B RID: 7451 RVA: 0x0002A94E File Offset: 0x00028B4E
		// (set) Token: 0x06001D1C RID: 7452 RVA: 0x0002A956 File Offset: 0x00028B56
		public Utf8String LobbyId { get; set; }

		// Token: 0x06001D1D RID: 7453 RVA: 0x0002A960 File Offset: 0x00028B60
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x0002A97D File Offset: 0x00028B7D
		internal void Set(ref UpdateLobbyCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
		}
	}
}
