using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x02000458 RID: 1112
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyLeaderboardDefinitionByLeaderboardIdOptionsInternal : ISettable<CopyLeaderboardDefinitionByLeaderboardIdOptions>, IDisposable
	{
		// Token: 0x1700083C RID: 2108
		// (set) Token: 0x06001D41 RID: 7489 RVA: 0x0002ACC2 File Offset: 0x00028EC2
		public Utf8String LeaderboardId
		{
			set
			{
				Helper.Set(value, ref this.m_LeaderboardId);
			}
		}

		// Token: 0x06001D42 RID: 7490 RVA: 0x0002ACD2 File Offset: 0x00028ED2
		public void Set(ref CopyLeaderboardDefinitionByLeaderboardIdOptions other)
		{
			this.m_ApiVersion = 1;
			this.LeaderboardId = other.LeaderboardId;
		}

		// Token: 0x06001D43 RID: 7491 RVA: 0x0002ACEC File Offset: 0x00028EEC
		public void Set(ref CopyLeaderboardDefinitionByLeaderboardIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LeaderboardId = other.Value.LeaderboardId;
			}
		}

		// Token: 0x06001D44 RID: 7492 RVA: 0x0002AD22 File Offset: 0x00028F22
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LeaderboardId);
		}

		// Token: 0x04000CBD RID: 3261
		private int m_ApiVersion;

		// Token: 0x04000CBE RID: 3262
		private IntPtr m_LeaderboardId;
	}
}
