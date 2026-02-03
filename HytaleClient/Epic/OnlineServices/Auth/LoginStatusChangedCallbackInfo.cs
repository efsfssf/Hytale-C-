using System;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000647 RID: 1607
	public struct LoginStatusChangedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x060029A8 RID: 10664 RVA: 0x0003D79B File Offset: 0x0003B99B
		// (set) Token: 0x060029A9 RID: 10665 RVA: 0x0003D7A3 File Offset: 0x0003B9A3
		public object ClientData { get; set; }

		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x060029AA RID: 10666 RVA: 0x0003D7AC File Offset: 0x0003B9AC
		// (set) Token: 0x060029AB RID: 10667 RVA: 0x0003D7B4 File Offset: 0x0003B9B4
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x060029AC RID: 10668 RVA: 0x0003D7BD File Offset: 0x0003B9BD
		// (set) Token: 0x060029AD RID: 10669 RVA: 0x0003D7C5 File Offset: 0x0003B9C5
		public LoginStatus PrevStatus { get; set; }

		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x060029AE RID: 10670 RVA: 0x0003D7CE File Offset: 0x0003B9CE
		// (set) Token: 0x060029AF RID: 10671 RVA: 0x0003D7D6 File Offset: 0x0003B9D6
		public LoginStatus CurrentStatus { get; set; }

		// Token: 0x060029B0 RID: 10672 RVA: 0x0003D7E0 File Offset: 0x0003B9E0
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x060029B1 RID: 10673 RVA: 0x0003D7FB File Offset: 0x0003B9FB
		internal void Set(ref LoginStatusChangedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.PrevStatus = other.PrevStatus;
			this.CurrentStatus = other.CurrentStatus;
		}
	}
}
