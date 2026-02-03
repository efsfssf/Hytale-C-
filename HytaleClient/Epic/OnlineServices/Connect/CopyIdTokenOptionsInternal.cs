using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005CA RID: 1482
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyIdTokenOptionsInternal : ISettable<CopyIdTokenOptions>, IDisposable
	{
		// Token: 0x17000B32 RID: 2866
		// (set) Token: 0x0600269B RID: 9883 RVA: 0x0003929C File Offset: 0x0003749C
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x000392AC File Offset: 0x000374AC
		public void Set(ref CopyIdTokenOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x000392C4 File Offset: 0x000374C4
		public void Set(ref CopyIdTokenOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x000392FA File Offset: 0x000374FA
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x040010C0 RID: 4288
		private int m_ApiVersion;

		// Token: 0x040010C1 RID: 4289
		private IntPtr m_LocalUserId;
	}
}
