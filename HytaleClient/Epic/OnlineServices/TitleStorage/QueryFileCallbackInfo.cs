using System;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x020000B1 RID: 177
	public struct QueryFileCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x00009551 File Offset: 0x00007751
		// (set) Token: 0x060006A0 RID: 1696 RVA: 0x00009559 File Offset: 0x00007759
		public Result ResultCode { get; set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x00009562 File Offset: 0x00007762
		// (set) Token: 0x060006A2 RID: 1698 RVA: 0x0000956A File Offset: 0x0000776A
		public object ClientData { get; set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x00009573 File Offset: 0x00007773
		// (set) Token: 0x060006A4 RID: 1700 RVA: 0x0000957B File Offset: 0x0000777B
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x060006A5 RID: 1701 RVA: 0x00009584 File Offset: 0x00007784
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x000095A1 File Offset: 0x000077A1
		internal void Set(ref QueryFileCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
