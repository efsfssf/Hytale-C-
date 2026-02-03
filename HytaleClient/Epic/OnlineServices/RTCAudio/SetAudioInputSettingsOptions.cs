using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000264 RID: 612
	public struct SetAudioInputSettingsOptions
	{
		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06001112 RID: 4370 RVA: 0x00018D87 File Offset: 0x00016F87
		// (set) Token: 0x06001113 RID: 4371 RVA: 0x00018D8F File Offset: 0x00016F8F
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06001114 RID: 4372 RVA: 0x00018D98 File Offset: 0x00016F98
		// (set) Token: 0x06001115 RID: 4373 RVA: 0x00018DA0 File Offset: 0x00016FA0
		public Utf8String DeviceId { get; set; }

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06001116 RID: 4374 RVA: 0x00018DA9 File Offset: 0x00016FA9
		// (set) Token: 0x06001117 RID: 4375 RVA: 0x00018DB1 File Offset: 0x00016FB1
		public float Volume { get; set; }

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06001118 RID: 4376 RVA: 0x00018DBA File Offset: 0x00016FBA
		// (set) Token: 0x06001119 RID: 4377 RVA: 0x00018DC2 File Offset: 0x00016FC2
		public bool PlatformAEC { get; set; }
	}
}
