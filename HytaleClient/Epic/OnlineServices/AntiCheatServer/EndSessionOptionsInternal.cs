using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatServer
{
	// Token: 0x0200067F RID: 1663
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct EndSessionOptionsInternal : ISettable<EndSessionOptions>, IDisposable
	{
		// Token: 0x06002B46 RID: 11078 RVA: 0x0003FE54 File Offset: 0x0003E054
		public void Set(ref EndSessionOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06002B47 RID: 11079 RVA: 0x0003FE60 File Offset: 0x0003E060
		public void Set(ref EndSessionOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06002B48 RID: 11080 RVA: 0x0003FE81 File Offset: 0x0003E081
		public void Dispose()
		{
		}

		// Token: 0x0400128E RID: 4750
		private int m_ApiVersion;
	}
}
