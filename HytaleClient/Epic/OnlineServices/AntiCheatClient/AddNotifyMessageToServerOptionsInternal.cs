using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006D6 RID: 1750
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyMessageToServerOptionsInternal : ISettable<AddNotifyMessageToServerOptions>, IDisposable
	{
		// Token: 0x06002D54 RID: 11604 RVA: 0x00042EF8 File Offset: 0x000410F8
		public void Set(ref AddNotifyMessageToServerOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06002D55 RID: 11605 RVA: 0x00042F04 File Offset: 0x00041104
		public void Set(ref AddNotifyMessageToServerOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06002D56 RID: 11606 RVA: 0x00042F25 File Offset: 0x00041125
		public void Dispose()
		{
		}

		// Token: 0x040013F2 RID: 5106
		private int m_ApiVersion;
	}
}
