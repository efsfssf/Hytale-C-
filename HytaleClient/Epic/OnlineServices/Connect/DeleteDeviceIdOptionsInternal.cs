using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005E0 RID: 1504
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DeleteDeviceIdOptionsInternal : ISettable<DeleteDeviceIdOptions>, IDisposable
	{
		// Token: 0x0600270A RID: 9994 RVA: 0x00039C69 File Offset: 0x00037E69
		public void Set(ref DeleteDeviceIdOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x0600270B RID: 9995 RVA: 0x00039C74 File Offset: 0x00037E74
		public void Set(ref DeleteDeviceIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x00039C95 File Offset: 0x00037E95
		public void Dispose()
		{
		}

		// Token: 0x040010ED RID: 4333
		private int m_ApiVersion;
	}
}
