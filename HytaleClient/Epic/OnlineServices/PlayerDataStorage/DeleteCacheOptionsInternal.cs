using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x020002F2 RID: 754
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DeleteCacheOptionsInternal : ISettable<DeleteCacheOptions>, IDisposable
	{
		// Token: 0x1700058D RID: 1421
		// (set) Token: 0x0600149F RID: 5279 RVA: 0x0001E17F File Offset: 0x0001C37F
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x0001E18F File Offset: 0x0001C38F
		public void Set(ref DeleteCacheOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x0001E1A8 File Offset: 0x0001C3A8
		public void Set(ref DeleteCacheOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x0001E1DE File Offset: 0x0001C3DE
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000911 RID: 2321
		private int m_ApiVersion;

		// Token: 0x04000912 RID: 2322
		private IntPtr m_LocalUserId;
	}
}
