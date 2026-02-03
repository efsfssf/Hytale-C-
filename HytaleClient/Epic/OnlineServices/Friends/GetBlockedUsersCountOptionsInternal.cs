using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004DC RID: 1244
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetBlockedUsersCountOptionsInternal : ISettable<GetBlockedUsersCountOptions>, IDisposable
	{
		// Token: 0x17000930 RID: 2352
		// (set) Token: 0x0600205A RID: 8282 RVA: 0x0002F923 File Offset: 0x0002DB23
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x0600205B RID: 8283 RVA: 0x0002F933 File Offset: 0x0002DB33
		public void Set(ref GetBlockedUsersCountOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x0600205C RID: 8284 RVA: 0x0002F94C File Offset: 0x0002DB4C
		public void Set(ref GetBlockedUsersCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x0002F982 File Offset: 0x0002DB82
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000E1A RID: 3610
		private int m_ApiVersion;

		// Token: 0x04000E1B RID: 3611
		private IntPtr m_LocalUserId;
	}
}
