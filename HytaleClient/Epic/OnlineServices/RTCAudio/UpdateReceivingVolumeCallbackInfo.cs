using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000278 RID: 632
	public struct UpdateReceivingVolumeCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x060011AA RID: 4522 RVA: 0x00019C24 File Offset: 0x00017E24
		// (set) Token: 0x060011AB RID: 4523 RVA: 0x00019C2C File Offset: 0x00017E2C
		public Result ResultCode { get; set; }

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x060011AC RID: 4524 RVA: 0x00019C35 File Offset: 0x00017E35
		// (set) Token: 0x060011AD RID: 4525 RVA: 0x00019C3D File Offset: 0x00017E3D
		public object ClientData { get; set; }

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x060011AE RID: 4526 RVA: 0x00019C46 File Offset: 0x00017E46
		// (set) Token: 0x060011AF RID: 4527 RVA: 0x00019C4E File Offset: 0x00017E4E
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x060011B0 RID: 4528 RVA: 0x00019C57 File Offset: 0x00017E57
		// (set) Token: 0x060011B1 RID: 4529 RVA: 0x00019C5F File Offset: 0x00017E5F
		public Utf8String RoomName { get; set; }

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x060011B2 RID: 4530 RVA: 0x00019C68 File Offset: 0x00017E68
		// (set) Token: 0x060011B3 RID: 4531 RVA: 0x00019C70 File Offset: 0x00017E70
		public float Volume { get; set; }

		// Token: 0x060011B4 RID: 4532 RVA: 0x00019C7C File Offset: 0x00017E7C
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x00019C9C File Offset: 0x00017E9C
		internal void Set(ref UpdateReceivingVolumeCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.Volume = other.Volume;
		}
	}
}
