using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000761 RID: 1889
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UnlockAchievementsOptionsInternal : ISettable<UnlockAchievementsOptions>, IDisposable
	{
		// Token: 0x17000EDC RID: 3804
		// (set) Token: 0x06003127 RID: 12583 RVA: 0x00049022 File Offset: 0x00047222
		public ProductUserId UserId
		{
			set
			{
				Helper.Set(value, ref this.m_UserId);
			}
		}

		// Token: 0x17000EDD RID: 3805
		// (set) Token: 0x06003128 RID: 12584 RVA: 0x00049032 File Offset: 0x00047232
		public Utf8String[] AchievementIds
		{
			set
			{
				Helper.Set<Utf8String>(value, ref this.m_AchievementIds, true, out this.m_AchievementsCount);
			}
		}

		// Token: 0x06003129 RID: 12585 RVA: 0x00049049 File Offset: 0x00047249
		public void Set(ref UnlockAchievementsOptions other)
		{
			this.m_ApiVersion = 1;
			this.UserId = other.UserId;
			this.AchievementIds = other.AchievementIds;
		}

		// Token: 0x0600312A RID: 12586 RVA: 0x00049070 File Offset: 0x00047270
		public void Set(ref UnlockAchievementsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.UserId = other.Value.UserId;
				this.AchievementIds = other.Value.AchievementIds;
			}
		}

		// Token: 0x0600312B RID: 12587 RVA: 0x000490BB File Offset: 0x000472BB
		public void Dispose()
		{
			Helper.Dispose(ref this.m_UserId);
			Helper.Dispose(ref this.m_AchievementIds);
		}

		// Token: 0x040015EB RID: 5611
		private int m_ApiVersion;

		// Token: 0x040015EC RID: 5612
		private IntPtr m_UserId;

		// Token: 0x040015ED RID: 5613
		private IntPtr m_AchievementIds;

		// Token: 0x040015EE RID: 5614
		private uint m_AchievementsCount;
	}
}
