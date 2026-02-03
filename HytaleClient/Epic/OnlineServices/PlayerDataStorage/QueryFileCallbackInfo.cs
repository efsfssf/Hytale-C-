using System;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x02000317 RID: 791
	public struct QueryFileCallbackInfo : ICallbackInfo
	{
		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06001584 RID: 5508 RVA: 0x0001F3C6 File Offset: 0x0001D5C6
		// (set) Token: 0x06001585 RID: 5509 RVA: 0x0001F3CE File Offset: 0x0001D5CE
		public Result ResultCode { get; set; }

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06001586 RID: 5510 RVA: 0x0001F3D7 File Offset: 0x0001D5D7
		// (set) Token: 0x06001587 RID: 5511 RVA: 0x0001F3DF File Offset: 0x0001D5DF
		public object ClientData { get; set; }

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001588 RID: 5512 RVA: 0x0001F3E8 File Offset: 0x0001D5E8
		// (set) Token: 0x06001589 RID: 5513 RVA: 0x0001F3F0 File Offset: 0x0001D5F0
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x0600158A RID: 5514 RVA: 0x0001F3FC File Offset: 0x0001D5FC
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x0001F419 File Offset: 0x0001D619
		internal void Set(ref QueryFileCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
