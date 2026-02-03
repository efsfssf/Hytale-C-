using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x0200072F RID: 1839
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyAchievementDefinitionV2ByIndexOptionsInternal : ISettable<CopyAchievementDefinitionV2ByIndexOptions>, IDisposable
	{
		// Token: 0x17000E4C RID: 3660
		// (set) Token: 0x06002FA1 RID: 12193 RVA: 0x00046C42 File Offset: 0x00044E42
		public uint AchievementIndex
		{
			set
			{
				this.m_AchievementIndex = value;
			}
		}

		// Token: 0x06002FA2 RID: 12194 RVA: 0x00046C4C File Offset: 0x00044E4C
		public void Set(ref CopyAchievementDefinitionV2ByIndexOptions other)
		{
			this.m_ApiVersion = 2;
			this.AchievementIndex = other.AchievementIndex;
		}

		// Token: 0x06002FA3 RID: 12195 RVA: 0x00046C64 File Offset: 0x00044E64
		public void Set(ref CopyAchievementDefinitionV2ByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.AchievementIndex = other.Value.AchievementIndex;
			}
		}

		// Token: 0x06002FA4 RID: 12196 RVA: 0x00046C9A File Offset: 0x00044E9A
		public void Dispose()
		{
		}

		// Token: 0x0400154C RID: 5452
		private int m_ApiVersion;

		// Token: 0x0400154D RID: 5453
		private uint m_AchievementIndex;
	}
}
