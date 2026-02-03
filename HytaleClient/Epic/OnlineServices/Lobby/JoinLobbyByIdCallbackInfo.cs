using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000399 RID: 921
	public struct JoinLobbyByIdCallbackInfo : ICallbackInfo
	{
		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x060018B6 RID: 6326 RVA: 0x000242BA File Offset: 0x000224BA
		// (set) Token: 0x060018B7 RID: 6327 RVA: 0x000242C2 File Offset: 0x000224C2
		public Result ResultCode { get; set; }

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x060018B8 RID: 6328 RVA: 0x000242CB File Offset: 0x000224CB
		// (set) Token: 0x060018B9 RID: 6329 RVA: 0x000242D3 File Offset: 0x000224D3
		public object ClientData { get; set; }

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x060018BA RID: 6330 RVA: 0x000242DC File Offset: 0x000224DC
		// (set) Token: 0x060018BB RID: 6331 RVA: 0x000242E4 File Offset: 0x000224E4
		public Utf8String LobbyId { get; set; }

		// Token: 0x060018BC RID: 6332 RVA: 0x000242F0 File Offset: 0x000224F0
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060018BD RID: 6333 RVA: 0x0002430D File Offset: 0x0002250D
		internal void Set(ref JoinLobbyByIdCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
		}
	}
}
