using System;

namespace Epic.OnlineServices.RTCAdmin
{
	// Token: 0x02000297 RID: 663
	public struct SetParticipantHardMuteCompleteCallbackInfo : ICallbackInfo
	{
		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06001299 RID: 4761 RVA: 0x0001B23B File Offset: 0x0001943B
		// (set) Token: 0x0600129A RID: 4762 RVA: 0x0001B243 File Offset: 0x00019443
		public Result ResultCode { get; set; }

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x0600129B RID: 4763 RVA: 0x0001B24C File Offset: 0x0001944C
		// (set) Token: 0x0600129C RID: 4764 RVA: 0x0001B254 File Offset: 0x00019454
		public object ClientData { get; set; }

		// Token: 0x0600129D RID: 4765 RVA: 0x0001B260 File Offset: 0x00019460
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x0001B27D File Offset: 0x0001947D
		internal void Set(ref SetParticipantHardMuteCompleteCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}
	}
}
