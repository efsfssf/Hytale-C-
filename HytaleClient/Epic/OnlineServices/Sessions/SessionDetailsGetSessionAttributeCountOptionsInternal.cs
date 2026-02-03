using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000155 RID: 341
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionDetailsGetSessionAttributeCountOptionsInternal : ISettable<SessionDetailsGetSessionAttributeCountOptions>, IDisposable
	{
		// Token: 0x06000A2C RID: 2604 RVA: 0x0000E371 File Offset: 0x0000C571
		public void Set(ref SessionDetailsGetSessionAttributeCountOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x0000E37C File Offset: 0x0000C57C
		public void Set(ref SessionDetailsGetSessionAttributeCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x0000E39D File Offset: 0x0000C59D
		public void Dispose()
		{
		}

		// Token: 0x0400049F RID: 1183
		private int m_ApiVersion;
	}
}
