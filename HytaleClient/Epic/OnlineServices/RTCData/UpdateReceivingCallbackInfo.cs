using System;

namespace Epic.OnlineServices.RTCData
{
	// Token: 0x020001EE RID: 494
	public struct UpdateReceivingCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000E40 RID: 3648 RVA: 0x00014DA3 File Offset: 0x00012FA3
		// (set) Token: 0x06000E41 RID: 3649 RVA: 0x00014DAB File Offset: 0x00012FAB
		public Result ResultCode { get; set; }

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000E42 RID: 3650 RVA: 0x00014DB4 File Offset: 0x00012FB4
		// (set) Token: 0x06000E43 RID: 3651 RVA: 0x00014DBC File Offset: 0x00012FBC
		public object ClientData { get; set; }

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000E44 RID: 3652 RVA: 0x00014DC5 File Offset: 0x00012FC5
		// (set) Token: 0x06000E45 RID: 3653 RVA: 0x00014DCD File Offset: 0x00012FCD
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000E46 RID: 3654 RVA: 0x00014DD6 File Offset: 0x00012FD6
		// (set) Token: 0x06000E47 RID: 3655 RVA: 0x00014DDE File Offset: 0x00012FDE
		public Utf8String RoomName { get; set; }

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000E48 RID: 3656 RVA: 0x00014DE7 File Offset: 0x00012FE7
		// (set) Token: 0x06000E49 RID: 3657 RVA: 0x00014DEF File Offset: 0x00012FEF
		public ProductUserId ParticipantId { get; set; }

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000E4A RID: 3658 RVA: 0x00014DF8 File Offset: 0x00012FF8
		// (set) Token: 0x06000E4B RID: 3659 RVA: 0x00014E00 File Offset: 0x00013000
		public bool DataEnabled { get; set; }

		// Token: 0x06000E4C RID: 3660 RVA: 0x00014E0C File Offset: 0x0001300C
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x00014E2C File Offset: 0x0001302C
		internal void Set(ref UpdateReceivingCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.ParticipantId = other.ParticipantId;
			this.DataEnabled = other.DataEnabled;
		}
	}
}
