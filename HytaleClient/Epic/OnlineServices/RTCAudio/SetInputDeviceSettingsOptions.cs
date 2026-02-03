using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000268 RID: 616
	public struct SetInputDeviceSettingsOptions
	{
		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x0600112D RID: 4397 RVA: 0x00018FDF File Offset: 0x000171DF
		// (set) Token: 0x0600112E RID: 4398 RVA: 0x00018FE7 File Offset: 0x000171E7
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x0600112F RID: 4399 RVA: 0x00018FF0 File Offset: 0x000171F0
		// (set) Token: 0x06001130 RID: 4400 RVA: 0x00018FF8 File Offset: 0x000171F8
		public Utf8String RealDeviceId { get; set; }

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06001131 RID: 4401 RVA: 0x00019001 File Offset: 0x00017201
		// (set) Token: 0x06001132 RID: 4402 RVA: 0x00019009 File Offset: 0x00017209
		public bool PlatformAEC { get; set; }
	}
}
