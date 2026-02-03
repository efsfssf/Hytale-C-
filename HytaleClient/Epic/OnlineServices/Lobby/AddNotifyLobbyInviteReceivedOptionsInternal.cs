using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000366 RID: 870
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyLobbyInviteReceivedOptionsInternal : ISettable<AddNotifyLobbyInviteReceivedOptions>, IDisposable
	{
		// Token: 0x06001788 RID: 6024 RVA: 0x00022628 File Offset: 0x00020828
		public void Set(ref AddNotifyLobbyInviteReceivedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06001789 RID: 6025 RVA: 0x00022634 File Offset: 0x00020834
		public void Set(ref AddNotifyLobbyInviteReceivedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x0600178A RID: 6026 RVA: 0x00022655 File Offset: 0x00020855
		public void Dispose()
		{
		}

		// Token: 0x04000A65 RID: 2661
		private int m_ApiVersion;
	}
}
