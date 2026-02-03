using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x0200009E RID: 158
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DeleteCacheOptionsInternal : ISettable<DeleteCacheOptions>, IDisposable
	{
		// Token: 0x1700010A RID: 266
		// (set) Token: 0x06000635 RID: 1589 RVA: 0x00008F93 File Offset: 0x00007193
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00008FA3 File Offset: 0x000071A3
		public void Set(ref DeleteCacheOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00008FBC File Offset: 0x000071BC
		public void Set(ref DeleteCacheOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00008FF2 File Offset: 0x000071F2
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000313 RID: 787
		private int m_ApiVersion;

		// Token: 0x04000314 RID: 788
		private IntPtr m_LocalUserId;
	}
}
