using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000360 RID: 864
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyJoinLobbyAcceptedOptionsInternal : ISettable<AddNotifyJoinLobbyAcceptedOptions>, IDisposable
	{
		// Token: 0x0600177F RID: 6015 RVA: 0x0002259A File Offset: 0x0002079A
		public void Set(ref AddNotifyJoinLobbyAcceptedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x000225A4 File Offset: 0x000207A4
		public void Set(ref AddNotifyJoinLobbyAcceptedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x000225C5 File Offset: 0x000207C5
		public void Dispose()
		{
		}

		// Token: 0x04000A62 RID: 2658
		private int m_ApiVersion;
	}
}
