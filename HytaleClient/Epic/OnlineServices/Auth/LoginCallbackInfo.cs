using System;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000641 RID: 1601
	public struct LoginCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x06002979 RID: 10617 RVA: 0x0003D2A3 File Offset: 0x0003B4A3
		// (set) Token: 0x0600297A RID: 10618 RVA: 0x0003D2AB File Offset: 0x0003B4AB
		public Result ResultCode { get; set; }

		// Token: 0x17000C1A RID: 3098
		// (get) Token: 0x0600297B RID: 10619 RVA: 0x0003D2B4 File Offset: 0x0003B4B4
		// (set) Token: 0x0600297C RID: 10620 RVA: 0x0003D2BC File Offset: 0x0003B4BC
		public object ClientData { get; set; }

		// Token: 0x17000C1B RID: 3099
		// (get) Token: 0x0600297D RID: 10621 RVA: 0x0003D2C5 File Offset: 0x0003B4C5
		// (set) Token: 0x0600297E RID: 10622 RVA: 0x0003D2CD File Offset: 0x0003B4CD
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000C1C RID: 3100
		// (get) Token: 0x0600297F RID: 10623 RVA: 0x0003D2D6 File Offset: 0x0003B4D6
		// (set) Token: 0x06002980 RID: 10624 RVA: 0x0003D2DE File Offset: 0x0003B4DE
		public PinGrantInfo? PinGrantInfo { get; set; }

		// Token: 0x17000C1D RID: 3101
		// (get) Token: 0x06002981 RID: 10625 RVA: 0x0003D2E7 File Offset: 0x0003B4E7
		// (set) Token: 0x06002982 RID: 10626 RVA: 0x0003D2EF File Offset: 0x0003B4EF
		public ContinuanceToken ContinuanceToken { get; set; }

		// Token: 0x17000C1E RID: 3102
		// (get) Token: 0x06002983 RID: 10627 RVA: 0x0003D2F8 File Offset: 0x0003B4F8
		// (set) Token: 0x06002984 RID: 10628 RVA: 0x0003D300 File Offset: 0x0003B500
		internal AccountFeatureRestrictedInfo? AccountFeatureRestrictedInfo_DEPRECATED { get; set; }

		// Token: 0x17000C1F RID: 3103
		// (get) Token: 0x06002985 RID: 10629 RVA: 0x0003D309 File Offset: 0x0003B509
		// (set) Token: 0x06002986 RID: 10630 RVA: 0x0003D311 File Offset: 0x0003B511
		public EpicAccountId SelectedAccountId { get; set; }

		// Token: 0x06002987 RID: 10631 RVA: 0x0003D31C File Offset: 0x0003B51C
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06002988 RID: 10632 RVA: 0x0003D33C File Offset: 0x0003B53C
		internal void Set(ref LoginCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.PinGrantInfo = other.PinGrantInfo;
			this.ContinuanceToken = other.ContinuanceToken;
			this.AccountFeatureRestrictedInfo_DEPRECATED = other.AccountFeatureRestrictedInfo_DEPRECATED;
			this.SelectedAccountId = other.SelectedAccountId;
		}
	}
}
