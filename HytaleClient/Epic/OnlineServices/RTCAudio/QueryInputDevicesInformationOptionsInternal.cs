using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000257 RID: 599
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryInputDevicesInformationOptionsInternal : ISettable<QueryInputDevicesInformationOptions>, IDisposable
	{
		// Token: 0x060010BD RID: 4285 RVA: 0x00017DE8 File Offset: 0x00015FE8
		public void Set(ref QueryInputDevicesInformationOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x00017DF4 File Offset: 0x00015FF4
		public void Set(ref QueryInputDevicesInformationOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x00017E15 File Offset: 0x00016015
		public void Dispose()
		{
		}

		// Token: 0x04000737 RID: 1847
		private int m_ApiVersion;
	}
}
