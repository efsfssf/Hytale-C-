using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000110 RID: 272
	public struct JoinSessionCallbackInfo : ICallbackInfo
	{
		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060008CD RID: 2253 RVA: 0x0000CD8E File Offset: 0x0000AF8E
		// (set) Token: 0x060008CE RID: 2254 RVA: 0x0000CD96 File Offset: 0x0000AF96
		public Result ResultCode { get; set; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060008CF RID: 2255 RVA: 0x0000CD9F File Offset: 0x0000AF9F
		// (set) Token: 0x060008D0 RID: 2256 RVA: 0x0000CDA7 File Offset: 0x0000AFA7
		public object ClientData { get; set; }

		// Token: 0x060008D1 RID: 2257 RVA: 0x0000CDB0 File Offset: 0x0000AFB0
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x0000CDCD File Offset: 0x0000AFCD
		internal void Set(ref JoinSessionCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}
	}
}
