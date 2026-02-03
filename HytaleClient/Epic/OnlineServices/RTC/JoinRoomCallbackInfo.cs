using System;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001B7 RID: 439
	public struct JoinRoomCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x00012A1F File Offset: 0x00010C1F
		// (set) Token: 0x06000CB9 RID: 3257 RVA: 0x00012A27 File Offset: 0x00010C27
		public Result ResultCode { get; set; }

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000CBA RID: 3258 RVA: 0x00012A30 File Offset: 0x00010C30
		// (set) Token: 0x06000CBB RID: 3259 RVA: 0x00012A38 File Offset: 0x00010C38
		public object ClientData { get; set; }

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000CBC RID: 3260 RVA: 0x00012A41 File Offset: 0x00010C41
		// (set) Token: 0x06000CBD RID: 3261 RVA: 0x00012A49 File Offset: 0x00010C49
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000CBE RID: 3262 RVA: 0x00012A52 File Offset: 0x00010C52
		// (set) Token: 0x06000CBF RID: 3263 RVA: 0x00012A5A File Offset: 0x00010C5A
		public Utf8String RoomName { get; set; }

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x00012A63 File Offset: 0x00010C63
		// (set) Token: 0x06000CC1 RID: 3265 RVA: 0x00012A6B File Offset: 0x00010C6B
		public Option[] RoomOptions { get; set; }

		// Token: 0x06000CC2 RID: 3266 RVA: 0x00012A74 File Offset: 0x00010C74
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x00012A94 File Offset: 0x00010C94
		internal void Set(ref JoinRoomCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.RoomOptions = other.RoomOptions;
		}
	}
}
