using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000266 RID: 614
	public struct SetAudioOutputSettingsOptions
	{
		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06001121 RID: 4385 RVA: 0x00018ED4 File Offset: 0x000170D4
		// (set) Token: 0x06001122 RID: 4386 RVA: 0x00018EDC File Offset: 0x000170DC
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06001123 RID: 4387 RVA: 0x00018EE5 File Offset: 0x000170E5
		// (set) Token: 0x06001124 RID: 4388 RVA: 0x00018EED File Offset: 0x000170ED
		public Utf8String DeviceId { get; set; }

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06001125 RID: 4389 RVA: 0x00018EF6 File Offset: 0x000170F6
		// (set) Token: 0x06001126 RID: 4390 RVA: 0x00018EFE File Offset: 0x000170FE
		public float Volume { get; set; }
	}
}
