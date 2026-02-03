using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003EB RID: 1003
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyModificationSetMaxMembersOptionsInternal : ISettable<LobbyModificationSetMaxMembersOptions>, IDisposable
	{
		// Token: 0x170007BD RID: 1981
		// (set) Token: 0x06001B20 RID: 6944 RVA: 0x00028BCE File Offset: 0x00026DCE
		public uint MaxMembers
		{
			set
			{
				this.m_MaxMembers = value;
			}
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x00028BD8 File Offset: 0x00026DD8
		public void Set(ref LobbyModificationSetMaxMembersOptions other)
		{
			this.m_ApiVersion = 1;
			this.MaxMembers = other.MaxMembers;
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x00028BF0 File Offset: 0x00026DF0
		public void Set(ref LobbyModificationSetMaxMembersOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.MaxMembers = other.Value.MaxMembers;
			}
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x00028C26 File Offset: 0x00026E26
		public void Dispose()
		{
		}

		// Token: 0x04000C25 RID: 3109
		private int m_ApiVersion;

		// Token: 0x04000C26 RID: 3110
		private uint m_MaxMembers;
	}
}
