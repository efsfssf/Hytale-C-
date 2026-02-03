using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x02000617 RID: 1559
	public struct QueryProductUserIdMappingsCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x06002844 RID: 10308 RVA: 0x0003B03F File Offset: 0x0003923F
		// (set) Token: 0x06002845 RID: 10309 RVA: 0x0003B047 File Offset: 0x00039247
		public Result ResultCode { get; set; }

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x06002846 RID: 10310 RVA: 0x0003B050 File Offset: 0x00039250
		// (set) Token: 0x06002847 RID: 10311 RVA: 0x0003B058 File Offset: 0x00039258
		public object ClientData { get; set; }

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x06002848 RID: 10312 RVA: 0x0003B061 File Offset: 0x00039261
		// (set) Token: 0x06002849 RID: 10313 RVA: 0x0003B069 File Offset: 0x00039269
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x0600284A RID: 10314 RVA: 0x0003B074 File Offset: 0x00039274
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600284B RID: 10315 RVA: 0x0003B091 File Offset: 0x00039291
		internal void Set(ref QueryProductUserIdMappingsCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
