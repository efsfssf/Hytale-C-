using System;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004CF RID: 1231
	public struct AcceptInviteCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x06002014 RID: 8212 RVA: 0x0002F028 File Offset: 0x0002D228
		// (set) Token: 0x06002015 RID: 8213 RVA: 0x0002F030 File Offset: 0x0002D230
		public Result ResultCode { get; set; }

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x06002016 RID: 8214 RVA: 0x0002F039 File Offset: 0x0002D239
		// (set) Token: 0x06002017 RID: 8215 RVA: 0x0002F041 File Offset: 0x0002D241
		public object ClientData { get; set; }

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x06002018 RID: 8216 RVA: 0x0002F04A File Offset: 0x0002D24A
		// (set) Token: 0x06002019 RID: 8217 RVA: 0x0002F052 File Offset: 0x0002D252
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x0600201A RID: 8218 RVA: 0x0002F05B File Offset: 0x0002D25B
		// (set) Token: 0x0600201B RID: 8219 RVA: 0x0002F063 File Offset: 0x0002D263
		public EpicAccountId TargetUserId { get; set; }

		// Token: 0x0600201C RID: 8220 RVA: 0x0002F06C File Offset: 0x0002D26C
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600201D RID: 8221 RVA: 0x0002F089 File Offset: 0x0002D289
		internal void Set(ref AcceptInviteCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}
	}
}
