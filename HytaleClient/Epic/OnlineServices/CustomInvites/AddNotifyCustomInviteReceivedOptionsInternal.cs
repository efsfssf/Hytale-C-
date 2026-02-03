using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x0200057B RID: 1403
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyCustomInviteReceivedOptionsInternal : ISettable<AddNotifyCustomInviteReceivedOptions>, IDisposable
	{
		// Token: 0x0600248A RID: 9354 RVA: 0x00035D24 File Offset: 0x00033F24
		public void Set(ref AddNotifyCustomInviteReceivedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x0600248B RID: 9355 RVA: 0x00035D30 File Offset: 0x00033F30
		public void Set(ref AddNotifyCustomInviteReceivedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x00035D51 File Offset: 0x00033F51
		public void Dispose()
		{
		}

		// Token: 0x04001005 RID: 4101
		private int m_ApiVersion;
	}
}
