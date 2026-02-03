using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003E9 RID: 1001
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyModificationSetInvitesAllowedOptionsInternal : ISettable<LobbyModificationSetInvitesAllowedOptions>, IDisposable
	{
		// Token: 0x170007BB RID: 1979
		// (set) Token: 0x06001B1A RID: 6938 RVA: 0x00028B5A File Offset: 0x00026D5A
		public bool InvitesAllowed
		{
			set
			{
				Helper.Set(value, ref this.m_InvitesAllowed);
			}
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x00028B6A File Offset: 0x00026D6A
		public void Set(ref LobbyModificationSetInvitesAllowedOptions other)
		{
			this.m_ApiVersion = 1;
			this.InvitesAllowed = other.InvitesAllowed;
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x00028B84 File Offset: 0x00026D84
		public void Set(ref LobbyModificationSetInvitesAllowedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.InvitesAllowed = other.Value.InvitesAllowed;
			}
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x00028BBA File Offset: 0x00026DBA
		public void Dispose()
		{
		}

		// Token: 0x04000C22 RID: 3106
		private int m_ApiVersion;

		// Token: 0x04000C23 RID: 3107
		private int m_InvitesAllowed;
	}
}
