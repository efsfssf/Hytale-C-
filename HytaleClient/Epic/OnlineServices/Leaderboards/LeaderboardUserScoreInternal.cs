using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x0200046E RID: 1134
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LeaderboardUserScoreInternal : IGettable<LeaderboardUserScore>, ISettable<LeaderboardUserScore>, IDisposable
	{
		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x06001DB3 RID: 7603 RVA: 0x0002BA0C File Offset: 0x00029C0C
		// (set) Token: 0x06001DB4 RID: 7604 RVA: 0x0002BA2D File Offset: 0x00029C2D
		public ProductUserId UserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_UserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_UserId);
			}
		}

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06001DB5 RID: 7605 RVA: 0x0002BA40 File Offset: 0x00029C40
		// (set) Token: 0x06001DB6 RID: 7606 RVA: 0x0002BA58 File Offset: 0x00029C58
		public int Score
		{
			get
			{
				return this.m_Score;
			}
			set
			{
				this.m_Score = value;
			}
		}

		// Token: 0x06001DB7 RID: 7607 RVA: 0x0002BA62 File Offset: 0x00029C62
		public void Set(ref LeaderboardUserScore other)
		{
			this.m_ApiVersion = 1;
			this.UserId = other.UserId;
			this.Score = other.Score;
		}

		// Token: 0x06001DB8 RID: 7608 RVA: 0x0002BA88 File Offset: 0x00029C88
		public void Set(ref LeaderboardUserScore? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.UserId = other.Value.UserId;
				this.Score = other.Value.Score;
			}
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x0002BAD3 File Offset: 0x00029CD3
		public void Dispose()
		{
			Helper.Dispose(ref this.m_UserId);
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x0002BAE2 File Offset: 0x00029CE2
		public void Get(out LeaderboardUserScore output)
		{
			output = default(LeaderboardUserScore);
			output.Set(ref this);
		}

		// Token: 0x04000D00 RID: 3328
		private int m_ApiVersion;

		// Token: 0x04000D01 RID: 3329
		private IntPtr m_UserId;

		// Token: 0x04000D02 RID: 3330
		private int m_Score;
	}
}
