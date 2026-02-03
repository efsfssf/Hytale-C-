using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	// Token: 0x020004C8 RID: 1224
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SetUserPreLogoutCallbackOptionsInternal : ISettable<SetUserPreLogoutCallbackOptions>, IDisposable
	{
		// Token: 0x06001FBA RID: 8122 RVA: 0x0002E697 File Offset: 0x0002C897
		public void Set(ref SetUserPreLogoutCallbackOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x0002E6A4 File Offset: 0x0002C8A4
		public void Set(ref SetUserPreLogoutCallbackOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x0002E6C5 File Offset: 0x0002C8C5
		public void Dispose()
		{
		}

		// Token: 0x04000DD1 RID: 3537
		private int m_ApiVersion;
	}
}
