using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x02000468 RID: 1128
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetLeaderboardUserScoreCountOptionsInternal : ISettable<GetLeaderboardUserScoreCountOptions>, IDisposable
	{
		// Token: 0x17000854 RID: 2132
		// (set) Token: 0x06001D84 RID: 7556 RVA: 0x0002B2C1 File Offset: 0x000294C1
		public Utf8String StatName
		{
			set
			{
				Helper.Set(value, ref this.m_StatName);
			}
		}

		// Token: 0x06001D85 RID: 7557 RVA: 0x0002B2D1 File Offset: 0x000294D1
		public void Set(ref GetLeaderboardUserScoreCountOptions other)
		{
			this.m_ApiVersion = 1;
			this.StatName = other.StatName;
		}

		// Token: 0x06001D86 RID: 7558 RVA: 0x0002B2E8 File Offset: 0x000294E8
		public void Set(ref GetLeaderboardUserScoreCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.StatName = other.Value.StatName;
			}
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x0002B31E File Offset: 0x0002951E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_StatName);
		}

		// Token: 0x04000CDD RID: 3293
		private int m_ApiVersion;

		// Token: 0x04000CDE RID: 3294
		private IntPtr m_StatName;
	}
}
