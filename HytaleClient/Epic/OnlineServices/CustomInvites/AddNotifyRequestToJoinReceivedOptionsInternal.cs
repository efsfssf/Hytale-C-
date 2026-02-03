using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x02000581 RID: 1409
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyRequestToJoinReceivedOptionsInternal : ISettable<AddNotifyRequestToJoinReceivedOptions>, IDisposable
	{
		// Token: 0x06002493 RID: 9363 RVA: 0x00035DB4 File Offset: 0x00033FB4
		public void Set(ref AddNotifyRequestToJoinReceivedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x00035DC0 File Offset: 0x00033FC0
		public void Set(ref AddNotifyRequestToJoinReceivedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06002495 RID: 9365 RVA: 0x00035DE1 File Offset: 0x00033FE1
		public void Dispose()
		{
		}

		// Token: 0x04001008 RID: 4104
		private int m_ApiVersion;
	}
}
