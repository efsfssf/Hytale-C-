using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200016A RID: 362
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionModificationSetHostAddressOptionsInternal : ISettable<SessionModificationSetHostAddressOptions>, IDisposable
	{
		// Token: 0x17000279 RID: 633
		// (set) Token: 0x06000AE3 RID: 2787 RVA: 0x0000F772 File Offset: 0x0000D972
		public Utf8String HostAddress
		{
			set
			{
				Helper.Set(value, ref this.m_HostAddress);
			}
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x0000F782 File Offset: 0x0000D982
		public void Set(ref SessionModificationSetHostAddressOptions other)
		{
			this.m_ApiVersion = 1;
			this.HostAddress = other.HostAddress;
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x0000F79C File Offset: 0x0000D99C
		public void Set(ref SessionModificationSetHostAddressOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.HostAddress = other.Value.HostAddress;
			}
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x0000F7D2 File Offset: 0x0000D9D2
		public void Dispose()
		{
			Helper.Dispose(ref this.m_HostAddress);
		}

		// Token: 0x040004F6 RID: 1270
		private int m_ApiVersion;

		// Token: 0x040004F7 RID: 1271
		private IntPtr m_HostAddress;
	}
}
