using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x020000ED RID: 237
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifySessionInviteRejectedOptionsInternal : ISettable<AddNotifySessionInviteRejectedOptions>, IDisposable
	{
		// Token: 0x0600080D RID: 2061 RVA: 0x0000BB00 File Offset: 0x00009D00
		public void Set(ref AddNotifySessionInviteRejectedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x0000BB0C File Offset: 0x00009D0C
		public void Set(ref AddNotifySessionInviteRejectedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0000BB2D File Offset: 0x00009D2D
		public void Dispose()
		{
		}

		// Token: 0x040003DB RID: 987
		private int m_ApiVersion;
	}
}
