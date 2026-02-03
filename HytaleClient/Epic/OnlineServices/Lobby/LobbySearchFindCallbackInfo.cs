using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003F3 RID: 1011
	public struct LobbySearchFindCallbackInfo : ICallbackInfo
	{
		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06001B3C RID: 6972 RVA: 0x00028F61 File Offset: 0x00027161
		// (set) Token: 0x06001B3D RID: 6973 RVA: 0x00028F69 File Offset: 0x00027169
		public Result ResultCode { get; set; }

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x06001B3E RID: 6974 RVA: 0x00028F72 File Offset: 0x00027172
		// (set) Token: 0x06001B3F RID: 6975 RVA: 0x00028F7A File Offset: 0x0002717A
		public object ClientData { get; set; }

		// Token: 0x06001B40 RID: 6976 RVA: 0x00028F84 File Offset: 0x00027184
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x00028FA1 File Offset: 0x000271A1
		internal void Set(ref LobbySearchFindCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}
	}
}
