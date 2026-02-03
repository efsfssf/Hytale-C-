using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x02000456 RID: 1110
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyLeaderboardDefinitionByIndexOptionsInternal : ISettable<CopyLeaderboardDefinitionByIndexOptions>, IDisposable
	{
		// Token: 0x1700083A RID: 2106
		// (set) Token: 0x06001D3B RID: 7483 RVA: 0x0002AC56 File Offset: 0x00028E56
		public uint LeaderboardIndex
		{
			set
			{
				this.m_LeaderboardIndex = value;
			}
		}

		// Token: 0x06001D3C RID: 7484 RVA: 0x0002AC60 File Offset: 0x00028E60
		public void Set(ref CopyLeaderboardDefinitionByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.LeaderboardIndex = other.LeaderboardIndex;
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x0002AC78 File Offset: 0x00028E78
		public void Set(ref CopyLeaderboardDefinitionByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LeaderboardIndex = other.Value.LeaderboardIndex;
			}
		}

		// Token: 0x06001D3E RID: 7486 RVA: 0x0002ACAE File Offset: 0x00028EAE
		public void Dispose()
		{
		}

		// Token: 0x04000CBA RID: 3258
		private int m_ApiVersion;

		// Token: 0x04000CBB RID: 3259
		private uint m_LeaderboardIndex;
	}
}
