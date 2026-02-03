using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000735 RID: 1845
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyUnlockedAchievementByAchievementIdOptionsInternal : ISettable<CopyUnlockedAchievementByAchievementIdOptions>, IDisposable
	{
		// Token: 0x17000E5B RID: 3675
		// (set) Token: 0x06002FC1 RID: 12225 RVA: 0x00046EE9 File Offset: 0x000450E9
		public ProductUserId UserId
		{
			set
			{
				Helper.Set(value, ref this.m_UserId);
			}
		}

		// Token: 0x17000E5C RID: 3676
		// (set) Token: 0x06002FC2 RID: 12226 RVA: 0x00046EF9 File Offset: 0x000450F9
		public Utf8String AchievementId
		{
			set
			{
				Helper.Set(value, ref this.m_AchievementId);
			}
		}

		// Token: 0x06002FC3 RID: 12227 RVA: 0x00046F09 File Offset: 0x00045109
		public void Set(ref CopyUnlockedAchievementByAchievementIdOptions other)
		{
			this.m_ApiVersion = 1;
			this.UserId = other.UserId;
			this.AchievementId = other.AchievementId;
		}

		// Token: 0x06002FC4 RID: 12228 RVA: 0x00046F30 File Offset: 0x00045130
		public void Set(ref CopyUnlockedAchievementByAchievementIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.UserId = other.Value.UserId;
				this.AchievementId = other.Value.AchievementId;
			}
		}

		// Token: 0x06002FC5 RID: 12229 RVA: 0x00046F7B File Offset: 0x0004517B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_UserId);
			Helper.Dispose(ref this.m_AchievementId);
		}

		// Token: 0x0400155E RID: 5470
		private int m_ApiVersion;

		// Token: 0x0400155F RID: 5471
		private IntPtr m_UserId;

		// Token: 0x04001560 RID: 5472
		private IntPtr m_AchievementId;
	}
}
