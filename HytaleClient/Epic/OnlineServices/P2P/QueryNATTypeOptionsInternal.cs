using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x020007A5 RID: 1957
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryNATTypeOptionsInternal : ISettable<QueryNATTypeOptions>, IDisposable
	{
		// Token: 0x060032C3 RID: 12995 RVA: 0x0004BC40 File Offset: 0x00049E40
		public void Set(ref QueryNATTypeOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x060032C4 RID: 12996 RVA: 0x0004BC4C File Offset: 0x00049E4C
		public void Set(ref QueryNATTypeOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x060032C5 RID: 12997 RVA: 0x0004BC6D File Offset: 0x00049E6D
		public void Dispose()
		{
		}

		// Token: 0x040016C0 RID: 5824
		private int m_ApiVersion;
	}
}
