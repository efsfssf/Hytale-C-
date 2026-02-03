using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000559 RID: 1369
	public struct QueryOffersCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x060023A9 RID: 9129 RVA: 0x0003473A File Offset: 0x0003293A
		// (set) Token: 0x060023AA RID: 9130 RVA: 0x00034742 File Offset: 0x00032942
		public Result ResultCode { get; set; }

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x060023AB RID: 9131 RVA: 0x0003474B File Offset: 0x0003294B
		// (set) Token: 0x060023AC RID: 9132 RVA: 0x00034753 File Offset: 0x00032953
		public object ClientData { get; set; }

		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x060023AD RID: 9133 RVA: 0x0003475C File Offset: 0x0003295C
		// (set) Token: 0x060023AE RID: 9134 RVA: 0x00034764 File Offset: 0x00032964
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x060023AF RID: 9135 RVA: 0x00034770 File Offset: 0x00032970
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060023B0 RID: 9136 RVA: 0x0003478D File Offset: 0x0003298D
		internal void Set(ref QueryOffersCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
