using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003C4 RID: 964
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyDetailsGetLobbyOwnerOptionsInternal : ISettable<LobbyDetailsGetLobbyOwnerOptions>, IDisposable
	{
		// Token: 0x060019C7 RID: 6599 RVA: 0x00025E1C File Offset: 0x0002401C
		public void Set(ref LobbyDetailsGetLobbyOwnerOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x00025E28 File Offset: 0x00024028
		public void Set(ref LobbyDetailsGetLobbyOwnerOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x060019C9 RID: 6601 RVA: 0x00025E49 File Offset: 0x00024049
		public void Dispose()
		{
		}

		// Token: 0x04000B72 RID: 2930
		private int m_ApiVersion;
	}
}
