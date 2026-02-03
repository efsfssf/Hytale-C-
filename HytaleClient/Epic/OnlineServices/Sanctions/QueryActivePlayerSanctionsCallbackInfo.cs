using System;

namespace Epic.OnlineServices.Sanctions
{
	// Token: 0x020001A5 RID: 421
	public struct QueryActivePlayerSanctionsCallbackInfo : ICallbackInfo
	{
		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000C30 RID: 3120 RVA: 0x00011BDA File Offset: 0x0000FDDA
		// (set) Token: 0x06000C31 RID: 3121 RVA: 0x00011BE2 File Offset: 0x0000FDE2
		public Result ResultCode { get; set; }

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000C32 RID: 3122 RVA: 0x00011BEB File Offset: 0x0000FDEB
		// (set) Token: 0x06000C33 RID: 3123 RVA: 0x00011BF3 File Offset: 0x0000FDF3
		public object ClientData { get; set; }

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000C34 RID: 3124 RVA: 0x00011BFC File Offset: 0x0000FDFC
		// (set) Token: 0x06000C35 RID: 3125 RVA: 0x00011C04 File Offset: 0x0000FE04
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000C36 RID: 3126 RVA: 0x00011C0D File Offset: 0x0000FE0D
		// (set) Token: 0x06000C37 RID: 3127 RVA: 0x00011C15 File Offset: 0x0000FE15
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x06000C38 RID: 3128 RVA: 0x00011C20 File Offset: 0x0000FE20
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x00011C3D File Offset: 0x0000FE3D
		internal void Set(ref QueryActivePlayerSanctionsCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.TargetUserId = other.TargetUserId;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
