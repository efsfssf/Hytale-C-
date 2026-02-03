using System;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000736 RID: 1846
	public struct CopyUnlockedAchievementByIndexOptions
	{
		// Token: 0x17000E5D RID: 3677
		// (get) Token: 0x06002FC6 RID: 12230 RVA: 0x00046F96 File Offset: 0x00045196
		// (set) Token: 0x06002FC7 RID: 12231 RVA: 0x00046F9E File Offset: 0x0004519E
		public ProductUserId UserId { get; set; }

		// Token: 0x17000E5E RID: 3678
		// (get) Token: 0x06002FC8 RID: 12232 RVA: 0x00046FA7 File Offset: 0x000451A7
		// (set) Token: 0x06002FC9 RID: 12233 RVA: 0x00046FAF File Offset: 0x000451AF
		public uint AchievementIndex { get; set; }
	}
}
