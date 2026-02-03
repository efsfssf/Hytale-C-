using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x020004AC RID: 1196
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RequestPermissionsOptionsInternal : ISettable<RequestPermissionsOptions>, IDisposable
	{
		// Token: 0x170008D9 RID: 2265
		// (set) Token: 0x06001F42 RID: 8002 RVA: 0x0002DBE0 File Offset: 0x0002BDE0
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170008DA RID: 2266
		// (set) Token: 0x06001F43 RID: 8003 RVA: 0x0002DBF0 File Offset: 0x0002BDF0
		public Utf8String[] PermissionKeys
		{
			set
			{
				Helper.Set<Utf8String>(value, ref this.m_PermissionKeys, true, out this.m_PermissionKeyCount);
			}
		}

		// Token: 0x06001F44 RID: 8004 RVA: 0x0002DC07 File Offset: 0x0002BE07
		public void Set(ref RequestPermissionsOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.PermissionKeys = other.PermissionKeys;
		}

		// Token: 0x06001F45 RID: 8005 RVA: 0x0002DC2C File Offset: 0x0002BE2C
		public void Set(ref RequestPermissionsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.PermissionKeys = other.Value.PermissionKeys;
			}
		}

		// Token: 0x06001F46 RID: 8006 RVA: 0x0002DC77 File Offset: 0x0002BE77
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_PermissionKeys);
		}

		// Token: 0x04000D91 RID: 3473
		private int m_ApiVersion;

		// Token: 0x04000D92 RID: 3474
		private IntPtr m_LocalUserId;

		// Token: 0x04000D93 RID: 3475
		private uint m_PermissionKeyCount;

		// Token: 0x04000D94 RID: 3476
		private IntPtr m_PermissionKeys;
	}
}
