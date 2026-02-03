using System;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000649 RID: 1609
	public struct LogoutCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x060029BF RID: 10687 RVA: 0x0003D9CF File Offset: 0x0003BBCF
		// (set) Token: 0x060029C0 RID: 10688 RVA: 0x0003D9D7 File Offset: 0x0003BBD7
		public Result ResultCode { get; set; }

		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x060029C1 RID: 10689 RVA: 0x0003D9E0 File Offset: 0x0003BBE0
		// (set) Token: 0x060029C2 RID: 10690 RVA: 0x0003D9E8 File Offset: 0x0003BBE8
		public object ClientData { get; set; }

		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x060029C3 RID: 10691 RVA: 0x0003D9F1 File Offset: 0x0003BBF1
		// (set) Token: 0x060029C4 RID: 10692 RVA: 0x0003D9F9 File Offset: 0x0003BBF9
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x060029C5 RID: 10693 RVA: 0x0003DA04 File Offset: 0x0003BC04
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060029C6 RID: 10694 RVA: 0x0003DA21 File Offset: 0x0003BC21
		internal void Set(ref LogoutCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
