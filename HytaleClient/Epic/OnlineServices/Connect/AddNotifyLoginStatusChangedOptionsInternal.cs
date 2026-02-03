using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005C5 RID: 1477
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyLoginStatusChangedOptionsInternal : ISettable<AddNotifyLoginStatusChangedOptions>, IDisposable
	{
		// Token: 0x0600265E RID: 9822 RVA: 0x000386A4 File Offset: 0x000368A4
		public void Set(ref AddNotifyLoginStatusChangedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x000386B0 File Offset: 0x000368B0
		public void Set(ref AddNotifyLoginStatusChangedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06002660 RID: 9824 RVA: 0x000386D1 File Offset: 0x000368D1
		public void Dispose()
		{
		}

		// Token: 0x0400109A RID: 4250
		private int m_ApiVersion;
	}
}
