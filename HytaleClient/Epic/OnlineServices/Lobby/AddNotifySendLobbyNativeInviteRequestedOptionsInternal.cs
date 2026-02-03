using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000372 RID: 882
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifySendLobbyNativeInviteRequestedOptionsInternal : ISettable<AddNotifySendLobbyNativeInviteRequestedOptions>, IDisposable
	{
		// Token: 0x060017A0 RID: 6048 RVA: 0x000227E6 File Offset: 0x000209E6
		public void Set(ref AddNotifySendLobbyNativeInviteRequestedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x000227F0 File Offset: 0x000209F0
		public void Set(ref AddNotifySendLobbyNativeInviteRequestedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x00022811 File Offset: 0x00020A11
		public void Dispose()
		{
		}

		// Token: 0x04000A6F RID: 2671
		private int m_ApiVersion;
	}
}
