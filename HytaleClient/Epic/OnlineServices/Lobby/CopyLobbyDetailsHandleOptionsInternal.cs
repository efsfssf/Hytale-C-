using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200037E RID: 894
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyLobbyDetailsHandleOptionsInternal : ISettable<CopyLobbyDetailsHandleOptions>, IDisposable
	{
		// Token: 0x1700068E RID: 1678
		// (set) Token: 0x060017E9 RID: 6121 RVA: 0x00022FB7 File Offset: 0x000211B7
		public Utf8String LobbyId
		{
			set
			{
				Helper.Set(value, ref this.m_LobbyId);
			}
		}

		// Token: 0x1700068F RID: 1679
		// (set) Token: 0x060017EA RID: 6122 RVA: 0x00022FC7 File Offset: 0x000211C7
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x00022FD7 File Offset: 0x000211D7
		public void Set(ref CopyLobbyDetailsHandleOptions other)
		{
			this.m_ApiVersion = 1;
			this.LobbyId = other.LobbyId;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x00022FFC File Offset: 0x000211FC
		public void Set(ref CopyLobbyDetailsHandleOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LobbyId = other.Value.LobbyId;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x00023047 File Offset: 0x00021247
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LobbyId);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000A8C RID: 2700
		private int m_ApiVersion;

		// Token: 0x04000A8D RID: 2701
		private IntPtr m_LobbyId;

		// Token: 0x04000A8E RID: 2702
		private IntPtr m_LocalUserId;
	}
}
