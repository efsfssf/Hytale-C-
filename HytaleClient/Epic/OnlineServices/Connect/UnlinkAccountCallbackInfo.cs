using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x0200061F RID: 1567
	public struct UnlinkAccountCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x06002882 RID: 10370 RVA: 0x0003B60B File Offset: 0x0003980B
		// (set) Token: 0x06002883 RID: 10371 RVA: 0x0003B613 File Offset: 0x00039813
		public Result ResultCode { get; set; }

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x06002884 RID: 10372 RVA: 0x0003B61C File Offset: 0x0003981C
		// (set) Token: 0x06002885 RID: 10373 RVA: 0x0003B624 File Offset: 0x00039824
		public object ClientData { get; set; }

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x06002886 RID: 10374 RVA: 0x0003B62D File Offset: 0x0003982D
		// (set) Token: 0x06002887 RID: 10375 RVA: 0x0003B635 File Offset: 0x00039835
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x06002888 RID: 10376 RVA: 0x0003B640 File Offset: 0x00039840
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06002889 RID: 10377 RVA: 0x0003B65D File Offset: 0x0003985D
		internal void Set(ref UnlinkAccountCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
