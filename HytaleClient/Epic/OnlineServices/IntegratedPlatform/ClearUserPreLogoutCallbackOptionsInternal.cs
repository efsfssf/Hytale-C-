using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	// Token: 0x020004B4 RID: 1204
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ClearUserPreLogoutCallbackOptionsInternal : ISettable<ClearUserPreLogoutCallbackOptions>, IDisposable
	{
		// Token: 0x06001F66 RID: 8038 RVA: 0x0002DF5C File Offset: 0x0002C15C
		public void Set(ref ClearUserPreLogoutCallbackOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x0002DF68 File Offset: 0x0002C168
		public void Set(ref ClearUserPreLogoutCallbackOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x0002DF89 File Offset: 0x0002C189
		public void Dispose()
		{
		}

		// Token: 0x04000DA1 RID: 3489
		private int m_ApiVersion;
	}
}
