using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000210 RID: 528
	public struct AudioOutputStateCallbackInfo : ICallbackInfo
	{
		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06000F4F RID: 3919 RVA: 0x00016875 File Offset: 0x00014A75
		// (set) Token: 0x06000F50 RID: 3920 RVA: 0x0001687D File Offset: 0x00014A7D
		public object ClientData { get; set; }

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06000F51 RID: 3921 RVA: 0x00016886 File Offset: 0x00014A86
		// (set) Token: 0x06000F52 RID: 3922 RVA: 0x0001688E File Offset: 0x00014A8E
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06000F53 RID: 3923 RVA: 0x00016897 File Offset: 0x00014A97
		// (set) Token: 0x06000F54 RID: 3924 RVA: 0x0001689F File Offset: 0x00014A9F
		public Utf8String RoomName { get; set; }

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06000F55 RID: 3925 RVA: 0x000168A8 File Offset: 0x00014AA8
		// (set) Token: 0x06000F56 RID: 3926 RVA: 0x000168B0 File Offset: 0x00014AB0
		public RTCAudioOutputStatus Status { get; set; }

		// Token: 0x06000F57 RID: 3927 RVA: 0x000168BC File Offset: 0x00014ABC
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x000168D7 File Offset: 0x00014AD7
		internal void Set(ref AudioOutputStateCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.Status = other.Status;
		}
	}
}
