using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Stats
{
	// Token: 0x020000CD RID: 205
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct IngestStatOptionsInternal : ISettable<IngestStatOptions>, IDisposable
	{
		// Token: 0x17000172 RID: 370
		// (set) Token: 0x0600077A RID: 1914 RVA: 0x0000AD06 File Offset: 0x00008F06
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000173 RID: 371
		// (set) Token: 0x0600077B RID: 1915 RVA: 0x0000AD16 File Offset: 0x00008F16
		public IngestData[] Stats
		{
			set
			{
				Helper.Set<IngestData, IngestDataInternal>(ref value, ref this.m_Stats, out this.m_StatsCount);
			}
		}

		// Token: 0x17000174 RID: 372
		// (set) Token: 0x0600077C RID: 1916 RVA: 0x0000AD2D File Offset: 0x00008F2D
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0000AD3D File Offset: 0x00008F3D
		public void Set(ref IngestStatOptions other)
		{
			this.m_ApiVersion = 3;
			this.LocalUserId = other.LocalUserId;
			this.Stats = other.Stats;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0000AD70 File Offset: 0x00008F70
		public void Set(ref IngestStatOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 3;
				this.LocalUserId = other.Value.LocalUserId;
				this.Stats = other.Value.Stats;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0000ADD0 File Offset: 0x00008FD0
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_Stats);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04000397 RID: 919
		private int m_ApiVersion;

		// Token: 0x04000398 RID: 920
		private IntPtr m_LocalUserId;

		// Token: 0x04000399 RID: 921
		private IntPtr m_Stats;

		// Token: 0x0400039A RID: 922
		private uint m_StatsCount;

		// Token: 0x0400039B RID: 923
		private IntPtr m_TargetUserId;
	}
}
