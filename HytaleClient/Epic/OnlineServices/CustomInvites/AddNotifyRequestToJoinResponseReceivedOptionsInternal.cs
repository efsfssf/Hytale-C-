using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x02000585 RID: 1413
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyRequestToJoinResponseReceivedOptionsInternal : ISettable<AddNotifyRequestToJoinResponseReceivedOptions>, IDisposable
	{
		// Token: 0x06002499 RID: 9369 RVA: 0x00035E14 File Offset: 0x00034014
		public void Set(ref AddNotifyRequestToJoinResponseReceivedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x00035E20 File Offset: 0x00034020
		public void Set(ref AddNotifyRequestToJoinResponseReceivedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x0600249B RID: 9371 RVA: 0x00035E41 File Offset: 0x00034041
		public void Dispose()
		{
		}

		// Token: 0x0400100A RID: 4106
		private int m_ApiVersion;
	}
}
