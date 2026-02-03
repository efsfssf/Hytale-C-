using System;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001AB RID: 427
	public struct AddNotifyDisconnectedOptions
	{
		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000C58 RID: 3160 RVA: 0x000120AF File Offset: 0x000102AF
		// (set) Token: 0x06000C59 RID: 3161 RVA: 0x000120B7 File Offset: 0x000102B7
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000C5A RID: 3162 RVA: 0x000120C0 File Offset: 0x000102C0
		// (set) Token: 0x06000C5B RID: 3163 RVA: 0x000120C8 File Offset: 0x000102C8
		public Utf8String RoomName { get; set; }
	}
}
