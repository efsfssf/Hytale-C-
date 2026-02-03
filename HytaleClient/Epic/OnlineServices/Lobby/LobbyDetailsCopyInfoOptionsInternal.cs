using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003BA RID: 954
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyDetailsCopyInfoOptionsInternal : ISettable<LobbyDetailsCopyInfoOptions>, IDisposable
	{
		// Token: 0x060019A9 RID: 6569 RVA: 0x00025BB9 File Offset: 0x00023DB9
		public void Set(ref LobbyDetailsCopyInfoOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x00025BC4 File Offset: 0x00023DC4
		public void Set(ref LobbyDetailsCopyInfoOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x00025BE5 File Offset: 0x00023DE5
		public void Dispose()
		{
		}

		// Token: 0x04000B63 RID: 2915
		private int m_ApiVersion;
	}
}
