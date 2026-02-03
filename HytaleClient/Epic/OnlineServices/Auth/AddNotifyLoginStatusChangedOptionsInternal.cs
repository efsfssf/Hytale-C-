using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x0200062C RID: 1580
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyLoginStatusChangedOptionsInternal : ISettable<AddNotifyLoginStatusChangedOptions>, IDisposable
	{
		// Token: 0x060028EE RID: 10478 RVA: 0x0003C133 File Offset: 0x0003A333
		public void Set(ref AddNotifyLoginStatusChangedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x060028EF RID: 10479 RVA: 0x0003C140 File Offset: 0x0003A340
		public void Set(ref AddNotifyLoginStatusChangedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x060028F0 RID: 10480 RVA: 0x0003C161 File Offset: 0x0003A361
		public void Dispose()
		{
		}

		// Token: 0x0400118D RID: 4493
		private int m_ApiVersion;
	}
}
