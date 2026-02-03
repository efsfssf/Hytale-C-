using System;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x020002F3 RID: 755
	public struct DeleteFileCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x060014A3 RID: 5283 RVA: 0x0001E1ED File Offset: 0x0001C3ED
		// (set) Token: 0x060014A4 RID: 5284 RVA: 0x0001E1F5 File Offset: 0x0001C3F5
		public Result ResultCode { get; set; }

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x060014A5 RID: 5285 RVA: 0x0001E1FE File Offset: 0x0001C3FE
		// (set) Token: 0x060014A6 RID: 5286 RVA: 0x0001E206 File Offset: 0x0001C406
		public object ClientData { get; set; }

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x060014A7 RID: 5287 RVA: 0x0001E20F File Offset: 0x0001C40F
		// (set) Token: 0x060014A8 RID: 5288 RVA: 0x0001E217 File Offset: 0x0001C417
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x060014A9 RID: 5289 RVA: 0x0001E220 File Offset: 0x0001C420
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x0001E23D File Offset: 0x0001C43D
		internal void Set(ref DeleteFileCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
