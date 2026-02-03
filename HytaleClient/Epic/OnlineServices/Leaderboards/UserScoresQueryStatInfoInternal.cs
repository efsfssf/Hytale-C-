using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x02000482 RID: 1154
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UserScoresQueryStatInfoInternal : IGettable<UserScoresQueryStatInfo>, ISettable<UserScoresQueryStatInfo>, IDisposable
	{
		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06001E30 RID: 7728 RVA: 0x0002C338 File Offset: 0x0002A538
		// (set) Token: 0x06001E31 RID: 7729 RVA: 0x0002C359 File Offset: 0x0002A559
		public Utf8String StatName
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_StatName, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_StatName);
			}
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06001E32 RID: 7730 RVA: 0x0002C36C File Offset: 0x0002A56C
		// (set) Token: 0x06001E33 RID: 7731 RVA: 0x0002C384 File Offset: 0x0002A584
		public LeaderboardAggregation Aggregation
		{
			get
			{
				return this.m_Aggregation;
			}
			set
			{
				this.m_Aggregation = value;
			}
		}

		// Token: 0x06001E34 RID: 7732 RVA: 0x0002C38E File Offset: 0x0002A58E
		public void Set(ref UserScoresQueryStatInfo other)
		{
			this.m_ApiVersion = 1;
			this.StatName = other.StatName;
			this.Aggregation = other.Aggregation;
		}

		// Token: 0x06001E35 RID: 7733 RVA: 0x0002C3B4 File Offset: 0x0002A5B4
		public void Set(ref UserScoresQueryStatInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.StatName = other.Value.StatName;
				this.Aggregation = other.Value.Aggregation;
			}
		}

		// Token: 0x06001E36 RID: 7734 RVA: 0x0002C3FF File Offset: 0x0002A5FF
		public void Dispose()
		{
			Helper.Dispose(ref this.m_StatName);
		}

		// Token: 0x06001E37 RID: 7735 RVA: 0x0002C40E File Offset: 0x0002A60E
		public void Get(out UserScoresQueryStatInfo output)
		{
			output = default(UserScoresQueryStatInfo);
			output.Set(ref this);
		}

		// Token: 0x04000D2C RID: 3372
		private int m_ApiVersion;

		// Token: 0x04000D2D RID: 3373
		private IntPtr m_StatName;

		// Token: 0x04000D2E RID: 3374
		private LeaderboardAggregation m_Aggregation;
	}
}
