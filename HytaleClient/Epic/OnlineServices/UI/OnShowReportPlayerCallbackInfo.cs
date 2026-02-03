using System;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000078 RID: 120
	public struct OnShowReportPlayerCallbackInfo : ICallbackInfo
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x00007423 File Offset: 0x00005623
		// (set) Token: 0x0600052F RID: 1327 RVA: 0x0000742B File Offset: 0x0000562B
		public Result ResultCode { get; set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x00007434 File Offset: 0x00005634
		// (set) Token: 0x06000531 RID: 1329 RVA: 0x0000743C File Offset: 0x0000563C
		public object ClientData { get; set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x00007445 File Offset: 0x00005645
		// (set) Token: 0x06000533 RID: 1331 RVA: 0x0000744D File Offset: 0x0000564D
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x00007456 File Offset: 0x00005656
		// (set) Token: 0x06000535 RID: 1333 RVA: 0x0000745E File Offset: 0x0000565E
		public EpicAccountId TargetUserId { get; set; }

		// Token: 0x06000536 RID: 1334 RVA: 0x00007468 File Offset: 0x00005668
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00007485 File Offset: 0x00005685
		internal void Set(ref OnShowReportPlayerCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}
	}
}
