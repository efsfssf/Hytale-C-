using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000140 RID: 320
	public struct RejectInviteCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700020C RID: 524
		// (get) Token: 0x060009B7 RID: 2487 RVA: 0x0000D7B2 File Offset: 0x0000B9B2
		// (set) Token: 0x060009B8 RID: 2488 RVA: 0x0000D7BA File Offset: 0x0000B9BA
		public Result ResultCode { get; set; }

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x060009B9 RID: 2489 RVA: 0x0000D7C3 File Offset: 0x0000B9C3
		// (set) Token: 0x060009BA RID: 2490 RVA: 0x0000D7CB File Offset: 0x0000B9CB
		public object ClientData { get; set; }

		// Token: 0x060009BB RID: 2491 RVA: 0x0000D7D4 File Offset: 0x0000B9D4
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x0000D7F1 File Offset: 0x0000B9F1
		internal void Set(ref RejectInviteCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}
	}
}
