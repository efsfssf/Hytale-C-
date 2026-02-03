using System;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x02000319 RID: 793
	public struct QueryFileListCallbackInfo : ICallbackInfo
	{
		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06001597 RID: 5527 RVA: 0x0001F596 File Offset: 0x0001D796
		// (set) Token: 0x06001598 RID: 5528 RVA: 0x0001F59E File Offset: 0x0001D79E
		public Result ResultCode { get; set; }

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06001599 RID: 5529 RVA: 0x0001F5A7 File Offset: 0x0001D7A7
		// (set) Token: 0x0600159A RID: 5530 RVA: 0x0001F5AF File Offset: 0x0001D7AF
		public object ClientData { get; set; }

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x0600159B RID: 5531 RVA: 0x0001F5B8 File Offset: 0x0001D7B8
		// (set) Token: 0x0600159C RID: 5532 RVA: 0x0001F5C0 File Offset: 0x0001D7C0
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x0600159D RID: 5533 RVA: 0x0001F5C9 File Offset: 0x0001D7C9
		// (set) Token: 0x0600159E RID: 5534 RVA: 0x0001F5D1 File Offset: 0x0001D7D1
		public uint FileCount { get; set; }

		// Token: 0x0600159F RID: 5535 RVA: 0x0001F5DC File Offset: 0x0001D7DC
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060015A0 RID: 5536 RVA: 0x0001F5F9 File Offset: 0x0001D7F9
		internal void Set(ref QueryFileListCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.FileCount = other.FileCount;
		}
	}
}
