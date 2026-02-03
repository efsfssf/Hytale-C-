using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000384 RID: 900
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CreateLobbySearchOptionsInternal : ISettable<CreateLobbySearchOptions>, IDisposable
	{
		// Token: 0x170006B6 RID: 1718
		// (set) Token: 0x06001833 RID: 6195 RVA: 0x000236A4 File Offset: 0x000218A4
		public uint MaxResults
		{
			set
			{
				this.m_MaxResults = value;
			}
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x000236AE File Offset: 0x000218AE
		public void Set(ref CreateLobbySearchOptions other)
		{
			this.m_ApiVersion = 1;
			this.MaxResults = other.MaxResults;
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x000236C8 File Offset: 0x000218C8
		public void Set(ref CreateLobbySearchOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.MaxResults = other.Value.MaxResults;
			}
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x000236FE File Offset: 0x000218FE
		public void Dispose()
		{
		}

		// Token: 0x04000AB6 RID: 2742
		private int m_ApiVersion;

		// Token: 0x04000AB7 RID: 2743
		private uint m_MaxResults;
	}
}
