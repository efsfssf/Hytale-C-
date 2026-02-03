using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000104 RID: 260
	public struct EndSessionCallbackInfo : ICallbackInfo
	{
		// Token: 0x170001CA RID: 458
		// (get) Token: 0x0600088D RID: 2189 RVA: 0x0000C7E1 File Offset: 0x0000A9E1
		// (set) Token: 0x0600088E RID: 2190 RVA: 0x0000C7E9 File Offset: 0x0000A9E9
		public Result ResultCode { get; set; }

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600088F RID: 2191 RVA: 0x0000C7F2 File Offset: 0x0000A9F2
		// (set) Token: 0x06000890 RID: 2192 RVA: 0x0000C7FA File Offset: 0x0000A9FA
		public object ClientData { get; set; }

		// Token: 0x06000891 RID: 2193 RVA: 0x0000C804 File Offset: 0x0000AA04
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x0000C821 File Offset: 0x0000AA21
		internal void Set(ref EndSessionCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}
	}
}
