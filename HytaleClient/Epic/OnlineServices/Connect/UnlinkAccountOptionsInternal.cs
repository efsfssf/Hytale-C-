using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x02000622 RID: 1570
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UnlinkAccountOptionsInternal : ISettable<UnlinkAccountOptions>, IDisposable
	{
		// Token: 0x17000BCF RID: 3023
		// (set) Token: 0x06002897 RID: 10391 RVA: 0x0003B7EB File Offset: 0x000399EB
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06002898 RID: 10392 RVA: 0x0003B7FB File Offset: 0x000399FB
		public void Set(ref UnlinkAccountOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06002899 RID: 10393 RVA: 0x0003B814 File Offset: 0x00039A14
		public void Set(ref UnlinkAccountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x0600289A RID: 10394 RVA: 0x0003B84A File Offset: 0x00039A4A
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04001168 RID: 4456
		private int m_ApiVersion;

		// Token: 0x04001169 RID: 4457
		private IntPtr m_LocalUserId;
	}
}
