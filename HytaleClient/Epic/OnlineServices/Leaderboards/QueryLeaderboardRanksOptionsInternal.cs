using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x0200047E RID: 1150
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryLeaderboardRanksOptionsInternal : ISettable<QueryLeaderboardRanksOptions>, IDisposable
	{
		// Token: 0x1700087A RID: 2170
		// (set) Token: 0x06001E14 RID: 7700 RVA: 0x0002C091 File Offset: 0x0002A291
		public Utf8String LeaderboardId
		{
			set
			{
				Helper.Set(value, ref this.m_LeaderboardId);
			}
		}

		// Token: 0x1700087B RID: 2171
		// (set) Token: 0x06001E15 RID: 7701 RVA: 0x0002C0A1 File Offset: 0x0002A2A1
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06001E16 RID: 7702 RVA: 0x0002C0B1 File Offset: 0x0002A2B1
		public void Set(ref QueryLeaderboardRanksOptions other)
		{
			this.m_ApiVersion = 2;
			this.LeaderboardId = other.LeaderboardId;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06001E17 RID: 7703 RVA: 0x0002C0D8 File Offset: 0x0002A2D8
		public void Set(ref QueryLeaderboardRanksOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.LeaderboardId = other.Value.LeaderboardId;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06001E18 RID: 7704 RVA: 0x0002C123 File Offset: 0x0002A323
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LeaderboardId);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000D1A RID: 3354
		private int m_ApiVersion;

		// Token: 0x04000D1B RID: 3355
		private IntPtr m_LeaderboardId;

		// Token: 0x04000D1C RID: 3356
		private IntPtr m_LocalUserId;
	}
}
