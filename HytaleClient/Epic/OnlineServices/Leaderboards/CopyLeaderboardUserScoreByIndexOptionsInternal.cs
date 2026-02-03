using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x0200045E RID: 1118
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyLeaderboardUserScoreByIndexOptionsInternal : ISettable<CopyLeaderboardUserScoreByIndexOptions>, IDisposable
	{
		// Token: 0x17000843 RID: 2115
		// (set) Token: 0x06001D55 RID: 7509 RVA: 0x0002AE3F File Offset: 0x0002903F
		public uint LeaderboardUserScoreIndex
		{
			set
			{
				this.m_LeaderboardUserScoreIndex = value;
			}
		}

		// Token: 0x17000844 RID: 2116
		// (set) Token: 0x06001D56 RID: 7510 RVA: 0x0002AE49 File Offset: 0x00029049
		public Utf8String StatName
		{
			set
			{
				Helper.Set(value, ref this.m_StatName);
			}
		}

		// Token: 0x06001D57 RID: 7511 RVA: 0x0002AE59 File Offset: 0x00029059
		public void Set(ref CopyLeaderboardUserScoreByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.LeaderboardUserScoreIndex = other.LeaderboardUserScoreIndex;
			this.StatName = other.StatName;
		}

		// Token: 0x06001D58 RID: 7512 RVA: 0x0002AE80 File Offset: 0x00029080
		public void Set(ref CopyLeaderboardUserScoreByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LeaderboardUserScoreIndex = other.Value.LeaderboardUserScoreIndex;
				this.StatName = other.Value.StatName;
			}
		}

		// Token: 0x06001D59 RID: 7513 RVA: 0x0002AECB File Offset: 0x000290CB
		public void Dispose()
		{
			Helper.Dispose(ref this.m_StatName);
		}

		// Token: 0x04000CC7 RID: 3271
		private int m_ApiVersion;

		// Token: 0x04000CC8 RID: 3272
		private uint m_LeaderboardUserScoreIndex;

		// Token: 0x04000CC9 RID: 3273
		private IntPtr m_StatName;
	}
}
