using System;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x020004AD RID: 1197
	public struct UpdateParentEmailCallbackInfo : ICallbackInfo
	{
		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06001F47 RID: 8007 RVA: 0x0002DC92 File Offset: 0x0002BE92
		// (set) Token: 0x06001F48 RID: 8008 RVA: 0x0002DC9A File Offset: 0x0002BE9A
		public Result ResultCode { get; set; }

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06001F49 RID: 8009 RVA: 0x0002DCA3 File Offset: 0x0002BEA3
		// (set) Token: 0x06001F4A RID: 8010 RVA: 0x0002DCAB File Offset: 0x0002BEAB
		public object ClientData { get; set; }

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06001F4B RID: 8011 RVA: 0x0002DCB4 File Offset: 0x0002BEB4
		// (set) Token: 0x06001F4C RID: 8012 RVA: 0x0002DCBC File Offset: 0x0002BEBC
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x06001F4D RID: 8013 RVA: 0x0002DCC8 File Offset: 0x0002BEC8
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06001F4E RID: 8014 RVA: 0x0002DCE5 File Offset: 0x0002BEE5
		internal void Set(ref UpdateParentEmailCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
