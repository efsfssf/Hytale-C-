using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x02000462 RID: 1122
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DefinitionInternal : IGettable<Definition>, ISettable<Definition>, IDisposable
	{
		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x06001D6E RID: 7534 RVA: 0x0002B04C File Offset: 0x0002924C
		// (set) Token: 0x06001D6F RID: 7535 RVA: 0x0002B06D File Offset: 0x0002926D
		public Utf8String LeaderboardId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_LeaderboardId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LeaderboardId);
			}
		}

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x06001D70 RID: 7536 RVA: 0x0002B080 File Offset: 0x00029280
		// (set) Token: 0x06001D71 RID: 7537 RVA: 0x0002B0A1 File Offset: 0x000292A1
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

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x06001D72 RID: 7538 RVA: 0x0002B0B4 File Offset: 0x000292B4
		// (set) Token: 0x06001D73 RID: 7539 RVA: 0x0002B0CC File Offset: 0x000292CC
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

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x06001D74 RID: 7540 RVA: 0x0002B0D8 File Offset: 0x000292D8
		// (set) Token: 0x06001D75 RID: 7541 RVA: 0x0002B0F9 File Offset: 0x000292F9
		public DateTimeOffset? StartTime
		{
			get
			{
				DateTimeOffset? result;
				Helper.Get(this.m_StartTime, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_StartTime);
			}
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06001D76 RID: 7542 RVA: 0x0002B10C File Offset: 0x0002930C
		// (set) Token: 0x06001D77 RID: 7543 RVA: 0x0002B12D File Offset: 0x0002932D
		public DateTimeOffset? EndTime
		{
			get
			{
				DateTimeOffset? result;
				Helper.Get(this.m_EndTime, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_EndTime);
			}
		}

		// Token: 0x06001D78 RID: 7544 RVA: 0x0002B140 File Offset: 0x00029340
		public void Set(ref Definition other)
		{
			this.m_ApiVersion = 1;
			this.LeaderboardId = other.LeaderboardId;
			this.StatName = other.StatName;
			this.Aggregation = other.Aggregation;
			this.StartTime = other.StartTime;
			this.EndTime = other.EndTime;
		}

		// Token: 0x06001D79 RID: 7545 RVA: 0x0002B198 File Offset: 0x00029398
		public void Set(ref Definition? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LeaderboardId = other.Value.LeaderboardId;
				this.StatName = other.Value.StatName;
				this.Aggregation = other.Value.Aggregation;
				this.StartTime = other.Value.StartTime;
				this.EndTime = other.Value.EndTime;
			}
		}

		// Token: 0x06001D7A RID: 7546 RVA: 0x0002B222 File Offset: 0x00029422
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LeaderboardId);
			Helper.Dispose(ref this.m_StatName);
		}

		// Token: 0x06001D7B RID: 7547 RVA: 0x0002B23D File Offset: 0x0002943D
		public void Get(out Definition output)
		{
			output = default(Definition);
			output.Set(ref this);
		}

		// Token: 0x04000CD4 RID: 3284
		private int m_ApiVersion;

		// Token: 0x04000CD5 RID: 3285
		private IntPtr m_LeaderboardId;

		// Token: 0x04000CD6 RID: 3286
		private IntPtr m_StatName;

		// Token: 0x04000CD7 RID: 3287
		private LeaderboardAggregation m_Aggregation;

		// Token: 0x04000CD8 RID: 3288
		private long m_StartTime;

		// Token: 0x04000CD9 RID: 3289
		private long m_EndTime;
	}
}
