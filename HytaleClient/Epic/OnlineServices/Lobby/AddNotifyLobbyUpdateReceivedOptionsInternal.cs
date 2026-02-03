using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200036E RID: 878
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyLobbyUpdateReceivedOptionsInternal : ISettable<AddNotifyLobbyUpdateReceivedOptions>, IDisposable
	{
		// Token: 0x06001794 RID: 6036 RVA: 0x000226E8 File Offset: 0x000208E8
		public void Set(ref AddNotifyLobbyUpdateReceivedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x000226F4 File Offset: 0x000208F4
		public void Set(ref AddNotifyLobbyUpdateReceivedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x00022715 File Offset: 0x00020915
		public void Dispose()
		{
		}

		// Token: 0x04000A69 RID: 2665
		private int m_ApiVersion;
	}
}
