using System;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000734 RID: 1844
	public struct CopyUnlockedAchievementByAchievementIdOptions
	{
		// Token: 0x17000E59 RID: 3673
		// (get) Token: 0x06002FBD RID: 12221 RVA: 0x00046EC7 File Offset: 0x000450C7
		// (set) Token: 0x06002FBE RID: 12222 RVA: 0x00046ECF File Offset: 0x000450CF
		public ProductUserId UserId { get; set; }

		// Token: 0x17000E5A RID: 3674
		// (get) Token: 0x06002FBF RID: 12223 RVA: 0x00046ED8 File Offset: 0x000450D8
		// (set) Token: 0x06002FC0 RID: 12224 RVA: 0x00046EE0 File Offset: 0x000450E0
		public Utf8String AchievementId { get; set; }
	}
}
