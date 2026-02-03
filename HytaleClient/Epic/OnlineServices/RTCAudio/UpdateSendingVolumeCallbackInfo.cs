using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000280 RID: 640
	public struct UpdateSendingVolumeCallbackInfo : ICallbackInfo
	{
		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x060011F8 RID: 4600 RVA: 0x0001A3D7 File Offset: 0x000185D7
		// (set) Token: 0x060011F9 RID: 4601 RVA: 0x0001A3DF File Offset: 0x000185DF
		public Result ResultCode { get; set; }

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x060011FA RID: 4602 RVA: 0x0001A3E8 File Offset: 0x000185E8
		// (set) Token: 0x060011FB RID: 4603 RVA: 0x0001A3F0 File Offset: 0x000185F0
		public object ClientData { get; set; }

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x060011FC RID: 4604 RVA: 0x0001A3F9 File Offset: 0x000185F9
		// (set) Token: 0x060011FD RID: 4605 RVA: 0x0001A401 File Offset: 0x00018601
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x060011FE RID: 4606 RVA: 0x0001A40A File Offset: 0x0001860A
		// (set) Token: 0x060011FF RID: 4607 RVA: 0x0001A412 File Offset: 0x00018612
		public Utf8String RoomName { get; set; }

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06001200 RID: 4608 RVA: 0x0001A41B File Offset: 0x0001861B
		// (set) Token: 0x06001201 RID: 4609 RVA: 0x0001A423 File Offset: 0x00018623
		public float Volume { get; set; }

		// Token: 0x06001202 RID: 4610 RVA: 0x0001A42C File Offset: 0x0001862C
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x0001A44C File Offset: 0x0001864C
		internal void Set(ref UpdateSendingVolumeCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.Volume = other.Volume;
		}
	}
}
