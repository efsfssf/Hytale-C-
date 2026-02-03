using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200036C RID: 876
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyLobbyMemberUpdateReceivedOptionsInternal : ISettable<AddNotifyLobbyMemberUpdateReceivedOptions>, IDisposable
	{
		// Token: 0x06001791 RID: 6033 RVA: 0x000226B8 File Offset: 0x000208B8
		public void Set(ref AddNotifyLobbyMemberUpdateReceivedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x000226C4 File Offset: 0x000208C4
		public void Set(ref AddNotifyLobbyMemberUpdateReceivedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x000226E5 File Offset: 0x000208E5
		public void Dispose()
		{
		}

		// Token: 0x04000A68 RID: 2664
		private int m_ApiVersion;
	}
}
