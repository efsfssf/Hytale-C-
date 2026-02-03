using System;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001D6 RID: 470
	public struct SetRoomSettingOptions
	{
		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000DA7 RID: 3495 RVA: 0x00014007 File Offset: 0x00012207
		// (set) Token: 0x06000DA8 RID: 3496 RVA: 0x0001400F File Offset: 0x0001220F
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000DA9 RID: 3497 RVA: 0x00014018 File Offset: 0x00012218
		// (set) Token: 0x06000DAA RID: 3498 RVA: 0x00014020 File Offset: 0x00012220
		public Utf8String RoomName { get; set; }

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000DAB RID: 3499 RVA: 0x00014029 File Offset: 0x00012229
		// (set) Token: 0x06000DAC RID: 3500 RVA: 0x00014031 File Offset: 0x00012231
		public Utf8String SettingName { get; set; }

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000DAD RID: 3501 RVA: 0x0001403A File Offset: 0x0001223A
		// (set) Token: 0x06000DAE RID: 3502 RVA: 0x00014042 File Offset: 0x00012242
		public Utf8String SettingValue { get; set; }
	}
}
