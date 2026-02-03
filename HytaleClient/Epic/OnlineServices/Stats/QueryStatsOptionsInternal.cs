using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Stats
{
	// Token: 0x020000D5 RID: 213
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryStatsOptionsInternal : ISettable<QueryStatsOptions>, IDisposable
	{
		// Token: 0x17000183 RID: 387
		// (set) Token: 0x060007B1 RID: 1969 RVA: 0x0000B098 File Offset: 0x00009298
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000184 RID: 388
		// (set) Token: 0x060007B2 RID: 1970 RVA: 0x0000B0A8 File Offset: 0x000092A8
		public DateTimeOffset? StartTime
		{
			set
			{
				Helper.Set(value, ref this.m_StartTime);
			}
		}

		// Token: 0x17000185 RID: 389
		// (set) Token: 0x060007B3 RID: 1971 RVA: 0x0000B0B8 File Offset: 0x000092B8
		public DateTimeOffset? EndTime
		{
			set
			{
				Helper.Set(value, ref this.m_EndTime);
			}
		}

		// Token: 0x17000186 RID: 390
		// (set) Token: 0x060007B4 RID: 1972 RVA: 0x0000B0C8 File Offset: 0x000092C8
		public Utf8String[] StatNames
		{
			set
			{
				Helper.Set<Utf8String>(value, ref this.m_StatNames, true, out this.m_StatNamesCount);
			}
		}

		// Token: 0x17000187 RID: 391
		// (set) Token: 0x060007B5 RID: 1973 RVA: 0x0000B0DF File Offset: 0x000092DF
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0000B0F0 File Offset: 0x000092F0
		public void Set(ref QueryStatsOptions other)
		{
			this.m_ApiVersion = 3;
			this.LocalUserId = other.LocalUserId;
			this.StartTime = other.StartTime;
			this.EndTime = other.EndTime;
			this.StatNames = other.StatNames;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0000B148 File Offset: 0x00009348
		public void Set(ref QueryStatsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 3;
				this.LocalUserId = other.Value.LocalUserId;
				this.StartTime = other.Value.StartTime;
				this.EndTime = other.Value.EndTime;
				this.StatNames = other.Value.StatNames;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0000B1D2 File Offset: 0x000093D2
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_StatNames);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x040003A9 RID: 937
		private int m_ApiVersion;

		// Token: 0x040003AA RID: 938
		private IntPtr m_LocalUserId;

		// Token: 0x040003AB RID: 939
		private long m_StartTime;

		// Token: 0x040003AC RID: 940
		private long m_EndTime;

		// Token: 0x040003AD RID: 941
		private IntPtr m_StatNames;

		// Token: 0x040003AE RID: 942
		private uint m_StatNamesCount;

		// Token: 0x040003AF RID: 943
		private IntPtr m_TargetUserId;
	}
}
