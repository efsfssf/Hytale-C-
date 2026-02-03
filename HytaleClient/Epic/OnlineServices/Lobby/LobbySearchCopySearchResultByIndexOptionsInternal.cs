using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003F2 RID: 1010
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbySearchCopySearchResultByIndexOptionsInternal : ISettable<LobbySearchCopySearchResultByIndexOptions>, IDisposable
	{
		// Token: 0x170007C1 RID: 1985
		// (set) Token: 0x06001B38 RID: 6968 RVA: 0x00028F04 File Offset: 0x00027104
		public uint LobbyIndex
		{
			set
			{
				this.m_LobbyIndex = value;
			}
		}

		// Token: 0x06001B39 RID: 6969 RVA: 0x00028F0E File Offset: 0x0002710E
		public void Set(ref LobbySearchCopySearchResultByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.LobbyIndex = other.LobbyIndex;
		}

		// Token: 0x06001B3A RID: 6970 RVA: 0x00028F28 File Offset: 0x00027128
		public void Set(ref LobbySearchCopySearchResultByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LobbyIndex = other.Value.LobbyIndex;
			}
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x00028F5E File Offset: 0x0002715E
		public void Dispose()
		{
		}

		// Token: 0x04000C3A RID: 3130
		private int m_ApiVersion;

		// Token: 0x04000C3B RID: 3131
		private uint m_LobbyIndex;
	}
}
