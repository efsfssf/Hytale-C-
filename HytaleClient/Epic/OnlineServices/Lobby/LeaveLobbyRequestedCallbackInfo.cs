using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003AD RID: 941
	public struct LeaveLobbyRequestedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06001960 RID: 6496 RVA: 0x000252A6 File Offset: 0x000234A6
		// (set) Token: 0x06001961 RID: 6497 RVA: 0x000252AE File Offset: 0x000234AE
		public object ClientData { get; set; }

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06001962 RID: 6498 RVA: 0x000252B7 File Offset: 0x000234B7
		// (set) Token: 0x06001963 RID: 6499 RVA: 0x000252BF File Offset: 0x000234BF
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x06001964 RID: 6500 RVA: 0x000252C8 File Offset: 0x000234C8
		// (set) Token: 0x06001965 RID: 6501 RVA: 0x000252D0 File Offset: 0x000234D0
		public Utf8String LobbyId { get; set; }

		// Token: 0x06001966 RID: 6502 RVA: 0x000252DC File Offset: 0x000234DC
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x000252F7 File Offset: 0x000234F7
		internal void Set(ref LeaveLobbyRequestedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.LobbyId = other.LobbyId;
		}
	}
}
