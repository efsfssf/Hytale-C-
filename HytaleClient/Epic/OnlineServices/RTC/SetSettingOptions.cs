using System;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001D8 RID: 472
	public struct SetSettingOptions
	{
		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000DB6 RID: 3510 RVA: 0x00014174 File Offset: 0x00012374
		// (set) Token: 0x06000DB7 RID: 3511 RVA: 0x0001417C File Offset: 0x0001237C
		public Utf8String SettingName { get; set; }

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000DB8 RID: 3512 RVA: 0x00014185 File Offset: 0x00012385
		// (set) Token: 0x06000DB9 RID: 3513 RVA: 0x0001418D File Offset: 0x0001238D
		public Utf8String SettingValue { get; set; }
	}
}
