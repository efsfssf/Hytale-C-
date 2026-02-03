using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005DD RID: 1501
	public struct DeleteDeviceIdCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x060026FB RID: 9979 RVA: 0x00039B14 File Offset: 0x00037D14
		// (set) Token: 0x060026FC RID: 9980 RVA: 0x00039B1C File Offset: 0x00037D1C
		public Result ResultCode { get; set; }

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x060026FD RID: 9981 RVA: 0x00039B25 File Offset: 0x00037D25
		// (set) Token: 0x060026FE RID: 9982 RVA: 0x00039B2D File Offset: 0x00037D2D
		public object ClientData { get; set; }

		// Token: 0x060026FF RID: 9983 RVA: 0x00039B38 File Offset: 0x00037D38
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06002700 RID: 9984 RVA: 0x00039B55 File Offset: 0x00037D55
		internal void Set(ref DeleteDeviceIdCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}
	}
}
