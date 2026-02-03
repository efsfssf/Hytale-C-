using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003D9 RID: 985
	public struct LobbyMemberUpdateReceivedCallbackInfo : ICallbackInfo
	{
		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06001ACF RID: 6863 RVA: 0x000283A3 File Offset: 0x000265A3
		// (set) Token: 0x06001AD0 RID: 6864 RVA: 0x000283AB File Offset: 0x000265AB
		public object ClientData { get; set; }

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06001AD1 RID: 6865 RVA: 0x000283B4 File Offset: 0x000265B4
		// (set) Token: 0x06001AD2 RID: 6866 RVA: 0x000283BC File Offset: 0x000265BC
		public Utf8String LobbyId { get; set; }

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x06001AD3 RID: 6867 RVA: 0x000283C5 File Offset: 0x000265C5
		// (set) Token: 0x06001AD4 RID: 6868 RVA: 0x000283CD File Offset: 0x000265CD
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x06001AD5 RID: 6869 RVA: 0x000283D8 File Offset: 0x000265D8
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06001AD6 RID: 6870 RVA: 0x000283F3 File Offset: 0x000265F3
		internal void Set(ref LobbyMemberUpdateReceivedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
			this.TargetUserId = other.TargetUserId;
		}
	}
}
