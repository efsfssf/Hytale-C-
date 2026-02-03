using System;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x02000479 RID: 1145
	public struct OnQueryLeaderboardUserScoresCompleteCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06001DF5 RID: 7669 RVA: 0x0002BE16 File Offset: 0x0002A016
		// (set) Token: 0x06001DF6 RID: 7670 RVA: 0x0002BE1E File Offset: 0x0002A01E
		public Result ResultCode { get; set; }

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06001DF7 RID: 7671 RVA: 0x0002BE27 File Offset: 0x0002A027
		// (set) Token: 0x06001DF8 RID: 7672 RVA: 0x0002BE2F File Offset: 0x0002A02F
		public object ClientData { get; set; }

		// Token: 0x06001DF9 RID: 7673 RVA: 0x0002BE38 File Offset: 0x0002A038
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06001DFA RID: 7674 RVA: 0x0002BE55 File Offset: 0x0002A055
		internal void Set(ref OnQueryLeaderboardUserScoresCompleteCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}
	}
}
