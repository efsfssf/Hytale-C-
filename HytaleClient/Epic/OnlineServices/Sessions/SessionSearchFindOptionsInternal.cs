using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000179 RID: 377
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionSearchFindOptionsInternal : ISettable<SessionSearchFindOptions>, IDisposable
	{
		// Token: 0x1700028A RID: 650
		// (set) Token: 0x06000B22 RID: 2850 RVA: 0x0000FDD2 File Offset: 0x0000DFD2
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x0000FDE2 File Offset: 0x0000DFE2
		public void Set(ref SessionSearchFindOptions other)
		{
			this.m_ApiVersion = 2;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x0000FDFC File Offset: 0x0000DFFC
		public void Set(ref SessionSearchFindOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x0000FE32 File Offset: 0x0000E032
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000514 RID: 1300
		private int m_ApiVersion;

		// Token: 0x04000515 RID: 1301
		private IntPtr m_LocalUserId;
	}
}
