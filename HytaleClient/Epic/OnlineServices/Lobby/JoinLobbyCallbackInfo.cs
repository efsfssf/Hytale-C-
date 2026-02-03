using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200039D RID: 925
	public struct JoinLobbyCallbackInfo : ICallbackInfo
	{
		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x060018DE RID: 6366 RVA: 0x00024679 File Offset: 0x00022879
		// (set) Token: 0x060018DF RID: 6367 RVA: 0x00024681 File Offset: 0x00022881
		public Result ResultCode { get; set; }

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x060018E0 RID: 6368 RVA: 0x0002468A File Offset: 0x0002288A
		// (set) Token: 0x060018E1 RID: 6369 RVA: 0x00024692 File Offset: 0x00022892
		public object ClientData { get; set; }

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x060018E2 RID: 6370 RVA: 0x0002469B File Offset: 0x0002289B
		// (set) Token: 0x060018E3 RID: 6371 RVA: 0x000246A3 File Offset: 0x000228A3
		public Utf8String LobbyId { get; set; }

		// Token: 0x060018E4 RID: 6372 RVA: 0x000246AC File Offset: 0x000228AC
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060018E5 RID: 6373 RVA: 0x000246C9 File Offset: 0x000228C9
		internal void Set(ref JoinLobbyCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
		}
	}
}
