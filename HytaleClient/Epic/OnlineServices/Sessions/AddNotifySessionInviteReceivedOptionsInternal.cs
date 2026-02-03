using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x020000EB RID: 235
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifySessionInviteReceivedOptionsInternal : ISettable<AddNotifySessionInviteReceivedOptions>, IDisposable
	{
		// Token: 0x0600080A RID: 2058 RVA: 0x0000BAD0 File Offset: 0x00009CD0
		public void Set(ref AddNotifySessionInviteReceivedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0000BADC File Offset: 0x00009CDC
		public void Set(ref AddNotifySessionInviteReceivedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0000BAFD File Offset: 0x00009CFD
		public void Dispose()
		{
		}

		// Token: 0x040003DA RID: 986
		private int m_ApiVersion;
	}
}
