using System;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x0200075C RID: 1884
	public struct QueryPlayerAchievementsOptions
	{
		// Token: 0x17000ED2 RID: 3794
		// (get) Token: 0x0600310D RID: 12557 RVA: 0x00048E07 File Offset: 0x00047007
		// (set) Token: 0x0600310E RID: 12558 RVA: 0x00048E0F File Offset: 0x0004700F
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x17000ED3 RID: 3795
		// (get) Token: 0x0600310F RID: 12559 RVA: 0x00048E18 File Offset: 0x00047018
		// (set) Token: 0x06003110 RID: 12560 RVA: 0x00048E20 File Offset: 0x00047020
		public ProductUserId LocalUserId { get; set; }
	}
}
