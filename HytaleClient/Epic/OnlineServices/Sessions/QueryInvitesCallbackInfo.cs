using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000138 RID: 312
	public struct QueryInvitesCallbackInfo : ICallbackInfo
	{
		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x0600097E RID: 2430 RVA: 0x0000D22A File Offset: 0x0000B42A
		// (set) Token: 0x0600097F RID: 2431 RVA: 0x0000D232 File Offset: 0x0000B432
		public Result ResultCode { get; set; }

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000980 RID: 2432 RVA: 0x0000D23B File Offset: 0x0000B43B
		// (set) Token: 0x06000981 RID: 2433 RVA: 0x0000D243 File Offset: 0x0000B443
		public object ClientData { get; set; }

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000982 RID: 2434 RVA: 0x0000D24C File Offset: 0x0000B44C
		// (set) Token: 0x06000983 RID: 2435 RVA: 0x0000D254 File Offset: 0x0000B454
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x06000984 RID: 2436 RVA: 0x0000D260 File Offset: 0x0000B460
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0000D27D File Offset: 0x0000B47D
		internal void Set(ref QueryInvitesCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
