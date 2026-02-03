using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003A7 RID: 935
	public struct KickMemberOptions
	{
		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06001938 RID: 6456 RVA: 0x00024EEE File Offset: 0x000230EE
		// (set) Token: 0x06001939 RID: 6457 RVA: 0x00024EF6 File Offset: 0x000230F6
		public Utf8String LobbyId { get; set; }

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x0600193A RID: 6458 RVA: 0x00024EFF File Offset: 0x000230FF
		// (set) Token: 0x0600193B RID: 6459 RVA: 0x00024F07 File Offset: 0x00023107
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x0600193C RID: 6460 RVA: 0x00024F10 File Offset: 0x00023110
		// (set) Token: 0x0600193D RID: 6461 RVA: 0x00024F18 File Offset: 0x00023118
		public ProductUserId TargetUserId { get; set; }
	}
}
