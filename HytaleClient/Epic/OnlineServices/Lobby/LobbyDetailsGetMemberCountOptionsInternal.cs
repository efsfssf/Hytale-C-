using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003CA RID: 970
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyDetailsGetMemberCountOptionsInternal : ISettable<LobbyDetailsGetMemberCountOptions>, IDisposable
	{
		// Token: 0x060019D6 RID: 6614 RVA: 0x00025F35 File Offset: 0x00024135
		public void Set(ref LobbyDetailsGetMemberCountOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x060019D7 RID: 6615 RVA: 0x00025F40 File Offset: 0x00024140
		public void Set(ref LobbyDetailsGetMemberCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x060019D8 RID: 6616 RVA: 0x00025F61 File Offset: 0x00024161
		public void Dispose()
		{
		}

		// Token: 0x04000B79 RID: 2937
		private int m_ApiVersion;
	}
}
