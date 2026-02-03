using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004D4 RID: 1236
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyBlockedUsersUpdateOptionsInternal : ISettable<AddNotifyBlockedUsersUpdateOptions>, IDisposable
	{
		// Token: 0x06002034 RID: 8244 RVA: 0x0002F342 File Offset: 0x0002D542
		public void Set(ref AddNotifyBlockedUsersUpdateOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06002035 RID: 8245 RVA: 0x0002F34C File Offset: 0x0002D54C
		public void Set(ref AddNotifyBlockedUsersUpdateOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x0002F36D File Offset: 0x0002D56D
		public void Dispose()
		{
		}

		// Token: 0x04000E02 RID: 3586
		private int m_ApiVersion;
	}
}
