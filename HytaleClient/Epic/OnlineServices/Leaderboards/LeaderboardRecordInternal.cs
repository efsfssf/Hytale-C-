using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x0200046B RID: 1131
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LeaderboardRecordInternal : IGettable<LeaderboardRecord>, ISettable<LeaderboardRecord>, IDisposable
	{
		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06001D91 RID: 7569 RVA: 0x0002B3A8 File Offset: 0x000295A8
		// (set) Token: 0x06001D92 RID: 7570 RVA: 0x0002B3C9 File Offset: 0x000295C9
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

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06001D93 RID: 7571 RVA: 0x0002B3DC File Offset: 0x000295DC
		// (set) Token: 0x06001D94 RID: 7572 RVA: 0x0002B3F4 File Offset: 0x000295F4
		public uint Rank
		{
			get
			{
				return this.m_Rank;
			}
			set
			{
				this.m_Rank = value;
			}
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06001D95 RID: 7573 RVA: 0x0002B400 File Offset: 0x00029600
		// (set) Token: 0x06001D96 RID: 7574 RVA: 0x0002B418 File Offset: 0x00029618
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

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06001D97 RID: 7575 RVA: 0x0002B424 File Offset: 0x00029624
		// (set) Token: 0x06001D98 RID: 7576 RVA: 0x0002B445 File Offset: 0x00029645
		public Utf8String UserDisplayName
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_UserDisplayName, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_UserDisplayName);
			}
		}

		// Token: 0x06001D99 RID: 7577 RVA: 0x0002B455 File Offset: 0x00029655
		public void Set(ref LeaderboardRecord other)
		{
			this.m_ApiVersion = 2;
			this.UserId = other.UserId;
			this.Rank = other.Rank;
			this.Score = other.Score;
			this.UserDisplayName = other.UserDisplayName;
		}

		// Token: 0x06001D9A RID: 7578 RVA: 0x0002B494 File Offset: 0x00029694
		public void Set(ref LeaderboardRecord? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.UserId = other.Value.UserId;
				this.Rank = other.Value.Rank;
				this.Score = other.Value.Score;
				this.UserDisplayName = other.Value.UserDisplayName;
			}
		}

		// Token: 0x06001D9B RID: 7579 RVA: 0x0002B509 File Offset: 0x00029709
		public void Dispose()
		{
			Helper.Dispose(ref this.m_UserId);
			Helper.Dispose(ref this.m_UserDisplayName);
		}

		// Token: 0x06001D9C RID: 7580 RVA: 0x0002B524 File Offset: 0x00029724
		public void Get(out LeaderboardRecord output)
		{
			output = default(LeaderboardRecord);
			output.Set(ref this);
		}

		// Token: 0x04000CE8 RID: 3304
		private int m_ApiVersion;

		// Token: 0x04000CE9 RID: 3305
		private IntPtr m_UserId;

		// Token: 0x04000CEA RID: 3306
		private uint m_Rank;

		// Token: 0x04000CEB RID: 3307
		private int m_Score;

		// Token: 0x04000CEC RID: 3308
		private IntPtr m_UserDisplayName;
	}
}
