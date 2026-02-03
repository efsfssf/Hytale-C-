using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200023E RID: 574
	public struct OnSetInputDeviceSettingsCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x0600101C RID: 4124 RVA: 0x00017366 File Offset: 0x00015566
		// (set) Token: 0x0600101D RID: 4125 RVA: 0x0001736E File Offset: 0x0001556E
		public Result ResultCode { get; set; }

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x0600101E RID: 4126 RVA: 0x00017377 File Offset: 0x00015577
		// (set) Token: 0x0600101F RID: 4127 RVA: 0x0001737F File Offset: 0x0001557F
		public object ClientData { get; set; }

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06001020 RID: 4128 RVA: 0x00017388 File Offset: 0x00015588
		// (set) Token: 0x06001021 RID: 4129 RVA: 0x00017390 File Offset: 0x00015590
		public Utf8String RealDeviceId { get; set; }

		// Token: 0x06001022 RID: 4130 RVA: 0x0001739C File Offset: 0x0001559C
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x000173B9 File Offset: 0x000155B9
		internal void Set(ref OnSetInputDeviceSettingsCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.RealDeviceId = other.RealDeviceId;
		}
	}
}
