using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000400 RID: 1024
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbySearchSetMaxResultsOptionsInternal : ISettable<LobbySearchSetMaxResultsOptions>, IDisposable
	{
		// Token: 0x170007D0 RID: 2000
		// (set) Token: 0x06001B6D RID: 7021 RVA: 0x000292AE File Offset: 0x000274AE
		public uint MaxResults
		{
			set
			{
				this.m_MaxResults = value;
			}
		}

		// Token: 0x06001B6E RID: 7022 RVA: 0x000292B8 File Offset: 0x000274B8
		public void Set(ref LobbySearchSetMaxResultsOptions other)
		{
			this.m_ApiVersion = 1;
			this.MaxResults = other.MaxResults;
		}

		// Token: 0x06001B6F RID: 7023 RVA: 0x000292D0 File Offset: 0x000274D0
		public void Set(ref LobbySearchSetMaxResultsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.MaxResults = other.Value.MaxResults;
			}
		}

		// Token: 0x06001B70 RID: 7024 RVA: 0x00029306 File Offset: 0x00027506
		public void Dispose()
		{
		}

		// Token: 0x04000C4D RID: 3149
		private int m_ApiVersion;

		// Token: 0x04000C4E RID: 3150
		private uint m_MaxResults;
	}
}
