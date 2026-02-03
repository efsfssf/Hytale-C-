using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200026A RID: 618
	public struct SetOutputDeviceSettingsOptions
	{
		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06001139 RID: 4409 RVA: 0x000190EF File Offset: 0x000172EF
		// (set) Token: 0x0600113A RID: 4410 RVA: 0x000190F7 File Offset: 0x000172F7
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x0600113B RID: 4411 RVA: 0x00019100 File Offset: 0x00017300
		// (set) Token: 0x0600113C RID: 4412 RVA: 0x00019108 File Offset: 0x00017308
		public Utf8String RealDeviceId { get; set; }
	}
}
