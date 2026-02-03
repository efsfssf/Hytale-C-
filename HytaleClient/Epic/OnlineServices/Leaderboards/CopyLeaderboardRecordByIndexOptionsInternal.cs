using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x0200045A RID: 1114
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyLeaderboardRecordByIndexOptionsInternal : ISettable<CopyLeaderboardRecordByIndexOptions>, IDisposable
	{
		// Token: 0x1700083E RID: 2110
		// (set) Token: 0x06001D47 RID: 7495 RVA: 0x0002AD42 File Offset: 0x00028F42
		public uint LeaderboardRecordIndex
		{
			set
			{
				this.m_LeaderboardRecordIndex = value;
			}
		}

		// Token: 0x06001D48 RID: 7496 RVA: 0x0002AD4C File Offset: 0x00028F4C
		public void Set(ref CopyLeaderboardRecordByIndexOptions other)
		{
			this.m_ApiVersion = 2;
			this.LeaderboardRecordIndex = other.LeaderboardRecordIndex;
		}

		// Token: 0x06001D49 RID: 7497 RVA: 0x0002AD64 File Offset: 0x00028F64
		public void Set(ref CopyLeaderboardRecordByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.LeaderboardRecordIndex = other.Value.LeaderboardRecordIndex;
			}
		}

		// Token: 0x06001D4A RID: 7498 RVA: 0x0002AD9A File Offset: 0x00028F9A
		public void Dispose()
		{
		}

		// Token: 0x04000CC0 RID: 3264
		private int m_ApiVersion;

		// Token: 0x04000CC1 RID: 3265
		private uint m_LeaderboardRecordIndex;
	}
}
