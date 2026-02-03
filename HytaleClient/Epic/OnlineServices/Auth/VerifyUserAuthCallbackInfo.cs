using System;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000669 RID: 1641
	public struct VerifyUserAuthCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000C86 RID: 3206
		// (get) Token: 0x06002ABB RID: 10939 RVA: 0x0003EDC9 File Offset: 0x0003CFC9
		// (set) Token: 0x06002ABC RID: 10940 RVA: 0x0003EDD1 File Offset: 0x0003CFD1
		public Result ResultCode { get; set; }

		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x06002ABD RID: 10941 RVA: 0x0003EDDA File Offset: 0x0003CFDA
		// (set) Token: 0x06002ABE RID: 10942 RVA: 0x0003EDE2 File Offset: 0x0003CFE2
		public object ClientData { get; set; }

		// Token: 0x06002ABF RID: 10943 RVA: 0x0003EDEC File Offset: 0x0003CFEC
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06002AC0 RID: 10944 RVA: 0x0003EE09 File Offset: 0x0003D009
		internal void Set(ref VerifyUserAuthCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}
	}
}
