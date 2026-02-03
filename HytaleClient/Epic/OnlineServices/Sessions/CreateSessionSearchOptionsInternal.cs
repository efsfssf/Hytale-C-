using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x020000FD RID: 253
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CreateSessionSearchOptionsInternal : ISettable<CreateSessionSearchOptions>, IDisposable
	{
		// Token: 0x170001C0 RID: 448
		// (set) Token: 0x0600086E RID: 2158 RVA: 0x0000C530 File Offset: 0x0000A730
		public uint MaxSearchResults
		{
			set
			{
				this.m_MaxSearchResults = value;
			}
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x0000C53A File Offset: 0x0000A73A
		public void Set(ref CreateSessionSearchOptions other)
		{
			this.m_ApiVersion = 1;
			this.MaxSearchResults = other.MaxSearchResults;
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x0000C554 File Offset: 0x0000A754
		public void Set(ref CreateSessionSearchOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.MaxSearchResults = other.Value.MaxSearchResults;
			}
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x0000C58A File Offset: 0x0000A78A
		public void Dispose()
		{
		}

		// Token: 0x0400040A RID: 1034
		private int m_ApiVersion;

		// Token: 0x0400040B RID: 1035
		private uint m_MaxSearchResults;
	}
}
