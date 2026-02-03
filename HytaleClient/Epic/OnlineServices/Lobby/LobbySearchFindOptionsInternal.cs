using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003F6 RID: 1014
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbySearchFindOptionsInternal : ISettable<LobbySearchFindOptions>, IDisposable
	{
		// Token: 0x170007C8 RID: 1992
		// (set) Token: 0x06001B4D RID: 6989 RVA: 0x000290C6 File Offset: 0x000272C6
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x000290D6 File Offset: 0x000272D6
		public void Set(ref LobbySearchFindOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x000290F0 File Offset: 0x000272F0
		public void Set(ref LobbySearchFindOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x00029126 File Offset: 0x00027326
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000C41 RID: 3137
		private int m_ApiVersion;

		// Token: 0x04000C42 RID: 3138
		private IntPtr m_LocalUserId;
	}
}
