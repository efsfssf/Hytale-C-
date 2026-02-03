using System;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001BC RID: 444
	public struct LeaveRoomCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000CEE RID: 3310 RVA: 0x00012FA3 File Offset: 0x000111A3
		// (set) Token: 0x06000CEF RID: 3311 RVA: 0x00012FAB File Offset: 0x000111AB
		public Result ResultCode { get; set; }

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000CF0 RID: 3312 RVA: 0x00012FB4 File Offset: 0x000111B4
		// (set) Token: 0x06000CF1 RID: 3313 RVA: 0x00012FBC File Offset: 0x000111BC
		public object ClientData { get; set; }

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000CF2 RID: 3314 RVA: 0x00012FC5 File Offset: 0x000111C5
		// (set) Token: 0x06000CF3 RID: 3315 RVA: 0x00012FCD File Offset: 0x000111CD
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000CF4 RID: 3316 RVA: 0x00012FD6 File Offset: 0x000111D6
		// (set) Token: 0x06000CF5 RID: 3317 RVA: 0x00012FDE File Offset: 0x000111DE
		public Utf8String RoomName { get; set; }

		// Token: 0x06000CF6 RID: 3318 RVA: 0x00012FE8 File Offset: 0x000111E8
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x00013005 File Offset: 0x00011205
		internal void Set(ref LeaveRoomCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
		}
	}
}
