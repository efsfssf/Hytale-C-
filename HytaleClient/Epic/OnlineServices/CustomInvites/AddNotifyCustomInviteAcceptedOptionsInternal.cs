using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x02000579 RID: 1401
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyCustomInviteAcceptedOptionsInternal : ISettable<AddNotifyCustomInviteAcceptedOptions>, IDisposable
	{
		// Token: 0x06002487 RID: 9351 RVA: 0x00035CF6 File Offset: 0x00033EF6
		public void Set(ref AddNotifyCustomInviteAcceptedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06002488 RID: 9352 RVA: 0x00035D00 File Offset: 0x00033F00
		public void Set(ref AddNotifyCustomInviteAcceptedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06002489 RID: 9353 RVA: 0x00035D21 File Offset: 0x00033F21
		public void Dispose()
		{
		}

		// Token: 0x04001004 RID: 4100
		private int m_ApiVersion;
	}
}
