using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005D7 RID: 1495
	public struct CreateUserCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x060026D5 RID: 9941 RVA: 0x000397A1 File Offset: 0x000379A1
		// (set) Token: 0x060026D6 RID: 9942 RVA: 0x000397A9 File Offset: 0x000379A9
		public Result ResultCode { get; set; }

		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x060026D7 RID: 9943 RVA: 0x000397B2 File Offset: 0x000379B2
		// (set) Token: 0x060026D8 RID: 9944 RVA: 0x000397BA File Offset: 0x000379BA
		public object ClientData { get; set; }

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x060026D9 RID: 9945 RVA: 0x000397C3 File Offset: 0x000379C3
		// (set) Token: 0x060026DA RID: 9946 RVA: 0x000397CB File Offset: 0x000379CB
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x060026DB RID: 9947 RVA: 0x000397D4 File Offset: 0x000379D4
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060026DC RID: 9948 RVA: 0x000397F1 File Offset: 0x000379F1
		internal void Set(ref CreateUserCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
