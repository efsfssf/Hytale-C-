using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003F8 RID: 1016
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbySearchGetSearchResultCountOptionsInternal : ISettable<LobbySearchGetSearchResultCountOptions>, IDisposable
	{
		// Token: 0x06001B51 RID: 6993 RVA: 0x00029135 File Offset: 0x00027335
		public void Set(ref LobbySearchGetSearchResultCountOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06001B52 RID: 6994 RVA: 0x00029140 File Offset: 0x00027340
		public void Set(ref LobbySearchGetSearchResultCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06001B53 RID: 6995 RVA: 0x00029161 File Offset: 0x00027361
		public void Dispose()
		{
		}

		// Token: 0x04000C43 RID: 3139
		private int m_ApiVersion;
	}
}
