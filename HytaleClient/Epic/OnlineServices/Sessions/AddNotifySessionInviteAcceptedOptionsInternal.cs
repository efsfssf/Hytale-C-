using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x020000E9 RID: 233
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifySessionInviteAcceptedOptionsInternal : ISettable<AddNotifySessionInviteAcceptedOptions>, IDisposable
	{
		// Token: 0x06000807 RID: 2055 RVA: 0x0000BAA0 File Offset: 0x00009CA0
		public void Set(ref AddNotifySessionInviteAcceptedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0000BAAC File Offset: 0x00009CAC
		public void Set(ref AddNotifySessionInviteAcceptedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0000BACD File Offset: 0x00009CCD
		public void Dispose()
		{
		}

		// Token: 0x040003D9 RID: 985
		private int m_ApiVersion;
	}
}
