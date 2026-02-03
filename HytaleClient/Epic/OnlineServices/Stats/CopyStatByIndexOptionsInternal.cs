using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Stats
{
	// Token: 0x020000C3 RID: 195
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyStatByIndexOptionsInternal : ISettable<CopyStatByIndexOptions>, IDisposable
	{
		// Token: 0x1700015A RID: 346
		// (set) Token: 0x0600073C RID: 1852 RVA: 0x0000A77C File Offset: 0x0000897C
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x1700015B RID: 347
		// (set) Token: 0x0600073D RID: 1853 RVA: 0x0000A78C File Offset: 0x0000898C
		public uint StatIndex
		{
			set
			{
				this.m_StatIndex = value;
			}
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0000A796 File Offset: 0x00008996
		public void Set(ref CopyStatByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.TargetUserId = other.TargetUserId;
			this.StatIndex = other.StatIndex;
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x0000A7BC File Offset: 0x000089BC
		public void Set(ref CopyStatByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.TargetUserId = other.Value.TargetUserId;
				this.StatIndex = other.Value.StatIndex;
			}
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x0000A807 File Offset: 0x00008A07
		public void Dispose()
		{
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x0400037C RID: 892
		private int m_ApiVersion;

		// Token: 0x0400037D RID: 893
		private IntPtr m_TargetUserId;

		// Token: 0x0400037E RID: 894
		private uint m_StatIndex;
	}
}
