using System;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x02000471 RID: 1137
	public struct OnQueryLeaderboardDefinitionsCompleteCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06001DC3 RID: 7619 RVA: 0x0002BAF4 File Offset: 0x00029CF4
		// (set) Token: 0x06001DC4 RID: 7620 RVA: 0x0002BAFC File Offset: 0x00029CFC
		public Result ResultCode { get; set; }

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06001DC5 RID: 7621 RVA: 0x0002BB05 File Offset: 0x00029D05
		// (set) Token: 0x06001DC6 RID: 7622 RVA: 0x0002BB0D File Offset: 0x00029D0D
		public object ClientData { get; set; }

		// Token: 0x06001DC7 RID: 7623 RVA: 0x0002BB18 File Offset: 0x00029D18
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x0002BB35 File Offset: 0x00029D35
		internal void Set(ref OnQueryLeaderboardDefinitionsCompleteCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}
	}
}
