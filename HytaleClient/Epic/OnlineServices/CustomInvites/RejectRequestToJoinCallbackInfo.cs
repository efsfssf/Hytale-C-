using System;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005AD RID: 1453
	public struct RejectRequestToJoinCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x060025A9 RID: 9641 RVA: 0x000374A6 File Offset: 0x000356A6
		// (set) Token: 0x060025AA RID: 9642 RVA: 0x000374AE File Offset: 0x000356AE
		public Result ResultCode { get; set; }

		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x060025AB RID: 9643 RVA: 0x000374B7 File Offset: 0x000356B7
		// (set) Token: 0x060025AC RID: 9644 RVA: 0x000374BF File Offset: 0x000356BF
		public object ClientData { get; set; }

		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x060025AD RID: 9645 RVA: 0x000374C8 File Offset: 0x000356C8
		// (set) Token: 0x060025AE RID: 9646 RVA: 0x000374D0 File Offset: 0x000356D0
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x060025AF RID: 9647 RVA: 0x000374D9 File Offset: 0x000356D9
		// (set) Token: 0x060025B0 RID: 9648 RVA: 0x000374E1 File Offset: 0x000356E1
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x060025B1 RID: 9649 RVA: 0x000374EC File Offset: 0x000356EC
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060025B2 RID: 9650 RVA: 0x00037509 File Offset: 0x00035709
		internal void Set(ref RejectRequestToJoinCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}
	}
}
