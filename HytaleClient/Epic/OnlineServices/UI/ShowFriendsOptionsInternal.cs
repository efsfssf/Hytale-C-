using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x0200008D RID: 141
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ShowFriendsOptionsInternal : ISettable<ShowFriendsOptions>, IDisposable
	{
		// Token: 0x170000E8 RID: 232
		// (set) Token: 0x060005BD RID: 1469 RVA: 0x00008047 File Offset: 0x00006247
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00008057 File Offset: 0x00006257
		public void Set(ref ShowFriendsOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00008070 File Offset: 0x00006270
		public void Set(ref ShowFriendsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x000080A6 File Offset: 0x000062A6
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x040002CE RID: 718
		private int m_ApiVersion;

		// Token: 0x040002CF RID: 719
		private IntPtr m_LocalUserId;
	}
}
