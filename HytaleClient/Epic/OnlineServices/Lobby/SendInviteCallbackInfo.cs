using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000449 RID: 1097
	public struct SendInviteCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06001CD9 RID: 7385 RVA: 0x0002A2D8 File Offset: 0x000284D8
		// (set) Token: 0x06001CDA RID: 7386 RVA: 0x0002A2E0 File Offset: 0x000284E0
		public Result ResultCode { get; set; }

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06001CDB RID: 7387 RVA: 0x0002A2E9 File Offset: 0x000284E9
		// (set) Token: 0x06001CDC RID: 7388 RVA: 0x0002A2F1 File Offset: 0x000284F1
		public object ClientData { get; set; }

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x06001CDD RID: 7389 RVA: 0x0002A2FA File Offset: 0x000284FA
		// (set) Token: 0x06001CDE RID: 7390 RVA: 0x0002A302 File Offset: 0x00028502
		public Utf8String LobbyId { get; set; }

		// Token: 0x06001CDF RID: 7391 RVA: 0x0002A30C File Offset: 0x0002850C
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06001CE0 RID: 7392 RVA: 0x0002A329 File Offset: 0x00028529
		internal void Set(ref SendInviteCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
		}
	}
}
