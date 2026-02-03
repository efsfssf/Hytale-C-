using System;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004FB RID: 1275
	public struct SendInviteCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x06002111 RID: 8465 RVA: 0x0003062A File Offset: 0x0002E82A
		// (set) Token: 0x06002112 RID: 8466 RVA: 0x00030632 File Offset: 0x0002E832
		public Result ResultCode { get; set; }

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06002113 RID: 8467 RVA: 0x0003063B File Offset: 0x0002E83B
		// (set) Token: 0x06002114 RID: 8468 RVA: 0x00030643 File Offset: 0x0002E843
		public object ClientData { get; set; }

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06002115 RID: 8469 RVA: 0x0003064C File Offset: 0x0002E84C
		// (set) Token: 0x06002116 RID: 8470 RVA: 0x00030654 File Offset: 0x0002E854
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x06002117 RID: 8471 RVA: 0x0003065D File Offset: 0x0002E85D
		// (set) Token: 0x06002118 RID: 8472 RVA: 0x00030665 File Offset: 0x0002E865
		public EpicAccountId TargetUserId { get; set; }

		// Token: 0x06002119 RID: 8473 RVA: 0x00030670 File Offset: 0x0002E870
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x0003068D File Offset: 0x0002E88D
		internal void Set(ref SendInviteCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}
	}
}
