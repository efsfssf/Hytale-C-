using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x02000466 RID: 1126
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetLeaderboardRecordCountOptionsInternal : ISettable<GetLeaderboardRecordCountOptions>, IDisposable
	{
		// Token: 0x06001D7F RID: 7551 RVA: 0x0002B280 File Offset: 0x00029480
		public void Set(ref GetLeaderboardRecordCountOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06001D80 RID: 7552 RVA: 0x0002B28C File Offset: 0x0002948C
		public void Set(ref GetLeaderboardRecordCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06001D81 RID: 7553 RVA: 0x0002B2AD File Offset: 0x000294AD
		public void Dispose()
		{
		}

		// Token: 0x04000CDB RID: 3291
		private int m_ApiVersion;
	}
}
