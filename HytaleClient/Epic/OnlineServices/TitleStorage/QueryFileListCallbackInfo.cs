using System;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x020000B3 RID: 179
	public struct QueryFileListCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060006B2 RID: 1714 RVA: 0x0000971E File Offset: 0x0000791E
		// (set) Token: 0x060006B3 RID: 1715 RVA: 0x00009726 File Offset: 0x00007926
		public Result ResultCode { get; set; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060006B4 RID: 1716 RVA: 0x0000972F File Offset: 0x0000792F
		// (set) Token: 0x060006B5 RID: 1717 RVA: 0x00009737 File Offset: 0x00007937
		public object ClientData { get; set; }

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x00009740 File Offset: 0x00007940
		// (set) Token: 0x060006B7 RID: 1719 RVA: 0x00009748 File Offset: 0x00007948
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x00009751 File Offset: 0x00007951
		// (set) Token: 0x060006B9 RID: 1721 RVA: 0x00009759 File Offset: 0x00007959
		public uint FileCount { get; set; }

		// Token: 0x060006BA RID: 1722 RVA: 0x00009764 File Offset: 0x00007964
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00009781 File Offset: 0x00007981
		internal void Set(ref QueryFileListCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.FileCount = other.FileCount;
		}
	}
}
