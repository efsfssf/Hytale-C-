using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200043F RID: 1087
	public struct QueryInvitesCallbackInfo : ICallbackInfo
	{
		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06001C89 RID: 7305 RVA: 0x00029B17 File Offset: 0x00027D17
		// (set) Token: 0x06001C8A RID: 7306 RVA: 0x00029B1F File Offset: 0x00027D1F
		public Result ResultCode { get; set; }

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x06001C8B RID: 7307 RVA: 0x00029B28 File Offset: 0x00027D28
		// (set) Token: 0x06001C8C RID: 7308 RVA: 0x00029B30 File Offset: 0x00027D30
		public object ClientData { get; set; }

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06001C8D RID: 7309 RVA: 0x00029B39 File Offset: 0x00027D39
		// (set) Token: 0x06001C8E RID: 7310 RVA: 0x00029B41 File Offset: 0x00027D41
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x06001C8F RID: 7311 RVA: 0x00029B4C File Offset: 0x00027D4C
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x00029B69 File Offset: 0x00027D69
		internal void Set(ref QueryInvitesCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
