using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000259 RID: 601
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryOutputDevicesInformationOptionsInternal : ISettable<QueryOutputDevicesInformationOptions>, IDisposable
	{
		// Token: 0x060010C0 RID: 4288 RVA: 0x00017E18 File Offset: 0x00016018
		public void Set(ref QueryOutputDevicesInformationOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x00017E24 File Offset: 0x00016024
		public void Set(ref QueryOutputDevicesInformationOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x00017E45 File Offset: 0x00016045
		public void Dispose()
		{
		}

		// Token: 0x04000738 RID: 1848
		private int m_ApiVersion;
	}
}
