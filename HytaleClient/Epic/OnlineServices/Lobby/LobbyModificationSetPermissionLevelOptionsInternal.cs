using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003ED RID: 1005
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyModificationSetPermissionLevelOptionsInternal : ISettable<LobbyModificationSetPermissionLevelOptions>, IDisposable
	{
		// Token: 0x170007BF RID: 1983
		// (set) Token: 0x06001B26 RID: 6950 RVA: 0x00028C3A File Offset: 0x00026E3A
		public LobbyPermissionLevel PermissionLevel
		{
			set
			{
				this.m_PermissionLevel = value;
			}
		}

		// Token: 0x06001B27 RID: 6951 RVA: 0x00028C44 File Offset: 0x00026E44
		public void Set(ref LobbyModificationSetPermissionLevelOptions other)
		{
			this.m_ApiVersion = 1;
			this.PermissionLevel = other.PermissionLevel;
		}

		// Token: 0x06001B28 RID: 6952 RVA: 0x00028C5C File Offset: 0x00026E5C
		public void Set(ref LobbyModificationSetPermissionLevelOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.PermissionLevel = other.Value.PermissionLevel;
			}
		}

		// Token: 0x06001B29 RID: 6953 RVA: 0x00028C92 File Offset: 0x00026E92
		public void Dispose()
		{
		}

		// Token: 0x04000C28 RID: 3112
		private int m_ApiVersion;

		// Token: 0x04000C29 RID: 3113
		private LobbyPermissionLevel m_PermissionLevel;
	}
}
