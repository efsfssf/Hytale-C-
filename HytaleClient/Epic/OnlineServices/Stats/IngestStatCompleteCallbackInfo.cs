using System;

namespace Epic.OnlineServices.Stats
{
	// Token: 0x020000CA RID: 202
	public struct IngestStatCompleteCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600075D RID: 1885 RVA: 0x0000AA88 File Offset: 0x00008C88
		// (set) Token: 0x0600075E RID: 1886 RVA: 0x0000AA90 File Offset: 0x00008C90
		public Result ResultCode { get; set; }

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600075F RID: 1887 RVA: 0x0000AA99 File Offset: 0x00008C99
		// (set) Token: 0x06000760 RID: 1888 RVA: 0x0000AAA1 File Offset: 0x00008CA1
		public object ClientData { get; set; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000761 RID: 1889 RVA: 0x0000AAAA File Offset: 0x00008CAA
		// (set) Token: 0x06000762 RID: 1890 RVA: 0x0000AAB2 File Offset: 0x00008CB2
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000763 RID: 1891 RVA: 0x0000AABB File Offset: 0x00008CBB
		// (set) Token: 0x06000764 RID: 1892 RVA: 0x0000AAC3 File Offset: 0x00008CC3
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x06000765 RID: 1893 RVA: 0x0000AACC File Offset: 0x00008CCC
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0000AAE9 File Offset: 0x00008CE9
		internal void Set(ref IngestStatCompleteCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}
	}
}
