using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003FE RID: 1022
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbySearchSetLobbyIdOptionsInternal : ISettable<LobbySearchSetLobbyIdOptions>, IDisposable
	{
		// Token: 0x170007CE RID: 1998
		// (set) Token: 0x06001B67 RID: 7015 RVA: 0x0002922F File Offset: 0x0002742F
		public Utf8String LobbyId
		{
			set
			{
				Helper.Set(value, ref this.m_LobbyId);
			}
		}

		// Token: 0x06001B68 RID: 7016 RVA: 0x0002923F File Offset: 0x0002743F
		public void Set(ref LobbySearchSetLobbyIdOptions other)
		{
			this.m_ApiVersion = 1;
			this.LobbyId = other.LobbyId;
		}

		// Token: 0x06001B69 RID: 7017 RVA: 0x00029258 File Offset: 0x00027458
		public void Set(ref LobbySearchSetLobbyIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LobbyId = other.Value.LobbyId;
			}
		}

		// Token: 0x06001B6A RID: 7018 RVA: 0x0002928E File Offset: 0x0002748E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LobbyId);
		}

		// Token: 0x04000C4A RID: 3146
		private int m_ApiVersion;

		// Token: 0x04000C4B RID: 3147
		private IntPtr m_LobbyId;
	}
}
