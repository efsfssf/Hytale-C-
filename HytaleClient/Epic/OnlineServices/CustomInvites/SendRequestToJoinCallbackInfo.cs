using System;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005BC RID: 1468
	public struct SendRequestToJoinCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000B1B RID: 2843
		// (get) Token: 0x06002632 RID: 9778 RVA: 0x00038290 File Offset: 0x00036490
		// (set) Token: 0x06002633 RID: 9779 RVA: 0x00038298 File Offset: 0x00036498
		public Result ResultCode { get; set; }

		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x06002634 RID: 9780 RVA: 0x000382A1 File Offset: 0x000364A1
		// (set) Token: 0x06002635 RID: 9781 RVA: 0x000382A9 File Offset: 0x000364A9
		public object ClientData { get; set; }

		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x06002636 RID: 9782 RVA: 0x000382B2 File Offset: 0x000364B2
		// (set) Token: 0x06002637 RID: 9783 RVA: 0x000382BA File Offset: 0x000364BA
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x06002638 RID: 9784 RVA: 0x000382C3 File Offset: 0x000364C3
		// (set) Token: 0x06002639 RID: 9785 RVA: 0x000382CB File Offset: 0x000364CB
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x0600263A RID: 9786 RVA: 0x000382D4 File Offset: 0x000364D4
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600263B RID: 9787 RVA: 0x000382F1 File Offset: 0x000364F1
		internal void Set(ref SendRequestToJoinCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}
	}
}
