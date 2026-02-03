using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000385 RID: 901
	public struct DestroyLobbyCallbackInfo : ICallbackInfo
	{
		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06001837 RID: 6199 RVA: 0x00023701 File Offset: 0x00021901
		// (set) Token: 0x06001838 RID: 6200 RVA: 0x00023709 File Offset: 0x00021909
		public Result ResultCode { get; set; }

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06001839 RID: 6201 RVA: 0x00023712 File Offset: 0x00021912
		// (set) Token: 0x0600183A RID: 6202 RVA: 0x0002371A File Offset: 0x0002191A
		public object ClientData { get; set; }

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x0600183B RID: 6203 RVA: 0x00023723 File Offset: 0x00021923
		// (set) Token: 0x0600183C RID: 6204 RVA: 0x0002372B File Offset: 0x0002192B
		public Utf8String LobbyId { get; set; }

		// Token: 0x0600183D RID: 6205 RVA: 0x00023734 File Offset: 0x00021934
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600183E RID: 6206 RVA: 0x00023751 File Offset: 0x00021951
		internal void Set(ref DestroyLobbyCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
		}
	}
}
