using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000454 RID: 1108
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UpdateLobbyOptionsInternal : ISettable<UpdateLobbyOptions>, IDisposable
	{
		// Token: 0x17000838 RID: 2104
		// (set) Token: 0x06001D35 RID: 7477 RVA: 0x0002ABD7 File Offset: 0x00028DD7
		public LobbyModification LobbyModificationHandle
		{
			set
			{
				Helper.Set(value, ref this.m_LobbyModificationHandle);
			}
		}

		// Token: 0x06001D36 RID: 7478 RVA: 0x0002ABE7 File Offset: 0x00028DE7
		public void Set(ref UpdateLobbyOptions other)
		{
			this.m_ApiVersion = 1;
			this.LobbyModificationHandle = other.LobbyModificationHandle;
		}

		// Token: 0x06001D37 RID: 7479 RVA: 0x0002AC00 File Offset: 0x00028E00
		public void Set(ref UpdateLobbyOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LobbyModificationHandle = other.Value.LobbyModificationHandle;
			}
		}

		// Token: 0x06001D38 RID: 7480 RVA: 0x0002AC36 File Offset: 0x00028E36
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LobbyModificationHandle);
		}

		// Token: 0x04000CB7 RID: 3255
		private int m_ApiVersion;

		// Token: 0x04000CB8 RID: 3256
		private IntPtr m_LobbyModificationHandle;
	}
}
