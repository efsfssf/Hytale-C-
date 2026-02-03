using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003A3 RID: 931
	public struct JoinRTCRoomOptions
	{
		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06001919 RID: 6425 RVA: 0x00024C02 File Offset: 0x00022E02
		// (set) Token: 0x0600191A RID: 6426 RVA: 0x00024C0A File Offset: 0x00022E0A
		public Utf8String LobbyId { get; set; }

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x0600191B RID: 6427 RVA: 0x00024C13 File Offset: 0x00022E13
		// (set) Token: 0x0600191C RID: 6428 RVA: 0x00024C1B File Offset: 0x00022E1B
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x0600191D RID: 6429 RVA: 0x00024C24 File Offset: 0x00022E24
		// (set) Token: 0x0600191E RID: 6430 RVA: 0x00024C2C File Offset: 0x00022E2C
		public LocalRTCOptions? LocalRTCOptions { get; set; }
	}
}
