using System;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x02000475 RID: 1141
	public struct OnQueryLeaderboardRanksCompleteCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06001DDA RID: 7642 RVA: 0x0002BC49 File Offset: 0x00029E49
		// (set) Token: 0x06001DDB RID: 7643 RVA: 0x0002BC51 File Offset: 0x00029E51
		public Result ResultCode { get; set; }

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06001DDC RID: 7644 RVA: 0x0002BC5A File Offset: 0x00029E5A
		// (set) Token: 0x06001DDD RID: 7645 RVA: 0x0002BC62 File Offset: 0x00029E62
		public object ClientData { get; set; }

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06001DDE RID: 7646 RVA: 0x0002BC6B File Offset: 0x00029E6B
		// (set) Token: 0x06001DDF RID: 7647 RVA: 0x0002BC73 File Offset: 0x00029E73
		public Utf8String LeaderboardId { get; set; }

		// Token: 0x06001DE0 RID: 7648 RVA: 0x0002BC7C File Offset: 0x00029E7C
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x0002BC99 File Offset: 0x00029E99
		internal void Set(ref OnQueryLeaderboardRanksCompleteCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LeaderboardId = other.LeaderboardId;
		}
	}
}
