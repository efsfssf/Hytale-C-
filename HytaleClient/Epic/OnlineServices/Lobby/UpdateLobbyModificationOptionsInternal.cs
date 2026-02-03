using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000452 RID: 1106
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UpdateLobbyModificationOptionsInternal : ISettable<UpdateLobbyModificationOptions>, IDisposable
	{
		// Token: 0x17000835 RID: 2101
		// (set) Token: 0x06001D2E RID: 7470 RVA: 0x0002AB1C File Offset: 0x00028D1C
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000836 RID: 2102
		// (set) Token: 0x06001D2F RID: 7471 RVA: 0x0002AB2C File Offset: 0x00028D2C
		public Utf8String LobbyId
		{
			set
			{
				Helper.Set(value, ref this.m_LobbyId);
			}
		}

		// Token: 0x06001D30 RID: 7472 RVA: 0x0002AB3C File Offset: 0x00028D3C
		public void Set(ref UpdateLobbyModificationOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.LobbyId = other.LobbyId;
		}

		// Token: 0x06001D31 RID: 7473 RVA: 0x0002AB60 File Offset: 0x00028D60
		public void Set(ref UpdateLobbyModificationOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.LobbyId = other.Value.LobbyId;
			}
		}

		// Token: 0x06001D32 RID: 7474 RVA: 0x0002ABAB File Offset: 0x00028DAB
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_LobbyId);
		}

		// Token: 0x04000CB3 RID: 3251
		private int m_ApiVersion;

		// Token: 0x04000CB4 RID: 3252
		private IntPtr m_LocalUserId;

		// Token: 0x04000CB5 RID: 3253
		private IntPtr m_LobbyId;
	}
}
