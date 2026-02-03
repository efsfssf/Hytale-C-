using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x02000484 RID: 1156
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyPermissionsUpdateReceivedOptionsInternal : ISettable<AddNotifyPermissionsUpdateReceivedOptions>, IDisposable
	{
		// Token: 0x06001E38 RID: 7736 RVA: 0x0002C420 File Offset: 0x0002A620
		public void Set(ref AddNotifyPermissionsUpdateReceivedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06001E39 RID: 7737 RVA: 0x0002C42C File Offset: 0x0002A62C
		public void Set(ref AddNotifyPermissionsUpdateReceivedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06001E3A RID: 7738 RVA: 0x0002C44D File Offset: 0x0002A64D
		public void Dispose()
		{
		}

		// Token: 0x04000D2F RID: 3375
		private int m_ApiVersion;
	}
}
