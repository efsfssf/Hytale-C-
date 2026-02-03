using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005EF RID: 1519
	public struct LoginCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x0600276D RID: 10093 RVA: 0x0003A5C2 File Offset: 0x000387C2
		// (set) Token: 0x0600276E RID: 10094 RVA: 0x0003A5CA File Offset: 0x000387CA
		public Result ResultCode { get; set; }

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x0600276F RID: 10095 RVA: 0x0003A5D3 File Offset: 0x000387D3
		// (set) Token: 0x06002770 RID: 10096 RVA: 0x0003A5DB File Offset: 0x000387DB
		public object ClientData { get; set; }

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x06002771 RID: 10097 RVA: 0x0003A5E4 File Offset: 0x000387E4
		// (set) Token: 0x06002772 RID: 10098 RVA: 0x0003A5EC File Offset: 0x000387EC
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x06002773 RID: 10099 RVA: 0x0003A5F5 File Offset: 0x000387F5
		// (set) Token: 0x06002774 RID: 10100 RVA: 0x0003A5FD File Offset: 0x000387FD
		public ContinuanceToken ContinuanceToken { get; set; }

		// Token: 0x06002775 RID: 10101 RVA: 0x0003A608 File Offset: 0x00038808
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x0003A625 File Offset: 0x00038825
		internal void Set(ref LoginCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.ContinuanceToken = other.ContinuanceToken;
		}
	}
}
