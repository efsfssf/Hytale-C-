using System;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x0200065F RID: 1631
	public struct QueryIdTokenCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x06002A2D RID: 10797 RVA: 0x0003DE42 File Offset: 0x0003C042
		// (set) Token: 0x06002A2E RID: 10798 RVA: 0x0003DE4A File Offset: 0x0003C04A
		public Result ResultCode { get; set; }

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x06002A2F RID: 10799 RVA: 0x0003DE53 File Offset: 0x0003C053
		// (set) Token: 0x06002A30 RID: 10800 RVA: 0x0003DE5B File Offset: 0x0003C05B
		public object ClientData { get; set; }

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x06002A31 RID: 10801 RVA: 0x0003DE64 File Offset: 0x0003C064
		// (set) Token: 0x06002A32 RID: 10802 RVA: 0x0003DE6C File Offset: 0x0003C06C
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x06002A33 RID: 10803 RVA: 0x0003DE75 File Offset: 0x0003C075
		// (set) Token: 0x06002A34 RID: 10804 RVA: 0x0003DE7D File Offset: 0x0003C07D
		public EpicAccountId TargetAccountId { get; set; }

		// Token: 0x06002A35 RID: 10805 RVA: 0x0003DE88 File Offset: 0x0003C088
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06002A36 RID: 10806 RVA: 0x0003DEA5 File Offset: 0x0003C0A5
		internal void Set(ref QueryIdTokenCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetAccountId = other.TargetAccountId;
		}
	}
}
