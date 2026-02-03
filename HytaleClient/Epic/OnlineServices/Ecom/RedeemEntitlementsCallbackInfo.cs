using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000569 RID: 1385
	public struct RedeemEntitlementsCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x0600242B RID: 9259 RVA: 0x000353F7 File Offset: 0x000335F7
		// (set) Token: 0x0600242C RID: 9260 RVA: 0x000353FF File Offset: 0x000335FF
		public Result ResultCode { get; set; }

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x0600242D RID: 9261 RVA: 0x00035408 File Offset: 0x00033608
		// (set) Token: 0x0600242E RID: 9262 RVA: 0x00035410 File Offset: 0x00033610
		public object ClientData { get; set; }

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x0600242F RID: 9263 RVA: 0x00035419 File Offset: 0x00033619
		// (set) Token: 0x06002430 RID: 9264 RVA: 0x00035421 File Offset: 0x00033621
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06002431 RID: 9265 RVA: 0x0003542A File Offset: 0x0003362A
		// (set) Token: 0x06002432 RID: 9266 RVA: 0x00035432 File Offset: 0x00033632
		public uint RedeemedEntitlementIdsCount { get; set; }

		// Token: 0x06002433 RID: 9267 RVA: 0x0003543C File Offset: 0x0003363C
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x00035459 File Offset: 0x00033659
		internal void Set(ref RedeemEntitlementsCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RedeemedEntitlementIdsCount = other.RedeemedEntitlementIdsCount;
		}
	}
}
