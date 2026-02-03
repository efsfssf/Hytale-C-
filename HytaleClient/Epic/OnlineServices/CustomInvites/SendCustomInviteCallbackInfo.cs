using System;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005B6 RID: 1462
	public struct SendCustomInviteCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x060025F3 RID: 9715 RVA: 0x00037BFF File Offset: 0x00035DFF
		// (set) Token: 0x060025F4 RID: 9716 RVA: 0x00037C07 File Offset: 0x00035E07
		public Result ResultCode { get; set; }

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x060025F5 RID: 9717 RVA: 0x00037C10 File Offset: 0x00035E10
		// (set) Token: 0x060025F6 RID: 9718 RVA: 0x00037C18 File Offset: 0x00035E18
		public object ClientData { get; set; }

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x060025F7 RID: 9719 RVA: 0x00037C21 File Offset: 0x00035E21
		// (set) Token: 0x060025F8 RID: 9720 RVA: 0x00037C29 File Offset: 0x00035E29
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x060025F9 RID: 9721 RVA: 0x00037C32 File Offset: 0x00035E32
		// (set) Token: 0x060025FA RID: 9722 RVA: 0x00037C3A File Offset: 0x00035E3A
		public ProductUserId[] TargetUserIds { get; set; }

		// Token: 0x060025FB RID: 9723 RVA: 0x00037C44 File Offset: 0x00035E44
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060025FC RID: 9724 RVA: 0x00037C61 File Offset: 0x00035E61
		internal void Set(ref SendCustomInviteCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserIds = other.TargetUserIds;
		}
	}
}
