using System;

namespace Epic.OnlineServices.UI
{
	// Token: 0x0200008A RID: 138
	public struct ShowFriendsCallbackInfo : ICallbackInfo
	{
		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060005A8 RID: 1448 RVA: 0x00007E66 File Offset: 0x00006066
		// (set) Token: 0x060005A9 RID: 1449 RVA: 0x00007E6E File Offset: 0x0000606E
		public Result ResultCode { get; set; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x00007E77 File Offset: 0x00006077
		// (set) Token: 0x060005AB RID: 1451 RVA: 0x00007E7F File Offset: 0x0000607F
		public object ClientData { get; set; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x00007E88 File Offset: 0x00006088
		// (set) Token: 0x060005AD RID: 1453 RVA: 0x00007E90 File Offset: 0x00006090
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x060005AE RID: 1454 RVA: 0x00007E9C File Offset: 0x0000609C
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00007EB9 File Offset: 0x000060B9
		internal void Set(ref ShowFriendsCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
