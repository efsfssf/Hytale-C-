using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000565 RID: 1381
	public struct QueryOwnershipTokenCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x06002408 RID: 9224 RVA: 0x00035087 File Offset: 0x00033287
		// (set) Token: 0x06002409 RID: 9225 RVA: 0x0003508F File Offset: 0x0003328F
		public Result ResultCode { get; set; }

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x0600240A RID: 9226 RVA: 0x00035098 File Offset: 0x00033298
		// (set) Token: 0x0600240B RID: 9227 RVA: 0x000350A0 File Offset: 0x000332A0
		public object ClientData { get; set; }

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x0600240C RID: 9228 RVA: 0x000350A9 File Offset: 0x000332A9
		// (set) Token: 0x0600240D RID: 9229 RVA: 0x000350B1 File Offset: 0x000332B1
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x0600240E RID: 9230 RVA: 0x000350BA File Offset: 0x000332BA
		// (set) Token: 0x0600240F RID: 9231 RVA: 0x000350C2 File Offset: 0x000332C2
		public Utf8String OwnershipToken { get; set; }

		// Token: 0x06002410 RID: 9232 RVA: 0x000350CC File Offset: 0x000332CC
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06002411 RID: 9233 RVA: 0x000350E9 File Offset: 0x000332E9
		internal void Set(ref QueryOwnershipTokenCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.OwnershipToken = other.OwnershipToken;
		}
	}
}
