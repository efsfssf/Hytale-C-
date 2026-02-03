using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x0200072B RID: 1835
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyAchievementDefinitionByIndexOptionsInternal : ISettable<CopyAchievementDefinitionByIndexOptions>, IDisposable
	{
		// Token: 0x17000E48 RID: 3656
		// (set) Token: 0x06002F95 RID: 12181 RVA: 0x00046B56 File Offset: 0x00044D56
		public uint AchievementIndex
		{
			set
			{
				this.m_AchievementIndex = value;
			}
		}

		// Token: 0x06002F96 RID: 12182 RVA: 0x00046B60 File Offset: 0x00044D60
		public void Set(ref CopyAchievementDefinitionByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.AchievementIndex = other.AchievementIndex;
		}

		// Token: 0x06002F97 RID: 12183 RVA: 0x00046B78 File Offset: 0x00044D78
		public void Set(ref CopyAchievementDefinitionByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.AchievementIndex = other.Value.AchievementIndex;
			}
		}

		// Token: 0x06002F98 RID: 12184 RVA: 0x00046BAE File Offset: 0x00044DAE
		public void Dispose()
		{
		}

		// Token: 0x04001546 RID: 5446
		private int m_ApiVersion;

		// Token: 0x04001547 RID: 5447
		private uint m_AchievementIndex;
	}
}
