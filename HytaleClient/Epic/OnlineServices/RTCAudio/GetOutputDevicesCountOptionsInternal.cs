using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000221 RID: 545
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetOutputDevicesCountOptionsInternal : ISettable<GetOutputDevicesCountOptions>, IDisposable
	{
		// Token: 0x06000F87 RID: 3975 RVA: 0x00016D08 File Offset: 0x00014F08
		public void Set(ref GetOutputDevicesCountOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x00016D14 File Offset: 0x00014F14
		public void Set(ref GetOutputDevicesCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x00016D35 File Offset: 0x00014F35
		public void Dispose()
		{
		}

		// Token: 0x040006FC RID: 1788
		private int m_ApiVersion;
	}
}
