using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000175 RID: 373
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionSearchCopySearchResultByIndexOptionsInternal : ISettable<SessionSearchCopySearchResultByIndexOptions>, IDisposable
	{
		// Token: 0x17000283 RID: 643
		// (set) Token: 0x06000B0D RID: 2829 RVA: 0x0000FC10 File Offset: 0x0000DE10
		public uint SessionIndex
		{
			set
			{
				this.m_SessionIndex = value;
			}
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x0000FC1A File Offset: 0x0000DE1A
		public void Set(ref SessionSearchCopySearchResultByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.SessionIndex = other.SessionIndex;
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x0000FC34 File Offset: 0x0000DE34
		public void Set(ref SessionSearchCopySearchResultByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.SessionIndex = other.Value.SessionIndex;
			}
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x0000FC6A File Offset: 0x0000DE6A
		public void Dispose()
		{
		}

		// Token: 0x0400050D RID: 1293
		private int m_ApiVersion;

		// Token: 0x0400050E RID: 1294
		private uint m_SessionIndex;
	}
}
