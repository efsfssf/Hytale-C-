using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003BB RID: 955
	public struct LobbyDetailsCopyMemberAttributeByIndexOptions
	{
		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x060019AC RID: 6572 RVA: 0x00025BE8 File Offset: 0x00023DE8
		// (set) Token: 0x060019AD RID: 6573 RVA: 0x00025BF0 File Offset: 0x00023DF0
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x060019AE RID: 6574 RVA: 0x00025BF9 File Offset: 0x00023DF9
		// (set) Token: 0x060019AF RID: 6575 RVA: 0x00025C01 File Offset: 0x00023E01
		public uint AttrIndex { get; set; }
	}
}
