using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x0200072D RID: 1837
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyAchievementDefinitionV2ByAchievementIdOptionsInternal : ISettable<CopyAchievementDefinitionV2ByAchievementIdOptions>, IDisposable
	{
		// Token: 0x17000E4A RID: 3658
		// (set) Token: 0x06002F9B RID: 12187 RVA: 0x00046BC2 File Offset: 0x00044DC2
		public Utf8String AchievementId
		{
			set
			{
				Helper.Set(value, ref this.m_AchievementId);
			}
		}

		// Token: 0x06002F9C RID: 12188 RVA: 0x00046BD2 File Offset: 0x00044DD2
		public void Set(ref CopyAchievementDefinitionV2ByAchievementIdOptions other)
		{
			this.m_ApiVersion = 2;
			this.AchievementId = other.AchievementId;
		}

		// Token: 0x06002F9D RID: 12189 RVA: 0x00046BEC File Offset: 0x00044DEC
		public void Set(ref CopyAchievementDefinitionV2ByAchievementIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.AchievementId = other.Value.AchievementId;
			}
		}

		// Token: 0x06002F9E RID: 12190 RVA: 0x00046C22 File Offset: 0x00044E22
		public void Dispose()
		{
			Helper.Dispose(ref this.m_AchievementId);
		}

		// Token: 0x04001549 RID: 5449
		private int m_ApiVersion;

		// Token: 0x0400154A RID: 5450
		private IntPtr m_AchievementId;
	}
}
