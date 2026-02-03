using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004E0 RID: 1248
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetFriendsCountOptionsInternal : ISettable<GetFriendsCountOptions>, IDisposable
	{
		// Token: 0x17000936 RID: 2358
		// (set) Token: 0x06002069 RID: 8297 RVA: 0x0002FA5F File Offset: 0x0002DC5F
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x0600206A RID: 8298 RVA: 0x0002FA6F File Offset: 0x0002DC6F
		public void Set(ref GetFriendsCountOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x0600206B RID: 8299 RVA: 0x0002FA88 File Offset: 0x0002DC88
		public void Set(ref GetFriendsCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x0600206C RID: 8300 RVA: 0x0002FABE File Offset: 0x0002DCBE
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000E22 RID: 3618
		private int m_ApiVersion;

		// Token: 0x04000E23 RID: 3619
		private IntPtr m_LocalUserId;
	}
}
