using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000056 RID: 86
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetFriendsVisibleOptionsInternal : ISettable<GetFriendsVisibleOptions>, IDisposable
	{
		// Token: 0x1700008A RID: 138
		// (set) Token: 0x06000497 RID: 1175 RVA: 0x00006B62 File Offset: 0x00004D62
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00006B72 File Offset: 0x00004D72
		public void Set(ref GetFriendsVisibleOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00006B8C File Offset: 0x00004D8C
		public void Set(ref GetFriendsVisibleOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00006BC2 File Offset: 0x00004DC2
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x040001E2 RID: 482
		private int m_ApiVersion;

		// Token: 0x040001E3 RID: 483
		private IntPtr m_LocalUserId;
	}
}
