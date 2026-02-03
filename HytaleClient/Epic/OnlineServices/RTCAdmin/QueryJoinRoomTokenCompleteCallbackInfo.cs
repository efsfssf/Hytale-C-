using System;

namespace Epic.OnlineServices.RTCAdmin
{
	// Token: 0x02000292 RID: 658
	public struct QueryJoinRoomTokenCompleteCallbackInfo : ICallbackInfo
	{
		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06001261 RID: 4705 RVA: 0x0001AB36 File Offset: 0x00018D36
		// (set) Token: 0x06001262 RID: 4706 RVA: 0x0001AB3E File Offset: 0x00018D3E
		public Result ResultCode { get; set; }

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06001263 RID: 4707 RVA: 0x0001AB47 File Offset: 0x00018D47
		// (set) Token: 0x06001264 RID: 4708 RVA: 0x0001AB4F File Offset: 0x00018D4F
		public object ClientData { get; set; }

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06001265 RID: 4709 RVA: 0x0001AB58 File Offset: 0x00018D58
		// (set) Token: 0x06001266 RID: 4710 RVA: 0x0001AB60 File Offset: 0x00018D60
		public Utf8String RoomName { get; set; }

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001267 RID: 4711 RVA: 0x0001AB69 File Offset: 0x00018D69
		// (set) Token: 0x06001268 RID: 4712 RVA: 0x0001AB71 File Offset: 0x00018D71
		public Utf8String ClientBaseUrl { get; set; }

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06001269 RID: 4713 RVA: 0x0001AB7A File Offset: 0x00018D7A
		// (set) Token: 0x0600126A RID: 4714 RVA: 0x0001AB82 File Offset: 0x00018D82
		public uint QueryId { get; set; }

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x0600126B RID: 4715 RVA: 0x0001AB8B File Offset: 0x00018D8B
		// (set) Token: 0x0600126C RID: 4716 RVA: 0x0001AB93 File Offset: 0x00018D93
		public uint TokenCount { get; set; }

		// Token: 0x0600126D RID: 4717 RVA: 0x0001AB9C File Offset: 0x00018D9C
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x0001ABBC File Offset: 0x00018DBC
		internal void Set(ref QueryJoinRoomTokenCompleteCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.RoomName = other.RoomName;
			this.ClientBaseUrl = other.ClientBaseUrl;
			this.QueryId = other.QueryId;
			this.TokenCount = other.TokenCount;
		}
	}
}
