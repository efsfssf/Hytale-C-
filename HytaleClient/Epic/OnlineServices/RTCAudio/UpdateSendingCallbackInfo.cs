using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200027C RID: 636
	public struct UpdateSendingCallbackInfo : ICallbackInfo
	{
		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x060011D1 RID: 4561 RVA: 0x00019FFF File Offset: 0x000181FF
		// (set) Token: 0x060011D2 RID: 4562 RVA: 0x0001A007 File Offset: 0x00018207
		public Result ResultCode { get; set; }

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x060011D3 RID: 4563 RVA: 0x0001A010 File Offset: 0x00018210
		// (set) Token: 0x060011D4 RID: 4564 RVA: 0x0001A018 File Offset: 0x00018218
		public object ClientData { get; set; }

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x060011D5 RID: 4565 RVA: 0x0001A021 File Offset: 0x00018221
		// (set) Token: 0x060011D6 RID: 4566 RVA: 0x0001A029 File Offset: 0x00018229
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x060011D7 RID: 4567 RVA: 0x0001A032 File Offset: 0x00018232
		// (set) Token: 0x060011D8 RID: 4568 RVA: 0x0001A03A File Offset: 0x0001823A
		public Utf8String RoomName { get; set; }

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x060011D9 RID: 4569 RVA: 0x0001A043 File Offset: 0x00018243
		// (set) Token: 0x060011DA RID: 4570 RVA: 0x0001A04B File Offset: 0x0001824B
		public RTCAudioStatus AudioStatus { get; set; }

		// Token: 0x060011DB RID: 4571 RVA: 0x0001A054 File Offset: 0x00018254
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x0001A074 File Offset: 0x00018274
		internal void Set(ref UpdateSendingCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.AudioStatus = other.AudioStatus;
		}
	}
}
