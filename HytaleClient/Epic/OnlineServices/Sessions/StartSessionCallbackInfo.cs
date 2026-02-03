using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000189 RID: 393
	public struct StartSessionCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000B89 RID: 2953 RVA: 0x00010DF1 File Offset: 0x0000EFF1
		// (set) Token: 0x06000B8A RID: 2954 RVA: 0x00010DF9 File Offset: 0x0000EFF9
		public Result ResultCode { get; set; }

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000B8B RID: 2955 RVA: 0x00010E02 File Offset: 0x0000F002
		// (set) Token: 0x06000B8C RID: 2956 RVA: 0x00010E0A File Offset: 0x0000F00A
		public object ClientData { get; set; }

		// Token: 0x06000B8D RID: 2957 RVA: 0x00010E14 File Offset: 0x0000F014
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x00010E31 File Offset: 0x0000F031
		internal void Set(ref StartSessionCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}
	}
}
