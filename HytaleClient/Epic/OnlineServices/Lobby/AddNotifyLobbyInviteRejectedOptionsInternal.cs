using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000368 RID: 872
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyLobbyInviteRejectedOptionsInternal : ISettable<AddNotifyLobbyInviteRejectedOptions>, IDisposable
	{
		// Token: 0x0600178B RID: 6027 RVA: 0x00022658 File Offset: 0x00020858
		public void Set(ref AddNotifyLobbyInviteRejectedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x0600178C RID: 6028 RVA: 0x00022664 File Offset: 0x00020864
		public void Set(ref AddNotifyLobbyInviteRejectedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x00022685 File Offset: 0x00020885
		public void Dispose()
		{
		}

		// Token: 0x04000A66 RID: 2662
		private int m_ApiVersion;
	}
}
