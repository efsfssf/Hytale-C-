using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005F5 RID: 1525
	public struct LogoutCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x060027A4 RID: 10148 RVA: 0x0003AB13 File Offset: 0x00038D13
		// (set) Token: 0x060027A5 RID: 10149 RVA: 0x0003AB1B File Offset: 0x00038D1B
		public Result ResultCode { get; set; }

		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x060027A6 RID: 10150 RVA: 0x0003AB24 File Offset: 0x00038D24
		// (set) Token: 0x060027A7 RID: 10151 RVA: 0x0003AB2C File Offset: 0x00038D2C
		public object ClientData { get; set; }

		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x060027A8 RID: 10152 RVA: 0x0003AB35 File Offset: 0x00038D35
		// (set) Token: 0x060027A9 RID: 10153 RVA: 0x0003AB3D File Offset: 0x00038D3D
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x060027AA RID: 10154 RVA: 0x0003AB48 File Offset: 0x00038D48
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060027AB RID: 10155 RVA: 0x0003AB65 File Offset: 0x00038D65
		internal void Set(ref LogoutCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
