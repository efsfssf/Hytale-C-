using System;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000744 RID: 1860
	public struct OnAchievementsUnlockedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000E8F RID: 3727
		// (get) Token: 0x06003044 RID: 12356 RVA: 0x00047CD9 File Offset: 0x00045ED9
		// (set) Token: 0x06003045 RID: 12357 RVA: 0x00047CE1 File Offset: 0x00045EE1
		public object ClientData { get; set; }

		// Token: 0x17000E90 RID: 3728
		// (get) Token: 0x06003046 RID: 12358 RVA: 0x00047CEA File Offset: 0x00045EEA
		// (set) Token: 0x06003047 RID: 12359 RVA: 0x00047CF2 File Offset: 0x00045EF2
		public ProductUserId UserId { get; set; }

		// Token: 0x17000E91 RID: 3729
		// (get) Token: 0x06003048 RID: 12360 RVA: 0x00047CFB File Offset: 0x00045EFB
		// (set) Token: 0x06003049 RID: 12361 RVA: 0x00047D03 File Offset: 0x00045F03
		public Utf8String[] AchievementIds { get; set; }

		// Token: 0x0600304A RID: 12362 RVA: 0x00047D0C File Offset: 0x00045F0C
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x0600304B RID: 12363 RVA: 0x00047D27 File Offset: 0x00045F27
		internal void Set(ref OnAchievementsUnlockedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.UserId = other.UserId;
			this.AchievementIds = other.AchievementIds;
		}
	}
}
