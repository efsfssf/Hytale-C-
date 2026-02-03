using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000551 RID: 1361
	public struct QueryEntitlementsCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x06002367 RID: 9063 RVA: 0x000340E6 File Offset: 0x000322E6
		// (set) Token: 0x06002368 RID: 9064 RVA: 0x000340EE File Offset: 0x000322EE
		public Result ResultCode { get; set; }

		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x06002369 RID: 9065 RVA: 0x000340F7 File Offset: 0x000322F7
		// (set) Token: 0x0600236A RID: 9066 RVA: 0x000340FF File Offset: 0x000322FF
		public object ClientData { get; set; }

		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x0600236B RID: 9067 RVA: 0x00034108 File Offset: 0x00032308
		// (set) Token: 0x0600236C RID: 9068 RVA: 0x00034110 File Offset: 0x00032310
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x0600236D RID: 9069 RVA: 0x0003411C File Offset: 0x0003231C
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600236E RID: 9070 RVA: 0x00034139 File Offset: 0x00032339
		internal void Set(ref QueryEntitlementsCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
