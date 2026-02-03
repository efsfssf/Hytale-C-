using System;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000748 RID: 1864
	public struct OnAchievementsUnlockedCallbackV2Info : ICallbackInfo
	{
		// Token: 0x17000E96 RID: 3734
		// (get) Token: 0x0600305F RID: 12383 RVA: 0x00047ED2 File Offset: 0x000460D2
		// (set) Token: 0x06003060 RID: 12384 RVA: 0x00047EDA File Offset: 0x000460DA
		public object ClientData { get; set; }

		// Token: 0x17000E97 RID: 3735
		// (get) Token: 0x06003061 RID: 12385 RVA: 0x00047EE3 File Offset: 0x000460E3
		// (set) Token: 0x06003062 RID: 12386 RVA: 0x00047EEB File Offset: 0x000460EB
		public ProductUserId UserId { get; set; }

		// Token: 0x17000E98 RID: 3736
		// (get) Token: 0x06003063 RID: 12387 RVA: 0x00047EF4 File Offset: 0x000460F4
		// (set) Token: 0x06003064 RID: 12388 RVA: 0x00047EFC File Offset: 0x000460FC
		public Utf8String AchievementId { get; set; }

		// Token: 0x17000E99 RID: 3737
		// (get) Token: 0x06003065 RID: 12389 RVA: 0x00047F05 File Offset: 0x00046105
		// (set) Token: 0x06003066 RID: 12390 RVA: 0x00047F0D File Offset: 0x0004610D
		public DateTimeOffset? UnlockTime { get; set; }

		// Token: 0x06003067 RID: 12391 RVA: 0x00047F18 File Offset: 0x00046118
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06003068 RID: 12392 RVA: 0x00047F33 File Offset: 0x00046133
		internal void Set(ref OnAchievementsUnlockedCallbackV2InfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.UserId = other.UserId;
			this.AchievementId = other.AchievementId;
			this.UnlockTime = other.UnlockTime;
		}
	}
}
