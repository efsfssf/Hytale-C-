using System;

namespace Epic.OnlineServices.ProgressionSnapshot
{
	// Token: 0x020002B4 RID: 692
	public struct SubmitSnapshotCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06001338 RID: 4920 RVA: 0x0001BFAB File Offset: 0x0001A1AB
		// (set) Token: 0x06001339 RID: 4921 RVA: 0x0001BFB3 File Offset: 0x0001A1B3
		public Result ResultCode { get; set; }

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x0600133A RID: 4922 RVA: 0x0001BFBC File Offset: 0x0001A1BC
		// (set) Token: 0x0600133B RID: 4923 RVA: 0x0001BFC4 File Offset: 0x0001A1C4
		public uint SnapshotId { get; set; }

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x0600133C RID: 4924 RVA: 0x0001BFCD File Offset: 0x0001A1CD
		// (set) Token: 0x0600133D RID: 4925 RVA: 0x0001BFD5 File Offset: 0x0001A1D5
		public object ClientData { get; set; }

		// Token: 0x0600133E RID: 4926 RVA: 0x0001BFE0 File Offset: 0x0001A1E0
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x0001BFFD File Offset: 0x0001A1FD
		internal void Set(ref SubmitSnapshotCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.SnapshotId = other.SnapshotId;
			this.ClientData = other.ClientData;
		}
	}
}
