using System;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x020004A9 RID: 1193
	public struct RequestPermissionsCallbackInfo : ICallbackInfo
	{
		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06001F2B RID: 7979 RVA: 0x0002D9F1 File Offset: 0x0002BBF1
		// (set) Token: 0x06001F2C RID: 7980 RVA: 0x0002D9F9 File Offset: 0x0002BBF9
		public Result ResultCode { get; set; }

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06001F2D RID: 7981 RVA: 0x0002DA02 File Offset: 0x0002BC02
		// (set) Token: 0x06001F2E RID: 7982 RVA: 0x0002DA0A File Offset: 0x0002BC0A
		public object ClientData { get; set; }

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06001F2F RID: 7983 RVA: 0x0002DA13 File Offset: 0x0002BC13
		// (set) Token: 0x06001F30 RID: 7984 RVA: 0x0002DA1B File Offset: 0x0002BC1B
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x06001F31 RID: 7985 RVA: 0x0002DA24 File Offset: 0x0002BC24
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06001F32 RID: 7986 RVA: 0x0002DA41 File Offset: 0x0002BC41
		internal void Set(ref RequestPermissionsCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
