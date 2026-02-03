using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x0200057F RID: 1407
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyRequestToJoinAcceptedOptionsInternal : ISettable<AddNotifyRequestToJoinAcceptedOptions>, IDisposable
	{
		// Token: 0x06002490 RID: 9360 RVA: 0x00035D84 File Offset: 0x00033F84
		public void Set(ref AddNotifyRequestToJoinAcceptedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06002491 RID: 9361 RVA: 0x00035D90 File Offset: 0x00033F90
		public void Set(ref AddNotifyRequestToJoinAcceptedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06002492 RID: 9362 RVA: 0x00035DB1 File Offset: 0x00033FB1
		public void Dispose()
		{
		}

		// Token: 0x04001007 RID: 4103
		private int m_ApiVersion;
	}
}
