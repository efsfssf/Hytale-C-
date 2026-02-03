using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000729 RID: 1833
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyAchievementDefinitionByAchievementIdOptionsInternal : ISettable<CopyAchievementDefinitionByAchievementIdOptions>, IDisposable
	{
		// Token: 0x17000E46 RID: 3654
		// (set) Token: 0x06002F8F RID: 12175 RVA: 0x00046AD9 File Offset: 0x00044CD9
		public Utf8String AchievementId
		{
			set
			{
				Helper.Set(value, ref this.m_AchievementId);
			}
		}

		// Token: 0x06002F90 RID: 12176 RVA: 0x00046AE9 File Offset: 0x00044CE9
		public void Set(ref CopyAchievementDefinitionByAchievementIdOptions other)
		{
			this.m_ApiVersion = 1;
			this.AchievementId = other.AchievementId;
		}

		// Token: 0x06002F91 RID: 12177 RVA: 0x00046B00 File Offset: 0x00044D00
		public void Set(ref CopyAchievementDefinitionByAchievementIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.AchievementId = other.Value.AchievementId;
			}
		}

		// Token: 0x06002F92 RID: 12178 RVA: 0x00046B36 File Offset: 0x00044D36
		public void Dispose()
		{
			Helper.Dispose(ref this.m_AchievementId);
		}

		// Token: 0x04001543 RID: 5443
		private int m_ApiVersion;

		// Token: 0x04001544 RID: 5444
		private IntPtr m_AchievementId;
	}
}
