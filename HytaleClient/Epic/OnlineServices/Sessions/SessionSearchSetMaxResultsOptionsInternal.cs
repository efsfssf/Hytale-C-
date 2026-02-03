using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000181 RID: 385
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionSearchSetMaxResultsOptionsInternal : ISettable<SessionSearchSetMaxResultsOptions>, IDisposable
	{
		// Token: 0x17000290 RID: 656
		// (set) Token: 0x06000B3C RID: 2876 RVA: 0x0000FF3B File Offset: 0x0000E13B
		public uint MaxSearchResults
		{
			set
			{
				this.m_MaxSearchResults = value;
			}
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x0000FF45 File Offset: 0x0000E145
		public void Set(ref SessionSearchSetMaxResultsOptions other)
		{
			this.m_ApiVersion = 1;
			this.MaxSearchResults = other.MaxSearchResults;
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0000FF5C File Offset: 0x0000E15C
		public void Set(ref SessionSearchSetMaxResultsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.MaxSearchResults = other.Value.MaxSearchResults;
			}
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x0000FF92 File Offset: 0x0000E192
		public void Dispose()
		{
		}

		// Token: 0x0400051D RID: 1309
		private int m_ApiVersion;

		// Token: 0x0400051E RID: 1310
		private uint m_MaxSearchResults;
	}
}
