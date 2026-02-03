using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	// Token: 0x020004B6 RID: 1206
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CreateIntegratedPlatformOptionsContainerOptionsInternal : ISettable<CreateIntegratedPlatformOptionsContainerOptions>, IDisposable
	{
		// Token: 0x06001F69 RID: 8041 RVA: 0x0002DF8C File Offset: 0x0002C18C
		public void Set(ref CreateIntegratedPlatformOptionsContainerOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x0002DF98 File Offset: 0x0002C198
		public void Set(ref CreateIntegratedPlatformOptionsContainerOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x0002DFB9 File Offset: 0x0002C1B9
		public void Dispose()
		{
		}

		// Token: 0x04000DA2 RID: 3490
		private int m_ApiVersion;
	}
}
