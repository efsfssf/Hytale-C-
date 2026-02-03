using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x02000480 RID: 1152
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryLeaderboardUserScoresOptionsInternal : ISettable<QueryLeaderboardUserScoresOptions>, IDisposable
	{
		// Token: 0x17000881 RID: 2177
		// (set) Token: 0x06001E23 RID: 7715 RVA: 0x0002C193 File Offset: 0x0002A393
		public ProductUserId[] UserIds
		{
			set
			{
				Helper.Set<ProductUserId>(value, ref this.m_UserIds, out this.m_UserIdsCount);
			}
		}

		// Token: 0x17000882 RID: 2178
		// (set) Token: 0x06001E24 RID: 7716 RVA: 0x0002C1A9 File Offset: 0x0002A3A9
		public UserScoresQueryStatInfo[] StatInfo
		{
			set
			{
				Helper.Set<UserScoresQueryStatInfo, UserScoresQueryStatInfoInternal>(ref value, ref this.m_StatInfo, out this.m_StatInfoCount);
			}
		}

		// Token: 0x17000883 RID: 2179
		// (set) Token: 0x06001E25 RID: 7717 RVA: 0x0002C1C0 File Offset: 0x0002A3C0
		public DateTimeOffset? StartTime
		{
			set
			{
				Helper.Set(value, ref this.m_StartTime);
			}
		}

		// Token: 0x17000884 RID: 2180
		// (set) Token: 0x06001E26 RID: 7718 RVA: 0x0002C1D0 File Offset: 0x0002A3D0
		public DateTimeOffset? EndTime
		{
			set
			{
				Helper.Set(value, ref this.m_EndTime);
			}
		}

		// Token: 0x17000885 RID: 2181
		// (set) Token: 0x06001E27 RID: 7719 RVA: 0x0002C1E0 File Offset: 0x0002A3E0
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x0002C1F0 File Offset: 0x0002A3F0
		public void Set(ref QueryLeaderboardUserScoresOptions other)
		{
			this.m_ApiVersion = 2;
			this.UserIds = other.UserIds;
			this.StatInfo = other.StatInfo;
			this.StartTime = other.StartTime;
			this.EndTime = other.EndTime;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06001E29 RID: 7721 RVA: 0x0002C248 File Offset: 0x0002A448
		public void Set(ref QueryLeaderboardUserScoresOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.UserIds = other.Value.UserIds;
				this.StatInfo = other.Value.StatInfo;
				this.StartTime = other.Value.StartTime;
				this.EndTime = other.Value.EndTime;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06001E2A RID: 7722 RVA: 0x0002C2D2 File Offset: 0x0002A4D2
		public void Dispose()
		{
			Helper.Dispose(ref this.m_UserIds);
			Helper.Dispose(ref this.m_StatInfo);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000D22 RID: 3362
		private int m_ApiVersion;

		// Token: 0x04000D23 RID: 3363
		private IntPtr m_UserIds;

		// Token: 0x04000D24 RID: 3364
		private uint m_UserIdsCount;

		// Token: 0x04000D25 RID: 3365
		private IntPtr m_StatInfo;

		// Token: 0x04000D26 RID: 3366
		private uint m_StatInfoCount;

		// Token: 0x04000D27 RID: 3367
		private long m_StartTime;

		// Token: 0x04000D28 RID: 3368
		private long m_EndTime;

		// Token: 0x04000D29 RID: 3369
		private IntPtr m_LocalUserId;
	}
}
