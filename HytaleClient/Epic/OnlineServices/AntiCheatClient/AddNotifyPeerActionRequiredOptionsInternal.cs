using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006D8 RID: 1752
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyPeerActionRequiredOptionsInternal : ISettable<AddNotifyPeerActionRequiredOptions>, IDisposable
	{
		// Token: 0x06002D57 RID: 11607 RVA: 0x00042F28 File Offset: 0x00041128
		public void Set(ref AddNotifyPeerActionRequiredOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06002D58 RID: 11608 RVA: 0x00042F34 File Offset: 0x00041134
		public void Set(ref AddNotifyPeerActionRequiredOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06002D59 RID: 11609 RVA: 0x00042F55 File Offset: 0x00041155
		public void Dispose()
		{
		}

		// Token: 0x040013F3 RID: 5107
		private int m_ApiVersion;
	}
}
