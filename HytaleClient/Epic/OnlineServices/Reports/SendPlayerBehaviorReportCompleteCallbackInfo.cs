using System;

namespace Epic.OnlineServices.Reports
{
	// Token: 0x020002A1 RID: 673
	public struct SendPlayerBehaviorReportCompleteCallbackInfo : ICallbackInfo
	{
		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x060012CD RID: 4813 RVA: 0x0001B683 File Offset: 0x00019883
		// (set) Token: 0x060012CE RID: 4814 RVA: 0x0001B68B File Offset: 0x0001988B
		public Result ResultCode { get; set; }

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x060012CF RID: 4815 RVA: 0x0001B694 File Offset: 0x00019894
		// (set) Token: 0x060012D0 RID: 4816 RVA: 0x0001B69C File Offset: 0x0001989C
		public object ClientData { get; set; }

		// Token: 0x060012D1 RID: 4817 RVA: 0x0001B6A8 File Offset: 0x000198A8
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x0001B6C5 File Offset: 0x000198C5
		internal void Set(ref SendPlayerBehaviorReportCompleteCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}
	}
}
