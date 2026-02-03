using System;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x0200063C RID: 1596
	public struct LinkAccountCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x06002952 RID: 10578 RVA: 0x0003CEAC File Offset: 0x0003B0AC
		// (set) Token: 0x06002953 RID: 10579 RVA: 0x0003CEB4 File Offset: 0x0003B0B4
		public Result ResultCode { get; set; }

		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x06002954 RID: 10580 RVA: 0x0003CEBD File Offset: 0x0003B0BD
		// (set) Token: 0x06002955 RID: 10581 RVA: 0x0003CEC5 File Offset: 0x0003B0C5
		public object ClientData { get; set; }

		// Token: 0x17000C0A RID: 3082
		// (get) Token: 0x06002956 RID: 10582 RVA: 0x0003CECE File Offset: 0x0003B0CE
		// (set) Token: 0x06002957 RID: 10583 RVA: 0x0003CED6 File Offset: 0x0003B0D6
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x06002958 RID: 10584 RVA: 0x0003CEDF File Offset: 0x0003B0DF
		// (set) Token: 0x06002959 RID: 10585 RVA: 0x0003CEE7 File Offset: 0x0003B0E7
		public PinGrantInfo? PinGrantInfo { get; set; }

		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x0600295A RID: 10586 RVA: 0x0003CEF0 File Offset: 0x0003B0F0
		// (set) Token: 0x0600295B RID: 10587 RVA: 0x0003CEF8 File Offset: 0x0003B0F8
		public EpicAccountId SelectedAccountId { get; set; }

		// Token: 0x0600295C RID: 10588 RVA: 0x0003CF04 File Offset: 0x0003B104
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600295D RID: 10589 RVA: 0x0003CF24 File Offset: 0x0003B124
		internal void Set(ref LinkAccountCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.PinGrantInfo = other.PinGrantInfo;
			this.SelectedAccountId = other.SelectedAccountId;
		}
	}
}
