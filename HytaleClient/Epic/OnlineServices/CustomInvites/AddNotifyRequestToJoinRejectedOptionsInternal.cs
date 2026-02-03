using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x02000583 RID: 1411
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyRequestToJoinRejectedOptionsInternal : ISettable<AddNotifyRequestToJoinRejectedOptions>, IDisposable
	{
		// Token: 0x06002496 RID: 9366 RVA: 0x00035DE4 File Offset: 0x00033FE4
		public void Set(ref AddNotifyRequestToJoinRejectedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x00035DF0 File Offset: 0x00033FF0
		public void Set(ref AddNotifyRequestToJoinRejectedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06002498 RID: 9368 RVA: 0x00035E11 File Offset: 0x00034011
		public void Dispose()
		{
		}

		// Token: 0x04001009 RID: 4105
		private int m_ApiVersion;
	}
}
