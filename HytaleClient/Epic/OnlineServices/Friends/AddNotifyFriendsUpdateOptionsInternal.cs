using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004D6 RID: 1238
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyFriendsUpdateOptionsInternal : ISettable<AddNotifyFriendsUpdateOptions>, IDisposable
	{
		// Token: 0x06002037 RID: 8247 RVA: 0x0002F370 File Offset: 0x0002D570
		public void Set(ref AddNotifyFriendsUpdateOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06002038 RID: 8248 RVA: 0x0002F37C File Offset: 0x0002D57C
		public void Set(ref AddNotifyFriendsUpdateOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x0002F39D File Offset: 0x0002D59D
		public void Dispose()
		{
		}

		// Token: 0x04000E03 RID: 3587
		private int m_ApiVersion;
	}
}
