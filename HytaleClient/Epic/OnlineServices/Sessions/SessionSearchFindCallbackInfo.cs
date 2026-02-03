using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000176 RID: 374
	public struct SessionSearchFindCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000B11 RID: 2833 RVA: 0x0000FC6D File Offset: 0x0000DE6D
		// (set) Token: 0x06000B12 RID: 2834 RVA: 0x0000FC75 File Offset: 0x0000DE75
		public Result ResultCode { get; set; }

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000B13 RID: 2835 RVA: 0x0000FC7E File Offset: 0x0000DE7E
		// (set) Token: 0x06000B14 RID: 2836 RVA: 0x0000FC86 File Offset: 0x0000DE86
		public object ClientData { get; set; }

		// Token: 0x06000B15 RID: 2837 RVA: 0x0000FC90 File Offset: 0x0000DE90
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x0000FCAD File Offset: 0x0000DEAD
		internal void Set(ref SessionSearchFindCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}
	}
}
