using System;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x0200075A RID: 1882
	public struct QueryDefinitionsOptions
	{
		// Token: 0x17000ECC RID: 3788
		// (get) Token: 0x06003101 RID: 12545 RVA: 0x00048CE5 File Offset: 0x00046EE5
		// (set) Token: 0x06003102 RID: 12546 RVA: 0x00048CED File Offset: 0x00046EED
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000ECD RID: 3789
		// (get) Token: 0x06003103 RID: 12547 RVA: 0x00048CF6 File Offset: 0x00046EF6
		// (set) Token: 0x06003104 RID: 12548 RVA: 0x00048CFE File Offset: 0x00046EFE
		internal EpicAccountId EpicUserId_DEPRECATED { get; set; }

		// Token: 0x17000ECE RID: 3790
		// (get) Token: 0x06003105 RID: 12549 RVA: 0x00048D07 File Offset: 0x00046F07
		// (set) Token: 0x06003106 RID: 12550 RVA: 0x00048D0F File Offset: 0x00046F0F
		internal Utf8String[] HiddenAchievementIds_DEPRECATED { get; set; }
	}
}
