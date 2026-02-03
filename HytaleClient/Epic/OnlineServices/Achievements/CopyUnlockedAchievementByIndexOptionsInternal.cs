using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000737 RID: 1847
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyUnlockedAchievementByIndexOptionsInternal : ISettable<CopyUnlockedAchievementByIndexOptions>, IDisposable
	{
		// Token: 0x17000E5F RID: 3679
		// (set) Token: 0x06002FCA RID: 12234 RVA: 0x00046FB8 File Offset: 0x000451B8
		public ProductUserId UserId
		{
			set
			{
				Helper.Set(value, ref this.m_UserId);
			}
		}

		// Token: 0x17000E60 RID: 3680
		// (set) Token: 0x06002FCB RID: 12235 RVA: 0x00046FC8 File Offset: 0x000451C8
		public uint AchievementIndex
		{
			set
			{
				this.m_AchievementIndex = value;
			}
		}

		// Token: 0x06002FCC RID: 12236 RVA: 0x00046FD2 File Offset: 0x000451D2
		public void Set(ref CopyUnlockedAchievementByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.UserId = other.UserId;
			this.AchievementIndex = other.AchievementIndex;
		}

		// Token: 0x06002FCD RID: 12237 RVA: 0x00046FF8 File Offset: 0x000451F8
		public void Set(ref CopyUnlockedAchievementByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.UserId = other.Value.UserId;
				this.AchievementIndex = other.Value.AchievementIndex;
			}
		}

		// Token: 0x06002FCE RID: 12238 RVA: 0x00047043 File Offset: 0x00045243
		public void Dispose()
		{
			Helper.Dispose(ref this.m_UserId);
		}

		// Token: 0x04001563 RID: 5475
		private int m_ApiVersion;

		// Token: 0x04001564 RID: 5476
		private IntPtr m_UserId;

		// Token: 0x04001565 RID: 5477
		private uint m_AchievementIndex;
	}
}
