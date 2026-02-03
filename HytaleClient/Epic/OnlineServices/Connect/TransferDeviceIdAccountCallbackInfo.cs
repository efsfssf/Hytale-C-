using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x0200061B RID: 1563
	public struct TransferDeviceIdAccountCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x06002863 RID: 10339 RVA: 0x0003B31F File Offset: 0x0003951F
		// (set) Token: 0x06002864 RID: 10340 RVA: 0x0003B327 File Offset: 0x00039527
		public Result ResultCode { get; set; }

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x06002865 RID: 10341 RVA: 0x0003B330 File Offset: 0x00039530
		// (set) Token: 0x06002866 RID: 10342 RVA: 0x0003B338 File Offset: 0x00039538
		public object ClientData { get; set; }

		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x06002867 RID: 10343 RVA: 0x0003B341 File Offset: 0x00039541
		// (set) Token: 0x06002868 RID: 10344 RVA: 0x0003B349 File Offset: 0x00039549
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x06002869 RID: 10345 RVA: 0x0003B354 File Offset: 0x00039554
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600286A RID: 10346 RVA: 0x0003B371 File Offset: 0x00039571
		internal void Set(ref TransferDeviceIdAccountCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
