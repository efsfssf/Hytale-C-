using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatServer
{
	// Token: 0x02000676 RID: 1654
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyClientActionRequiredOptionsInternal : ISettable<AddNotifyClientActionRequiredOptions>, IDisposable
	{
		// Token: 0x06002B0D RID: 11021 RVA: 0x0003F507 File Offset: 0x0003D707
		public void Set(ref AddNotifyClientActionRequiredOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06002B0E RID: 11022 RVA: 0x0003F514 File Offset: 0x0003D714
		public void Set(ref AddNotifyClientActionRequiredOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06002B0F RID: 11023 RVA: 0x0003F535 File Offset: 0x0003D735
		public void Dispose()
		{
		}

		// Token: 0x04001273 RID: 4723
		private int m_ApiVersion;
	}
}
