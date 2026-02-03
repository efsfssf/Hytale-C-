using System;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004F3 RID: 1267
	public struct QueryFriendsCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x060020D8 RID: 8408 RVA: 0x000300C0 File Offset: 0x0002E2C0
		// (set) Token: 0x060020D9 RID: 8409 RVA: 0x000300C8 File Offset: 0x0002E2C8
		public Result ResultCode { get; set; }

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x060020DA RID: 8410 RVA: 0x000300D1 File Offset: 0x0002E2D1
		// (set) Token: 0x060020DB RID: 8411 RVA: 0x000300D9 File Offset: 0x0002E2D9
		public object ClientData { get; set; }

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x060020DC RID: 8412 RVA: 0x000300E2 File Offset: 0x0002E2E2
		// (set) Token: 0x060020DD RID: 8413 RVA: 0x000300EA File Offset: 0x0002E2EA
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x060020DE RID: 8414 RVA: 0x000300F4 File Offset: 0x0002E2F4
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x00030111 File Offset: 0x0002E311
		internal void Set(ref QueryFriendsCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
