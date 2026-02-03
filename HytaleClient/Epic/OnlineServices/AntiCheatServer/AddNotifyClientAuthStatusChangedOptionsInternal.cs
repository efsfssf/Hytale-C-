using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatServer
{
	// Token: 0x02000678 RID: 1656
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyClientAuthStatusChangedOptionsInternal : ISettable<AddNotifyClientAuthStatusChangedOptions>, IDisposable
	{
		// Token: 0x06002B10 RID: 11024 RVA: 0x0003F538 File Offset: 0x0003D738
		public void Set(ref AddNotifyClientAuthStatusChangedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06002B11 RID: 11025 RVA: 0x0003F544 File Offset: 0x0003D744
		public void Set(ref AddNotifyClientAuthStatusChangedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06002B12 RID: 11026 RVA: 0x0003F565 File Offset: 0x0003D765
		public void Dispose()
		{
		}

		// Token: 0x04001274 RID: 4724
		private int m_ApiVersion;
	}
}
