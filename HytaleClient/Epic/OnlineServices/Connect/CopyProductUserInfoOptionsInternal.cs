using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005D2 RID: 1490
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyProductUserInfoOptionsInternal : ISettable<CopyProductUserInfoOptions>, IDisposable
	{
		// Token: 0x17000B40 RID: 2880
		// (set) Token: 0x060026BC RID: 9916 RVA: 0x0003955F File Offset: 0x0003775F
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x060026BD RID: 9917 RVA: 0x0003956F File Offset: 0x0003776F
		public void Set(ref CopyProductUserInfoOptions other)
		{
			this.m_ApiVersion = 1;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x060026BE RID: 9918 RVA: 0x00039588 File Offset: 0x00037788
		public void Set(ref CopyProductUserInfoOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x060026BF RID: 9919 RVA: 0x000395BE File Offset: 0x000377BE
		public void Dispose()
		{
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x040010D2 RID: 4306
		private int m_ApiVersion;

		// Token: 0x040010D3 RID: 4307
		private IntPtr m_TargetUserId;
	}
}
