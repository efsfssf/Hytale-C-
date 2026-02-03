using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatServer
{
	// Token: 0x0200067A RID: 1658
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyMessageToClientOptionsInternal : ISettable<AddNotifyMessageToClientOptions>, IDisposable
	{
		// Token: 0x06002B13 RID: 11027 RVA: 0x0003F568 File Offset: 0x0003D768
		public void Set(ref AddNotifyMessageToClientOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06002B14 RID: 11028 RVA: 0x0003F574 File Offset: 0x0003D774
		public void Set(ref AddNotifyMessageToClientOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06002B15 RID: 11029 RVA: 0x0003F595 File Offset: 0x0003D795
		public void Dispose()
		{
		}

		// Token: 0x04001275 RID: 4725
		private int m_ApiVersion;
	}
}
