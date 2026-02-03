using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003BD RID: 957
	public struct LobbyDetailsCopyMemberAttributeByKeyOptions
	{
		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x060019B5 RID: 6581 RVA: 0x00025CA2 File Offset: 0x00023EA2
		// (set) Token: 0x060019B6 RID: 6582 RVA: 0x00025CAA File Offset: 0x00023EAA
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x060019B7 RID: 6583 RVA: 0x00025CB3 File Offset: 0x00023EB3
		// (set) Token: 0x060019B8 RID: 6584 RVA: 0x00025CBB File Offset: 0x00023EBB
		public Utf8String AttrKey { get; set; }
	}
}
