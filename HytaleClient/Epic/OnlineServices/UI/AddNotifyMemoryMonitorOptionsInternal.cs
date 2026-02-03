using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000052 RID: 82
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyMemoryMonitorOptionsInternal : ISettable<AddNotifyMemoryMonitorOptions>, IDisposable
	{
		// Token: 0x0600048C RID: 1164 RVA: 0x00006AA4 File Offset: 0x00004CA4
		public void Set(ref AddNotifyMemoryMonitorOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00006AB0 File Offset: 0x00004CB0
		public void Set(ref AddNotifyMemoryMonitorOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00006AD1 File Offset: 0x00004CD1
		public void Dispose()
		{
		}

		// Token: 0x040001DD RID: 477
		private int m_ApiVersion;
	}
}
