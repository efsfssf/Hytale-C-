using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005D3 RID: 1491
	public struct CreateDeviceIdCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x060026C0 RID: 9920 RVA: 0x000395CD File Offset: 0x000377CD
		// (set) Token: 0x060026C1 RID: 9921 RVA: 0x000395D5 File Offset: 0x000377D5
		public Result ResultCode { get; set; }

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x060026C2 RID: 9922 RVA: 0x000395DE File Offset: 0x000377DE
		// (set) Token: 0x060026C3 RID: 9923 RVA: 0x000395E6 File Offset: 0x000377E6
		public object ClientData { get; set; }

		// Token: 0x060026C4 RID: 9924 RVA: 0x000395F0 File Offset: 0x000377F0
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060026C5 RID: 9925 RVA: 0x0003960D File Offset: 0x0003780D
		internal void Set(ref CreateDeviceIdCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}
	}
}
