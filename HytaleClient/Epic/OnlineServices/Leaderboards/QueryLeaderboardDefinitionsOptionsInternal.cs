using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x0200047C RID: 1148
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryLeaderboardDefinitionsOptionsInternal : ISettable<QueryLeaderboardDefinitionsOptions>, IDisposable
	{
		// Token: 0x17000875 RID: 2165
		// (set) Token: 0x06001E0A RID: 7690 RVA: 0x0002BF9C File Offset: 0x0002A19C
		public DateTimeOffset? StartTime
		{
			set
			{
				Helper.Set(value, ref this.m_StartTime);
			}
		}

		// Token: 0x17000876 RID: 2166
		// (set) Token: 0x06001E0B RID: 7691 RVA: 0x0002BFAC File Offset: 0x0002A1AC
		public DateTimeOffset? EndTime
		{
			set
			{
				Helper.Set(value, ref this.m_EndTime);
			}
		}

		// Token: 0x17000877 RID: 2167
		// (set) Token: 0x06001E0C RID: 7692 RVA: 0x0002BFBC File Offset: 0x0002A1BC
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06001E0D RID: 7693 RVA: 0x0002BFCC File Offset: 0x0002A1CC
		public void Set(ref QueryLeaderboardDefinitionsOptions other)
		{
			this.m_ApiVersion = 2;
			this.StartTime = other.StartTime;
			this.EndTime = other.EndTime;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06001E0E RID: 7694 RVA: 0x0002C000 File Offset: 0x0002A200
		public void Set(ref QueryLeaderboardDefinitionsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.StartTime = other.Value.StartTime;
				this.EndTime = other.Value.EndTime;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06001E0F RID: 7695 RVA: 0x0002C060 File Offset: 0x0002A260
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000D14 RID: 3348
		private int m_ApiVersion;

		// Token: 0x04000D15 RID: 3349
		private long m_StartTime;

		// Token: 0x04000D16 RID: 3350
		private long m_EndTime;

		// Token: 0x04000D17 RID: 3351
		private IntPtr m_LocalUserId;
	}
}
