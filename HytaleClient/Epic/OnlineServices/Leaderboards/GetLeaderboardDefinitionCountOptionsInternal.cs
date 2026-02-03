using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x02000464 RID: 1124
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetLeaderboardDefinitionCountOptionsInternal : ISettable<GetLeaderboardDefinitionCountOptions>, IDisposable
	{
		// Token: 0x06001D7C RID: 7548 RVA: 0x0002B24F File Offset: 0x0002944F
		public void Set(ref GetLeaderboardDefinitionCountOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06001D7D RID: 7549 RVA: 0x0002B25C File Offset: 0x0002945C
		public void Set(ref GetLeaderboardDefinitionCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06001D7E RID: 7550 RVA: 0x0002B27D File Offset: 0x0002947D
		public void Dispose()
		{
		}

		// Token: 0x04000CDA RID: 3290
		private int m_ApiVersion;
	}
}
