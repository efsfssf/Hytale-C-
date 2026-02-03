using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200017B RID: 379
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionSearchGetSearchResultCountOptionsInternal : ISettable<SessionSearchGetSearchResultCountOptions>, IDisposable
	{
		// Token: 0x06000B26 RID: 2854 RVA: 0x0000FE41 File Offset: 0x0000E041
		public void Set(ref SessionSearchGetSearchResultCountOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x0000FE4C File Offset: 0x0000E04C
		public void Set(ref SessionSearchGetSearchResultCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x0000FE6D File Offset: 0x0000E06D
		public void Dispose()
		{
		}

		// Token: 0x04000516 RID: 1302
		private int m_ApiVersion;
	}
}
