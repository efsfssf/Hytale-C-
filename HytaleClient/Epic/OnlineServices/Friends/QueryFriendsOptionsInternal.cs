using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004F6 RID: 1270
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryFriendsOptionsInternal : ISettable<QueryFriendsOptions>, IDisposable
	{
		// Token: 0x17000957 RID: 2391
		// (set) Token: 0x060020ED RID: 8429 RVA: 0x0003029F File Offset: 0x0002E49F
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x060020EE RID: 8430 RVA: 0x000302AF File Offset: 0x0002E4AF
		public void Set(ref QueryFriendsOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x000302C8 File Offset: 0x0002E4C8
		public void Set(ref QueryFriendsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x000302FE File Offset: 0x0002E4FE
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000E42 RID: 3650
		private int m_ApiVersion;

		// Token: 0x04000E43 RID: 3651
		private IntPtr m_LocalUserId;
	}
}
