using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000555 RID: 1365
	public struct QueryEntitlementTokenCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x06002389 RID: 9097 RVA: 0x0003441C File Offset: 0x0003261C
		// (set) Token: 0x0600238A RID: 9098 RVA: 0x00034424 File Offset: 0x00032624
		public Result ResultCode { get; set; }

		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x0600238B RID: 9099 RVA: 0x0003442D File Offset: 0x0003262D
		// (set) Token: 0x0600238C RID: 9100 RVA: 0x00034435 File Offset: 0x00032635
		public object ClientData { get; set; }

		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x0600238D RID: 9101 RVA: 0x0003443E File Offset: 0x0003263E
		// (set) Token: 0x0600238E RID: 9102 RVA: 0x00034446 File Offset: 0x00032646
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x0600238F RID: 9103 RVA: 0x0003444F File Offset: 0x0003264F
		// (set) Token: 0x06002390 RID: 9104 RVA: 0x00034457 File Offset: 0x00032657
		public Utf8String EntitlementToken { get; set; }

		// Token: 0x06002391 RID: 9105 RVA: 0x00034460 File Offset: 0x00032660
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06002392 RID: 9106 RVA: 0x0003447D File Offset: 0x0003267D
		internal void Set(ref QueryEntitlementTokenCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.EntitlementToken = other.EntitlementToken;
		}
	}
}
