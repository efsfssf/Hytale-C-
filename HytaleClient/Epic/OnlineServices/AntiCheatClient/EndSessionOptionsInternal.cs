using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006E1 RID: 1761
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct EndSessionOptionsInternal : ISettable<EndSessionOptions>, IDisposable
	{
		// Token: 0x06002D83 RID: 11651 RVA: 0x000436FA File Offset: 0x000418FA
		public void Set(ref EndSessionOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06002D84 RID: 11652 RVA: 0x00043704 File Offset: 0x00041904
		public void Set(ref EndSessionOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06002D85 RID: 11653 RVA: 0x00043725 File Offset: 0x00041925
		public void Dispose()
		{
		}

		// Token: 0x04001425 RID: 5157
		private int m_ApiVersion;
	}
}
