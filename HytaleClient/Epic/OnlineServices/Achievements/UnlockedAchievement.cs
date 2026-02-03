using System;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000762 RID: 1890
	public struct UnlockedAchievement
	{
		// Token: 0x17000EDE RID: 3806
		// (get) Token: 0x0600312C RID: 12588 RVA: 0x000490D6 File Offset: 0x000472D6
		// (set) Token: 0x0600312D RID: 12589 RVA: 0x000490DE File Offset: 0x000472DE
		public Utf8String AchievementId { get; set; }

		// Token: 0x17000EDF RID: 3807
		// (get) Token: 0x0600312E RID: 12590 RVA: 0x000490E7 File Offset: 0x000472E7
		// (set) Token: 0x0600312F RID: 12591 RVA: 0x000490EF File Offset: 0x000472EF
		public DateTimeOffset? UnlockTime { get; set; }

		// Token: 0x06003130 RID: 12592 RVA: 0x000490F8 File Offset: 0x000472F8
		internal void Set(ref UnlockedAchievementInternal other)
		{
			this.AchievementId = other.AchievementId;
			this.UnlockTime = other.UnlockTime;
		}
	}
}
