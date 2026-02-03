using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200036A RID: 874
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyLobbyMemberStatusReceivedOptionsInternal : ISettable<AddNotifyLobbyMemberStatusReceivedOptions>, IDisposable
	{
		// Token: 0x0600178E RID: 6030 RVA: 0x00022688 File Offset: 0x00020888
		public void Set(ref AddNotifyLobbyMemberStatusReceivedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x00022694 File Offset: 0x00020894
		public void Set(ref AddNotifyLobbyMemberStatusReceivedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x000226B5 File Offset: 0x000208B5
		public void Dispose()
		{
		}

		// Token: 0x04000A67 RID: 2663
		private int m_ApiVersion;
	}
}
