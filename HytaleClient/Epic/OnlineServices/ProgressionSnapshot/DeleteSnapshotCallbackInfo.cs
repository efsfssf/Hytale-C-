using System;

namespace Epic.OnlineServices.ProgressionSnapshot
{
	// Token: 0x020002A9 RID: 681
	public struct DeleteSnapshotCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001300 RID: 4864 RVA: 0x0001BB15 File Offset: 0x00019D15
		// (set) Token: 0x06001301 RID: 4865 RVA: 0x0001BB1D File Offset: 0x00019D1D
		public Result ResultCode { get; set; }

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06001302 RID: 4866 RVA: 0x0001BB26 File Offset: 0x00019D26
		// (set) Token: 0x06001303 RID: 4867 RVA: 0x0001BB2E File Offset: 0x00019D2E
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06001304 RID: 4868 RVA: 0x0001BB37 File Offset: 0x00019D37
		// (set) Token: 0x06001305 RID: 4869 RVA: 0x0001BB3F File Offset: 0x00019D3F
		public object ClientData { get; set; }

		// Token: 0x06001306 RID: 4870 RVA: 0x0001BB48 File Offset: 0x00019D48
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x0001BB65 File Offset: 0x00019D65
		internal void Set(ref DeleteSnapshotCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.LocalUserId = other.LocalUserId;
			this.ClientData = other.ClientData;
		}
	}
}
