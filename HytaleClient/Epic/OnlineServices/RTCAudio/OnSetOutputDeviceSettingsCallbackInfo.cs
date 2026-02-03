using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000242 RID: 578
	public struct OnSetOutputDeviceSettingsCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06001037 RID: 4151 RVA: 0x00017536 File Offset: 0x00015736
		// (set) Token: 0x06001038 RID: 4152 RVA: 0x0001753E File Offset: 0x0001573E
		public Result ResultCode { get; set; }

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06001039 RID: 4153 RVA: 0x00017547 File Offset: 0x00015747
		// (set) Token: 0x0600103A RID: 4154 RVA: 0x0001754F File Offset: 0x0001574F
		public object ClientData { get; set; }

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x0600103B RID: 4155 RVA: 0x00017558 File Offset: 0x00015758
		// (set) Token: 0x0600103C RID: 4156 RVA: 0x00017560 File Offset: 0x00015760
		public Utf8String RealDeviceId { get; set; }

		// Token: 0x0600103D RID: 4157 RVA: 0x0001756C File Offset: 0x0001576C
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x00017589 File Offset: 0x00015789
		internal void Set(ref OnSetOutputDeviceSettingsCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.RealDeviceId = other.RealDeviceId;
		}
	}
}
