using System;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000760 RID: 1888
	public struct UnlockAchievementsOptions
	{
		// Token: 0x17000EDA RID: 3802
		// (get) Token: 0x06003123 RID: 12579 RVA: 0x00049000 File Offset: 0x00047200
		// (set) Token: 0x06003124 RID: 12580 RVA: 0x00049008 File Offset: 0x00047208
		public ProductUserId UserId { get; set; }

		// Token: 0x17000EDB RID: 3803
		// (get) Token: 0x06003125 RID: 12581 RVA: 0x00049011 File Offset: 0x00047211
		// (set) Token: 0x06003126 RID: 12582 RVA: 0x00049019 File Offset: 0x00047219
		public Utf8String[] AchievementIds { get; set; }
	}
}
