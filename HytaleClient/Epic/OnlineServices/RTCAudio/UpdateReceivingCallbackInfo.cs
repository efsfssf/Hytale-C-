using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000274 RID: 628
	public struct UpdateReceivingCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x0600117C RID: 4476 RVA: 0x00019764 File Offset: 0x00017964
		// (set) Token: 0x0600117D RID: 4477 RVA: 0x0001976C File Offset: 0x0001796C
		public Result ResultCode { get; set; }

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x0600117E RID: 4478 RVA: 0x00019775 File Offset: 0x00017975
		// (set) Token: 0x0600117F RID: 4479 RVA: 0x0001977D File Offset: 0x0001797D
		public object ClientData { get; set; }

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06001180 RID: 4480 RVA: 0x00019786 File Offset: 0x00017986
		// (set) Token: 0x06001181 RID: 4481 RVA: 0x0001978E File Offset: 0x0001798E
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06001182 RID: 4482 RVA: 0x00019797 File Offset: 0x00017997
		// (set) Token: 0x06001183 RID: 4483 RVA: 0x0001979F File Offset: 0x0001799F
		public Utf8String RoomName { get; set; }

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06001184 RID: 4484 RVA: 0x000197A8 File Offset: 0x000179A8
		// (set) Token: 0x06001185 RID: 4485 RVA: 0x000197B0 File Offset: 0x000179B0
		public ProductUserId ParticipantId { get; set; }

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06001186 RID: 4486 RVA: 0x000197B9 File Offset: 0x000179B9
		// (set) Token: 0x06001187 RID: 4487 RVA: 0x000197C1 File Offset: 0x000179C1
		public bool AudioEnabled { get; set; }

		// Token: 0x06001188 RID: 4488 RVA: 0x000197CC File Offset: 0x000179CC
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x000197EC File Offset: 0x000179EC
		internal void Set(ref UpdateReceivingCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.ParticipantId = other.ParticipantId;
			this.AudioEnabled = other.AudioEnabled;
		}
	}
}
