using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000364 RID: 868
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyLobbyInviteAcceptedOptionsInternal : ISettable<AddNotifyLobbyInviteAcceptedOptions>, IDisposable
	{
		// Token: 0x06001785 RID: 6021 RVA: 0x000225F8 File Offset: 0x000207F8
		public void Set(ref AddNotifyLobbyInviteAcceptedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x00022604 File Offset: 0x00020804
		public void Set(ref AddNotifyLobbyInviteAcceptedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x00022625 File Offset: 0x00020825
		public void Dispose()
		{
		}

		// Token: 0x04000A64 RID: 2660
		private int m_ApiVersion;
	}
}
