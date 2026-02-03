using System;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x02000326 RID: 806
	public struct WriteFileCallbackInfo : ICallbackInfo
	{
		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06001605 RID: 5637 RVA: 0x0002011C File Offset: 0x0001E31C
		// (set) Token: 0x06001606 RID: 5638 RVA: 0x00020124 File Offset: 0x0001E324
		public Result ResultCode { get; set; }

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06001607 RID: 5639 RVA: 0x0002012D File Offset: 0x0001E32D
		// (set) Token: 0x06001608 RID: 5640 RVA: 0x00020135 File Offset: 0x0001E335
		public object ClientData { get; set; }

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06001609 RID: 5641 RVA: 0x0002013E File Offset: 0x0001E33E
		// (set) Token: 0x0600160A RID: 5642 RVA: 0x00020146 File Offset: 0x0001E346
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x0600160B RID: 5643 RVA: 0x0002014F File Offset: 0x0001E34F
		// (set) Token: 0x0600160C RID: 5644 RVA: 0x00020157 File Offset: 0x0001E357
		public Utf8String Filename { get; set; }

		// Token: 0x0600160D RID: 5645 RVA: 0x00020160 File Offset: 0x0001E360
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x0002017D File Offset: 0x0001E37D
		internal void Set(ref WriteFileCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.Filename = other.Filename;
		}
	}
}
